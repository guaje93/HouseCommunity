using HouseCommunity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Services
{
    public class AuthService : IAuthService
    {
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
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

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            };
        }

        public string GenerateUserName(string firstName, string lastName, IEnumerable<string> userNamesFromDb)
        {
            var userName = $"HC{firstName.Substring(0, 2).ToUpper()}{lastName.Substring(0, 3).ToUpper()}";
            if (!userNamesFromDb.Contains(userName))
                return userName;
            else
            {
                var tempUserName = userName;
                var index = 1;
                do
                {
                    if (index < 10)
                        tempUserName = userName + "0" + index;
                    else
                        tempUserName = userName + index;

                    index++;
                }
                while (userNamesFromDb.Contains(tempUserName));
                return tempUserName;
            }
        }

        public string GenerateRandomPassword(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    public interface IAuthService
    {
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        string GenerateUserName(string firstName, string lastName, IEnumerable<string> userNamesFromDb);
        string GenerateRandomPassword(int length);
    }
}
