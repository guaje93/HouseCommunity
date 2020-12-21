using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class GetDamageForHouseManagerDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string User { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<string> FilesPaths { get; set; }
    }
}
