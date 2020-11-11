using System.Collections.Generic;

namespace HouseCommunity.Model
{
    public class HousingDevelopment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Building> Buildings { get; set; }
    }
}
