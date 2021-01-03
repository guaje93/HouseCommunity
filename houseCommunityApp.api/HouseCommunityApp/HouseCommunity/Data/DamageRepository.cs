using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseCommunity.Data.Interfaces;
using HouseCommunity.DTOs;
using HouseCommunity.Model;
using Microsoft.EntityFrameworkCore;

namespace HouseCommunity.Data
{
    public class DamageRepository : BaseRepository<Damage>, IDamageRepository
    {

        #region Constructor

        public DamageRepository(DataContext dataContext) : base(dataContext)
        {
        }

        #endregion //Constructor


        public async Task<Damage> AddDamage(Damage damage)
        {
            await AddAsync(damage);
            return damage;
        }

        public async Task<Damage> UpdateDamage(Damage damage)
        {
            await UpdateAsync(damage);
            return damage;
        }

        public IEnumerable<Damage> GetDamagesByUserAndStatus(User user, DamageStatus status)
        {
            var buildingId = _context.Buildings
                                     .Include(prop => prop.HouseManager)
                                     .Where(p => p.HouseManager == user)
                                     .Select(p => p.Id);

            var damages =  GetAll(x => x.Include(p => p.Building)
                                              .ThenInclude(p => p.Flats)
                                              .ThenInclude(p => p)
                                              .Include(p => p.Building)
                                              .ThenInclude(p => p.Address)
                                              .Include(p => p.BlobFiles)
                                              .Include(p => p.RequestCreator));
            damages = damages.Where(p => buildingId.Contains(p.Building.Id))
                             .Where(p => p.Status == status);
            return damages;
        }

        public async Task<Damage> GetDamage(int id)
        {
            var damage = await GetById(id, x => x.Include(p => p.Building)
                                            .ThenInclude(p => p.Flats)
                                            .ThenInclude(p => p)
                                            .Include(p => p.Building)
                                            .ThenInclude(p => p.Address)
                                            .Include(p => p.BlobFiles)
                                            .Include(p => p.RequestCreator)
                                            .Include(p => p.Building)
                                            .ThenInclude(p => p.HouseManager));
            return damage;
        }

        
    }
}
