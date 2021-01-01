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
    public class DamageRepository : IDamageRepository
    {
        private readonly DataContext _dataContext;

        public DamageRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<Damage> AddDamage(AddDamageDTO addDamageDTO)
        {
            var user = await _dataContext.Users.Include(p => p.UserFlats)
                                               .ThenInclude(p => p.Flat) 
                                               .ThenInclude(p => p.Building)
                                               .FirstOrDefaultAsync(p => p.Id == addDamageDTO.UserId);

            var building = await _dataContext.Buildings.FirstOrDefaultAsync(p => p.Id == addDamageDTO.BuildingId);

            
            var damage = new Damage()
            {
                Building = building,
                RequestCreator = user,
                CreationDate = DateTime.Now,
                Description = addDamageDTO.Description,
                Status = DamageStatus.WaitingForFix,
                Title = addDamageDTO.Title,
            };
            _dataContext.Add(damage);
            await _dataContext.SaveChangesAsync();
            return damage;

        }

        public async Task<Damage> AddImage(AddImageDTO addDamageDTO)
        {
            var damage = await _dataContext.Damages.FirstOrDefaultAsync(p => p.Id == addDamageDTO.Id);
            if (damage.BlobFiles == null)
                damage.BlobFiles = new List<BlobFile>();

            damage.BlobFiles.Add(new BlobFile()
            {
                FileName = addDamageDTO.FileName,
                FileUrl = addDamageDTO.FileUrl,
            });
            await _dataContext.SaveChangesAsync();
            return damage;
        }

        public IEnumerable<Damage> GetDamagesForHouseManager(int id, DamageStatus status)
        {
            var buildingId = _dataContext.Buildings.Include(prop => prop.HouseManager).Where(p => p.HouseManager.Id == id).Select(p => p.Id);
            var damages = _dataContext.Damages.Include(p => p.Building)
                                              .ThenInclude(p => p.Flats)
                                              .ThenInclude(p => p)
                                              .Include(p => p.Building)
                                              .ThenInclude(p => p.Address)
                                              .Include(p => p.BlobFiles)
                                              .Include(p => p.RequestCreator)
                                              .Where(p => buildingId.Contains(p.Building.Id))
                                              .Where(p => p.Status == status);
            return damages;
        }

        public async Task<Damage> ChangeStatus(int id, DamageStatus status)
        {
            var damage = await _dataContext.Damages.Include(p => p.Building).ThenInclude(p => p.HouseManager).FirstOrDefaultAsync(p => p.Id == id);
            damage.Status = status;
            await _dataContext.SaveChangesAsync();
            return damage;
        }
    }
}
