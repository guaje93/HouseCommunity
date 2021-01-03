using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Request;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        #region Constructors

        public UserRepository(DataContext dataContext) : base(dataContext)
        {
        }

        #endregion //Constructors

        #region Methods

        public async Task<User> GetUserById(int id)
        {
            var user = await GetById(id, x => x.Include(p => p.UserFlats)
                                           .ThenInclude(p => p.Flat)
                                           .ThenInclude(p => p.Building)
                                           .ThenInclude(p => p.Cost)
                                           .Include(p => p.UserFlats)
                                           .ThenInclude(p => p.Flat)
                                           .ThenInclude(p => p.Building)
                                           .ThenInclude(p => p.Address)
                                           .Include(p => p.UserConversations)
                                           .ThenInclude(p => p.Conversation)
                                           .ThenInclude(p => p.Users)
                                           .Include(p => p.UserConversations)
                                           .ThenInclude(p => p.Conversation)
                                           .ThenInclude(p => p.Messages)
                                           .ThenInclude(p => p.Sender));

            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await GetAll().FirstOrDefaultAsync(p => p.Email == email);

            return user;
        }

        public async Task<bool> UserExists(string username) => await _context.Users.AnyAsync(p => p.UserName == username);
        public async Task<bool> UserExists(int id) => await _context.Users.AnyAsync(p => p.Id == id);

        public ICollection<User> GetUsers()
        {
            var users = GetAll( x => x.Include(p => p.UserFlats)
                                .ThenInclude(p => p.Flat)
                                .ThenInclude(p => p.Building)
                                .ThenInclude(p => p.Address)
                                .Include(p => p.UserFlats)
                                .ThenInclude(p => p.Flat)
                                .ThenInclude(p => p.Building)
                                .ThenInclude(p => p.HousingDevelopment)).ToList();
            return users;
        }

        public async Task<ICollection<User>> GetUsersByRole(UserRole userRole)
        {
            var users = await GetAll(x => x.Include(p => p.UserFlats)
                                      .ThenInclude(p => p.Flat)
                                      .ThenInclude(p => p.Building)
                                      .ThenInclude(p => p.Address)
                                      .Include(p => p.UserFlats)
                                      .ThenInclude(p => p.Flat)
                                      .ThenInclude(p => p.Building)
                                      .ThenInclude(p => p.HousingDevelopment))
                                      .Where(p => p.UserRole == userRole)
                                      .ToListAsync();
            return users;
        }

        public async Task<User> UpdateUser(User user)
        {
            await UpdateAsync(user);
            return user;
        }

        #endregion //Methods
    }
}
