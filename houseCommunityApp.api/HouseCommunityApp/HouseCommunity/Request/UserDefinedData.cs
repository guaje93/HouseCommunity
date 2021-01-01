using HouseCommunity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Request
{
    public class UserDefinedData
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }

        public ICollection<FlatForInfoDTO> UserFlats { get; set; }

    }
}
