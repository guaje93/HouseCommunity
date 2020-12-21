using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class AddImageDTO
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl{ get; set; }
    }
}
