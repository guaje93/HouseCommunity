﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Model
{
    public class Flat : IEntity
    {
        public int Id { get; set; }
        public string FlatNumber { get; set; }
        public double Area { get; set; }
        public Building Building { get; set; }
        public int BuildingId { get; set; }
        public ICollection<UserFlat> Residents { get; set; }
        public int ResidentsAmount { get; set; }
        public ICollection<Media> MediaHistory { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public double HeatingEstimatedUsage { get; set; }
        public double ColdWaterEstimatedUsage { get; set; }
        public double HotWaterEstimatedUsage { get; set; }
    }
}
