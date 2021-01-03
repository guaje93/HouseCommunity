namespace HouseCommunity.Services
{
    public interface IRentCostsBuilder
    {
        Model.PaymentDetail Build();
        RentCostsBuilder CalculatePaymentDetails(Model.Flat flat);
        RentCostsBuilder CalculatePaymentRefunds(System.Collections.Generic.IEnumerable<Model.Media> mediaUsageInLastPeriod, Model.Flat flat);
    }
}