﻿using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Request;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public class AuthRepository : IAuthRepository
    {
        #region Fields

        private readonly DataContext _context;

        #endregion //Fields

        #region Constructors

        public AuthRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        #endregion //Constructors

        #region Methods

        public async Task<User> LogIn(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.UserName == userName);
            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }
        public async Task<User> GetUserForReset(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Email == email);
            return user;
        }

        public async Task<User> ResetPassword(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.UserName == username);
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;

        }

        public async Task<bool> UserExists(string username) => await _context.Users.AnyAsync(p => p.UserName == username);

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            };

            return true;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            };
        }

        public async Task<string> GetUserNameById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);
            return user.UserName;
        }

        public async Task<User> ChangePassword(PasswordChangeRequest passwordChangeRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == passwordChangeRequest.Id);

            if (!VerifyPasswordHash(passwordChangeRequest.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                return null;

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(passwordChangeRequest.ConfirmPassword, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> RegisterUser(UserForRegisterDTO userForRegisterDTO)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userForRegisterDTO.Password, out passwordHash, out passwordSalt);
            
            var flat = await _context.Flats.Include(p => p.Residents).FirstOrDefaultAsync(p => p.Id == userForRegisterDTO.FlatId);
            var user = new User()
            {
                UserName = userForRegisterDTO.UserName,
                FirstName = userForRegisterDTO.FirstName,
                LastName = userForRegisterDTO.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = userForRegisterDTO.Email
            };
            flat.Residents.Add(user);

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> HasAccessToAdministration(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userId);
            return user.UserRole == UserRole.Administrator;
        }



        #endregion //Methods
    }
}
