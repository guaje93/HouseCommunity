using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class CostsForPaymentDTO
    {
        public decimal ColdWaterCost { get; set; }
        public decimal HotWaterCost { get; set; }
        public decimal HeatingCost { get; set; }
        public decimal GarbageCost { get; set; }
        public decimal OperatingCost { get; set; }
        public decimal AdministrationCost { get; set; }
    }
}
