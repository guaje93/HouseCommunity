using HouseCommunity.Data.Interfaces;
using HouseCommunity.DTOs;
using HouseCommunity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public class BuildingRepository : IBuildingRepository
    {
        #region Fields

        private readonly DataContext _context;

        #endregion //Fields

        #region Constructors

        public BuildingRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        #endregion //Constructors


        public async Task<Building> GetBuildingByManager(User manager)
        {
            var building = await _context.Buildings.Include(p => p.HouseManager)
                                                   .Include(p => p.Address)
                                                   .Include(p => p.Flats)
                                                   .ThenInclude(p=> p.Residents)
                                                   .ThenInclude(p => p.Flat)
                                                   .Include(p => p.Flats)
                                                   .ThenInclude(p => p.Residents)
                                                   .ThenInclude(p => p.User)
                                                   .FirstOrDefaultAsync(p => p.HouseManager == manager);
            return building;
        }

        public async Task<Building> GetBuilding(int id)
        {
            var building = await _context.Buildings.Include(p => p.HouseManager)
                                                   .Include(p => p.Address)
                                                   .Include(p => p.Flats)
                                                   .ThenInclude(p => p.Residents)
                                                   .ThenInclude(p => p.Flat)
                                                   .Include(p => p.Flats)
                                                   .ThenInclude(p => p.Residents)
                                                   .ThenInclude(p => p.User)
                                                   .FirstOrDefaultAsync(p => p.HouseManager.Id == id);
            return building;
        }

        public async Task<ICollection<Building>> GetBuildings()
        {
           var buildings = await _context.Buildings.Include(p => p.Address).ToListAsync();
            return buildings;
        }

        public async Task<Flat> GetFlat(int flatId)
        {
            var flat = await _context.Flats.Include(p => p.Residents)
                                            .ThenInclude(p => p.User)
                                            .Include(p => p.Building)
                                            .ThenInclude(p => p.HousingDevelopment)
                                            .Include(p => p.Building)
                                            .ThenInclude(p => p.Address)
                                            .Include(p => p.Building)
                                            .ThenInclude(p => p.Cost)
                                            .FirstOrDefaultAsync(p=> p.Id == flatId);
            return flat;
        }

        public async Task<ICollection<Flat>> GetFlats()
        {
            var flats = await _context.Flats.Include(p => p.Residents)
                                            .ThenInclude(p => p.User)
                                            .Include(p=>p.Building)
                                            .ThenInclude(p => p.HousingDevelopment)
                                            .Include(p => p.Building)
                                            .ThenInclude(p => p.Address)
                                            .Include(p => p.Building)
                                            .ThenInclude(p => p.HouseManager)
                                            .ToListAsync();
            return flats;
        }

        public async Task<Flat> RegisterFlat(UserForRegisterExistingDTO userForRegisterDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Email == userForRegisterDTO.Email);
            var flat = await _context.Flats.FirstOrDefaultAsync(p => p.Id == userForRegisterDTO.FlatId);
            user.UserFlats.Add(new UserFlat()
            {
                Flat = flat
            });
            await _context.SaveChangesAsync();
            return flat;
        }

        public async Task<Flat> UpdateFlat(Flat flat)
        {
            _context.Update(flat);
            await _context.SaveChangesAsync();
            return flat;
        }
    }
}
