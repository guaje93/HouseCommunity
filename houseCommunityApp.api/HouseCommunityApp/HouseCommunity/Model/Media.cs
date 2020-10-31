﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Model
{
    public class Media
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string FileName { get; set; }
        public string UserDescription { get; set; }
        public double CurrentValue { get; set; }
        public MediaEnum MediaType { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime AcceptanceDate { get; set; }

        public User User { get; set; }
    }
}