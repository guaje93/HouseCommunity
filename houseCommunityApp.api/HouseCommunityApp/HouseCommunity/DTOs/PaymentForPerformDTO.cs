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
        public string FlatAddress { get; set; }
        public double Value { get; set; }
        public PaymentDetail Details { get; set; }
        public string Period { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public DateTime ? PaymentBookDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public PaymentType Type { get; internal set; }
        public string Description { get; internal set; }
    }
}
