using HouseCommunity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class PaymentForPerformDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public PaymentDetail Details { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public DateTime ? PaymentBookDate { get; set; }
        public string ? PaymentStatus { get; set; }
        public string ? BookStatus { get; set; }
    }
}
