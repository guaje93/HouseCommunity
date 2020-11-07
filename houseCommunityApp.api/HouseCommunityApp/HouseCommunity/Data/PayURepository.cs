using HouseCommunity.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public class PayURepository : IPayURepository
    {
        private readonly PayUMetadata _payUMetadata;

        public PayURepository(PayUMetadata payUMetadata)
        {
            _payUMetadata = payUMetadata;
        }

        public string GetClientId() => _payUMetadata.ClientID;

        public string GetClientSecret() => _payUMetadata.ClientSecret;

    }
    public interface IPayURepository
    {
        string GetClientId();
        string GetClientSecret();
    }
}
