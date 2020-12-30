using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class CustomPaymentDetailsDTO

    {
        public int FlatId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime Period { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public DateTime Deadline { get; set; }
    }
}