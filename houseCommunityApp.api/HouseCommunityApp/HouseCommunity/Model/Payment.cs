using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Model
{
    public class Payment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public ICollection<PaymentDetail> Details { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public DateTime PaymentBookDate { get; set; }
        public User User { get; set; }
        public int UserId{ get; set; }


    }

    public class PaymentDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
