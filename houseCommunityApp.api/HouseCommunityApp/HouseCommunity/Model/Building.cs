using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Model
{
    public class Building
    {
        public int Id { get; set; }
        public ICollection<Flat> Flats { get; set; }
        public HousingDevelopment HousingDevelopment { get; set; }
        public int HousingDevelopmentId { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
        public Cost Cost{ get; set; }
    }
}
