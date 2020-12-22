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
        public int ResidentsAmount{ get; set; }
        public string AvatarUrl { get; set; }
        public double ColdWaterEstimatedUsage { get; set; }
        public double HotWaterEstimatedUsage { get; set; }
        public double HeatingEstimatedUsage { get; set; }
    }
}
