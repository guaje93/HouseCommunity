using HouseCommunity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class UserForLogInDueToPermissions
    {
        public int Id { get; set; }
        public UserRole UserRole { get; set; }
        IEnumerable<int> Flats { get; set; }
    }
}
