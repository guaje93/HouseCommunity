﻿using System;
using System.Collections.Generic;

namespace HouseCommunity.Model
{
    public class Payment : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public PaymentDetail Details { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public DateTime PaymentBookDate { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public Flat Flat { get; set; }
        public int UserId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentType PaymentType { get; set; }
        public string OrderId { get; set; }
        public string Description { get; internal set; }
    }

    public enum PaymentType
    {
        CUSTOM, RENT
    }
}
