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

                var userData = System.IO.File.ReadAllText("Data/Seeds/UserSeedData.json");
                var houseDevelopmentData = System.IO.File.ReadAllText("Data/Seeds/HousingDevelopmentSeedData.json");
                var buildingData = System.IO.File.ReadAllText("Data/Seeds/BuildingSeedData.json");
                var flatsData = System.IO.File.ReadAllText("Data/Seeds/FlatSeedData.json");
                var addressData = System.IO.File.ReadAllText("Data/Seeds/AddressSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                var houseDevelopment = JsonConvert.DeserializeObject<List<HousingDevelopment>>(houseDevelopmentData);
                var building1 = new Building();
                var building2 = new Building();
                var flats = JsonConvert.DeserializeObject<List<Flat>>(flatsData);
                var address = JsonConvert.DeserializeObject<List<Address>>(addressData);  

                for (int i = 0; i < users.Count; i++)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);

                    users[i].PasswordHash = passwordHash;
                    users[i].PasswordSalt = passwordSalt;
                    users[i].UserName = users[i].UserName.ToLower();
                    users[i].UserRole = users[i].UserRole;
                    if(i > 0 && i < 4)
                    {
                    users[i].Flat = flats[i-1];
                    users[i].Flat.Building = building1;
                    users[i].Flat.Building.Address = address[0];
                    users[i].Flat.Building.HousingDevelopment = houseDevelopment.First();
                    }
                    else if(i >=4 && i < 9)
                    {
                        users[i].Flat = flats[i-1];
                        users[i].Flat.Building = building2;
                        users[i].Flat.Building.Address = address[1];
                        users[i].Flat.Building.HousingDevelopment = houseDevelopment.First();
                    }

                    dataContext.Users.Add(users[i]);

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
                    if (dataContext.Users.FirstOrDefault(p => p.UserName.ToLower() == "reba").Flat.Payments == null)
                        dataContext.Users.FirstOrDefault(p => p.UserName.ToLower() == "reba").Flat.Payments = new List<Payment>();
                    else
                        dataContext.Users.FirstOrDefault(p => p.UserName.ToLower() == "reba").Flat.Payments.Add(payment);
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
