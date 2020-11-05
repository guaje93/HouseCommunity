using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class PayUResponseDTO
    {
        public Order Order{ get; set; }
    }

    public class Order
    {
        public string OrderId { get; set; }
        public DateTime OrderCreateDate{ get; set; }
        public string NotifyUrl { get; set; }
        public string Description { get; set; }
        public string TotalAmount { get; set; }
        public string Status { get; set; }
    }

}
