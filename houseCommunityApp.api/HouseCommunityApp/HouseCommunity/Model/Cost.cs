namespace HouseCommunity.Model
{
    public class Cost
    {
        public int Id { get; set; }
        public double ColdWaterUnitCost { get; set; }
        public double HotWaterUnitCost { get; set; }
        public double HeatingUnitCost { get; set; }
        public double GarbageUnitCost { get; set; }
        public double OperatingUnitCost { get; set; }
        public double AdministrationUnitCost { get; set; }
    }
}
