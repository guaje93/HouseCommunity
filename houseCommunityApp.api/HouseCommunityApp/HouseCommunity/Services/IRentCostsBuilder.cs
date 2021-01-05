using HouseCommunity.Model;
using System;
using System.Collections.Generic;

namespace HouseCommunity.Services
{
    public interface IRentCostsBuilder
    {
        Model.PaymentDetail Build();
        RentCostsBuilder CalculatePaymentDetails(Flat flat);
        RentCostsBuilder CalculatePaymentRefunds(IEnumerable<Media> mediaUsageInLastPeriod, Flat flat, DateTime date);
    }
}