using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Request;
using HouseCommunity.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public class AuthRepository : BaseRepository<User>, IAuthRepository
    {
        private readonly IAuthService _authService;


        #region Constructors

        public AuthRepository(DataContext dataContext, IAuthService authService) : base(dataContext)
        {
            this._authService = authService;
        }

        #endregion //Constructors

        #region Methods

        public async Task<User> LogIn(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.UserName == userName);
            if (user == null)
                return null;

            if (!_authService.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<User> ResetPassword(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.UserName == username);
            byte[] passwordHash, passwordSalt;
            _authService.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;

        }

        public async Task<User> ChangePassword(PasswordChangeRequest passwordChangeRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == passwordChangeRequest.Id);

            if (user == null || !_authService.VerifyPasswordHash(passwordChangeRequest.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                return null;

            byte[] passwordHash, passwordSalt;
            _authService.CreatePasswordHash(passwordChangeRequest.ConfirmPassword, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> RegisterUser(UserForRegisterDTO userForRegisterDTO)
        {

            byte[] passwordHash, passwordSalt;
            _authService.CreatePasswordHash(_authService.GenerateRandomPassword(8), out passwordHash, out passwordSalt);

            var flat = await _context.Flats.Include(p => p.Residents).FirstOrDefaultAsync(p => p.Id == userForRegisterDTO.FlatId);
            var userNames = await GetAll().ToListAsync();
            var user = new User()
            {
                UserName = _authService.GenerateUserName(userForRegisterDTO.FirstName, userForRegisterDTO.LastName, userNames.Select(p => p.UserName)),
                FirstName = userForRegisterDTO.FirstName,
                LastName = userForRegisterDTO.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = userForRegisterDTO.Email,
                UserRole = UserRole.Resident
            };
            flat.Residents.Add(new UserFlat()
            {
                User = user
            });

            await _context.SaveChangesAsync();
            return user;
        }     

        public async Task<bool> HasAdministrationRights(int userId)
        {
            var user = await GetById(userId);
            return user.UserRole == UserRole.Administrator;
        }

        public async Task<bool> HasHouseManagerRights(int userId)
        {
            var user = await GetById(userId);
            return user.UserRole == UserRole.HouseManager;
        }

        #endregion //Methods
    }
}
