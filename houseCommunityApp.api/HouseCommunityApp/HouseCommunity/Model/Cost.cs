namespace HouseCommunity.Model
{
    public class Cost
    {
        public int Id { get; set; }
        public decimal ColdWaterUnitCost { get; set; }
        public decimal HotWaterUnitCost { get; set; }
        public decimal HeatingUnitCost { get; set; }
        public decimal GarbageUnitCost { get; set; }
        public decimal ExUnitCost { get; set; }
        public decimal OperatingUnitCost { get; set; }
        public decimal AdministrationUnitCost { get; set; }
    }
}
