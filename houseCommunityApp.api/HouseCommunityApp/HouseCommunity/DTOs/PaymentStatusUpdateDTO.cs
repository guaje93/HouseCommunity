using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class PaymentStatusUpdateDTO
    {
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
    }
}
