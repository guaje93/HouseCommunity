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


        public async Task<Building> GetBuilding(int userId)
        {
            var building = await _context.Buildings.Include(p => p.HouseManager)
                                                   .Include(p => p.Address)
                                                   .Include(p => p.Flats)
                                                   .ThenInclude(p=> p.Residents)
                                                   .FirstOrDefaultAsync(p => p.HouseManager.Id == userId);
            return building;
        }

        public async Task<Flat> GetFlat(int flatId)
        {
            var flat = await _context.Flats.Include(p => p.Residents).FirstOrDefaultAsync(p=> p.Id == flatId);
            return flat;
        }

        public async Task<ICollection<FlatsForListDTO>> GetFlats(int userId)
        {
            var building = await GetBuilding(userId);
            return building.Flats.Select(p => new FlatsForListDTO()
            {
                Id = p.Id,
                Address = p.Building.Address.ToString() + " m." + p.FlatNumber,
                Residents = p.Residents.Select(r => new UserNamesListDTO()
                {
                    Id = r.Id,
                    Email = r.Email,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    PhoneNumber = r.PhoneNumber
                }).ToList()
            }).ToList();
        }
    }
}
