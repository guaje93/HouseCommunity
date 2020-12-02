using System;

namespace HouseCommunity.Model
{
    public class Media
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string FileName { get; set; }
        public string UserDescription { get; set; }
        public double LastValue { get; set; }
        public double CurrentValue { get; set; }
        public MediaEnum MediaType { get; set; }
        public DateTime StartPeriodDate { get; set; }
        public DateTime EndPeriodDate { get; set; }
        public DateTime ? CreationDate { get; set; }
        public DateTime ? AcceptanceDate { get; set; }
        public MediaStatus Status { get; set; }
        public Flat Flat{ get; set; }
    }

    public enum MediaStatus
    {
        WaitingForUser,
        UpdatedByUser,
        AcceptedByAdministration
    }
}
