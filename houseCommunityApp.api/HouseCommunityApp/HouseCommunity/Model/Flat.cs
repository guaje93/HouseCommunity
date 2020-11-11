using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Model
{
    public class Flat
    {
        public int Id { get; set; }
        public string FlatNumber { get; set; }
        public Building Building { get; set; }
        public int BuildingId { get; set; }
        public ICollection<User> Residents { get; set; }
        public ICollection<Media> MediaHistory { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
