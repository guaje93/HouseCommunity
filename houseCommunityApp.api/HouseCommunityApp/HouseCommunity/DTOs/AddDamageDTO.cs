using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class AddDamageDTO
    {
        public int UserId { get; set; }
        public int BuildingId { get; set; }
        public string Title{ get; set; }
        public string Description { get; set; }

    }
}
