using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class ResidentsForListDTO
    {
        public int HousingDevelopmentId { get; set; }
        public int BuildingId { get; set; }
        public int FlatId { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string HousingDevelopmentName { get; set; }
        public string Address { get; set; }
        public string LocalNumber { get; set; }
        public string Name { get; set; }
    }

    public class ResidentsForRegisterDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

}
