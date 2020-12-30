namespace HouseCommunity.Model
{
    public class PaymentDetail
    {
        public int Id { get; set; }
        public double HotWaterValue { get; set; }
        public double ColdWaterValue { get; set; }
        public double HeatingValue { get; set; }
        public double AdministrationValue { get; set; }
        public double HeatingRefundValue { get; set; }
        public double GarbageValue { get; set; }
        public double WaterRefundValue { get; set; }
        public string HeatingDescription { get; set; }
        public string ColdWaterDescription { get; set; }
        public string GarbageDescription { get; set; }
        public string HotWaterDescription { get; set; }
        public string AdministrationDescription { get; set; }
        public string HeatingRefundDescription { get; set; }
        public string WaterRefundDescription { get; set; }
    }
}
