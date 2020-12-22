using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class MessageDTO
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
