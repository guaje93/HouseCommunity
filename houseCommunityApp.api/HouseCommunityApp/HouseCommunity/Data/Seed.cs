using HouseCommunity.Data;
using HouseCommunity.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace HouseCommunity.Helpers
{
    public class Seed
    {
        public static void SeedUsers(DataContext dataContext)
        {

            if (!dataContext.Users.Any())
            {

                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.UserName = user.UserName.ToLower();
                    dataContext.Users.Add(user);

                }
                dataContext.SaveChanges();
            }
        }

        public static void SeedPayments(DataContext dataContext)
        {

            if (!dataContext.Payments.Any())
            {

                var paymentData = System.IO.File.ReadAllText("Data/UserPaymentsSeed.json");
                var payments = JsonConvert.DeserializeObject<List<Payment>>(paymentData);
                foreach (var payment in payments)
                {
                    if (dataContext.Users.FirstOrDefault(p => p.UserName.ToLower() == "reba").Payments == null)
                        dataContext.Users.FirstOrDefault(p => p.UserName.ToLower() == "reba").Payments = new List<Payment>();
                    else
                        dataContext.Users.FirstOrDefault(p => p.UserName.ToLower() == "reba").Payments.Add(payment);
                }
                dataContext.SaveChanges();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            };
        }
    }
}
