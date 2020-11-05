using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class PaymentForBookDTO
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public DateTime PaymentBookDate { get; set; }
    }
}
