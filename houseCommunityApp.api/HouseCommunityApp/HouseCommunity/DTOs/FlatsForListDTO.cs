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
}
