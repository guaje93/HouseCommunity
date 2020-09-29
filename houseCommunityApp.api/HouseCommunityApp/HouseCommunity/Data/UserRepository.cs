using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Request;
using Microsoft.EntityFrameworkCore;
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
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

            if (user == null)
                return null;

            return new UserForInfoDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthdate = user.Birthdate,
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<User> UpdateUserContactData(UserContactData userContactData)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userContactData.Id);
            if (user == null)
                return null;

            user.PhoneNumber = userContactData.PhoneNumber;
            user.Email = userContactData.Email;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        #endregion //Methods
    }
}
