using System;
using System.Collections.Generic;

namespace HouseCommunity.Model
{
    public class Payment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public PaymentDetail Details { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public DateTime PaymentBookDate { get; set; }
        public Flat Flat { get; set; }
        public int UserId { get; set; }
        public string PaymentStatus { get; set; }
        public string BookStatus { get; set; }
        public string OrderId { get; set; }
    }

    public class PaymentDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Period { get; set; }
        public string Description { get; set; }
    }
}
