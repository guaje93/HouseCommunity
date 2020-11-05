using HouseCommunity.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public class DotPayRepository : IDotPayRepository
    {
        private readonly DotPayMetadata _dotPayMetadata;

        public DotPayRepository(DotPayMetadata dotPayMetadata)
        {
            _dotPayMetadata = dotPayMetadata;
        }

        public string GetSellerId() => _dotPayMetadata.SellerID;

        public string GenerateSHA256Sum(string amount, string description)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] hashValue = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(_dotPayMetadata.PID + _dotPayMetadata.SellerID + amount + _dotPayMetadata.Currancy + description));
                string hashString = string.Empty;
                foreach (byte x in hashValue)
                {
                    hashString += String.Format("{0:x2}", x);
                }
                return hashString;
            }
        }
    }
    public interface IDotPayRepository
    {
        string GenerateSHA256Sum(string amount, string description);
        string GetSellerId();
    }
}
