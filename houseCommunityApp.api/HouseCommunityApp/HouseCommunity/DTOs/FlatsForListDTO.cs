using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class FlatsForListDTO
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public ICollection<UserNamesListDTO> Residents { get; set; }
    }

    public class FlatForFilteringDTO
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string Name { get; set; }
        public int FlatId { get; set; }
        public int BuildingId { get; set; }
        public int HousingDevelopmentId { get; set; }
        public string HousingDevelopmentName { get; set; }
        public string Address { get; set; }
        public int LocalNumber { get; set; }
    }
}
