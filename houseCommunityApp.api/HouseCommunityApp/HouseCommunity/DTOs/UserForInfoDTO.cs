using HouseCommunity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class UserForInfoDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public IEnumerable<FlatForInfoDTO> UserFlats { get; set; }
        public UserRole UserRole { get; set; }
        public string AvatarUrl{ get; set; }
       
    }

    public class FlatForInfoDTO
    {
        public int Id{ get; set; }
        public string FlatName { get; set; }
        public double Area { get; set; }
        public double ColdWaterEstimatedUsage { get; set; }
        public double ColdWaterUnitCost { get; set; }
        public double HotWaterEstimatedUsage { get; set; }
        public double HotWaterUnitCost { get; set; }
        public double HeatingUnitCost { get; set; }
        public double HeatingEstimatedUsage { get; set; }
        public int ResidentsAmount { get; set; }
    }
}
