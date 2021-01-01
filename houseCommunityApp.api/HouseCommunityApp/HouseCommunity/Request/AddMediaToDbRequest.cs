using HouseCommunity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Request
{
    public class AddMediaToDbRequest
    {
        public int UserId { get; set; }
        public int FlatId { get; set; }
        public string ImageUrl { get; set; }
        public string UserDescription { get; set; }
        public string MediaType{ get; set; }
        public double CurrentValue { get; set; }
    }
}
