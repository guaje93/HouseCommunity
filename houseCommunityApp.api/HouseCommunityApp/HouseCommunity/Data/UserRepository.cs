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

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Flat)
                                           .ThenInclude(p => p.Building)
                                           .ThenInclude(p => p.Cost)
                                           .Include(p => p.UserConversations)
                                           .ThenInclude(p => p.Conversation)
                                           .ThenInclude(p => p.Users)
                                           .Include(p => p.UserConversations)
                                           .ThenInclude(p => p.Conversation)
                                           .ThenInclude(p => p.Messages)
                                           .ThenInclude(p => p.Sender)
                                           .FirstOrDefaultAsync(p => p.Id == id);

            if (user == null)
                return null;

            return user;
        }

        public ICollection<User> GetUsers()
        {
            var users = _context.Users
                                      .Include(p => p.Flat)
                                      .ThenInclude(p => p.Building)
                                      .ThenInclude(p => p.Address)
                                      .Include(p => p.Flat)
                                      .ThenInclude(p => p.Building)
                                      .ThenInclude(p => p.HousingDevelopment).ToList();
            return users;
        }

        public async Task<ICollection<User>> GetUsersWithRole(UserRole userRole)
        {
            var users = await _context.Users
                                      .Include(p => p.Flat)
                                      .ThenInclude(p => p.Building)
                                      .ThenInclude(p => p.Address)
                                      .Include(p => p.Flat)
                                      .ThenInclude(p => p.Building)
                                      .ThenInclude(p => p.HousingDevelopment)
                                      .Where(p => p.UserRole == userRole)
                                      .ToListAsync();
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
            if (user.Flat != null)
            {
                user.Flat.ResidentsAmount = userContactData.ResidentsAmount;
                user.Flat.ColdWaterEstimatedUsage = userContactData.ColdWaterEstimatedUsage;
                user.Flat.HotWaterEstimatedUsage = userContactData.HotWaterEstimatedUsage;
                user.Flat.HeatingEstimatedUsage = userContactData.HeatingEstimatedUsage;
            }
            user.AvatarUrl = userContactData.AvatarUrl;

            _context.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        #endregion //Methods
    }
}
