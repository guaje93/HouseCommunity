﻿using HouseCommunity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class MediaForAndministrationDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string FileName { get; set; }
        public string UserDescription { get; set; }
        public double CurrentValue { get; set; }
        public MediaEnum MediaType { get; set; }
        public string Period { get; set; }
        public DateTime ? CreationDate { get; set; }
        public MediaStatus Status { get; set; }
        public DateTime? AcceptanceDate { get; internal set; }
        public double LastValue { get; internal set; }
    }
}
