using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Request;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public class UserRepository : IUserRepository
    {
        #region Fields

        private readonly DataContext _context;

        #endregion //Fields

        #region Constructors

        public UserRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        #endregion //Constructors

        #region Methods

        public async Task<UserForInfoDTO> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Flat)
                                           .ThenInclude(p => p.Building)
                                           .ThenInclude(p => p.Cost)
                                           .FirstOrDefaultAsync(p => p.Id == id);

            if (user == null)
                return null;

            return new UserForInfoDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthdate = user.Birthdate,
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Area = user.Flat.Area,
                ResidentsAmount = user.Flat.ResidentsAmount,
                ColdWaterEstimatedUsage = user.Flat.ColdWaterEstimatedUsage,
                HotWaterEstimatedUsage = user.Flat.HotWaterEstimatedUsage,
                HeatingEstimatedUsage = user.Flat.HeatingEstimatedUsage,
                ColdWaterUnitCost = user.Flat.Building.Cost.ColdWaterUnitCost,
                HotWaterUnitCost = user.Flat.Building.Cost.HotWaterUnitCost,
                HeatingUnitCost = user.Flat.Building.Cost.HeatingUnitCost,

            };
        }

        public async Task<ICollection<ResidentsForListDTO>> GetResidents()
        {
            var users = await _context.Users
                                      .Include(p => p.Flat)
                                      .ThenInclude(p => p.Building)
                                      .ThenInclude(p => p.HousingDevelopment)
                                      .Where(p => p.UserRole == UserRole.Resident)
                                      .Select(user => new ResidentsForListDTO()
                                      {
                                          HousingDevelopmentId = user.Flat.Building.HousingDevelopment.Id,
                                          HousingDevelopmentName = user.Flat.Building.HousingDevelopment.Name,
                                          UserId = user.Id,
                                          UserEmail = user.Email,
                                          BuildingId = user.Flat.BuildingId,
                                          FlatId = user.Flat.Id,
                                          LocalNumber =  user.Flat.FlatNumber, 
                                          Address = user.Flat.Building.Address.ToString(),
                                          Name = $"{user.FirstName} {user.LastName}"
                                      }).ToListAsync();
            return users;
        }

        public async Task<User> UpdateUserDefinedData(UserDefinedData userContactData)
        {
            var user = await _context.Users.Include(p => p.Flat)
                                           .FirstOrDefaultAsync(p => p.Id == userContactData.Id);
            if (user == null)
                return null;

            user.PhoneNumber = userContactData.PhoneNumber;
            user.Email = userContactData.Email;
            user.Flat.ResidentsAmount = userContactData.ResidentsAmount;
            user.Flat.ColdWaterEstimatedUsage = userContactData.ColdWaterEstimatedUsage;
            user.Flat.HotWaterEstimatedUsage = userContactData.HotWaterEstimatedUsage;
            user.Flat.HeatingEstimatedUsage = userContactData.HeatingEstimatedUsage;

            _context.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        #endregion //Methods
    }
}
