namespace HouseCommunity.Controllers
{
    public class MediaUpdatedByUserDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string FileName { get; set; }
        public string UserDescription { get; set; }
        public double CurrentValue { get; set; }
    }
}