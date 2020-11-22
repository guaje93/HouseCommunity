using HouseCommunity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class MediaFroDisplayHistoryDTO
    {
        public List<SingleMediaItem> SingleMediaItems { get; set; }
    }

    public class SingleMediaItem
    {
        public string FileName { get; set; }
        public string Description{ get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime StartPeriodDate { get; set; }
        public DateTime EndPeriodDate { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public double CurrentValue{ get; set; }
        public string ImageUrl{ get; set; }
        public MediaEnum MediaEnum { get; set; }
        public MediaStatus Status { get; set; }

    }
}
