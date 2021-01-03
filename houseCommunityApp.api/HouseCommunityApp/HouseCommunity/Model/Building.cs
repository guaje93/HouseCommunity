using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Model
{
    public class Building : IEntity
    {
        public int Id { get; set; }
        public ICollection<Flat> Flats { get; set; }
        public ICollection<Damage> Damages { get; set; }
        public HousingDevelopment HousingDevelopment { get; set; }
        public int HousingDevelopmentId { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
        public Cost Cost{ get; set; }
        public User HouseManager { get; set; }
        public double HeatingEstimatedUsageForOneHuman { get; set; }
        public double ColdWaterEstimatedUsageForOneHuman { get; set; }
        public double HotWaterEstimatedUsageForOneHuman { get; set; }
    }
}
