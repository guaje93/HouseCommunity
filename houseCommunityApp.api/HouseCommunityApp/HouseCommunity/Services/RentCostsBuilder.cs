using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseCommunity.Model;

namespace HouseCommunity.Services
{
    public class RentCostsBuilder : IRentCostsBuilder
    {
        private readonly PaymentDetail _payment;
        private readonly ICostCalculator _hotWaterCostsCalculator;
        private readonly ICostCalculator _coldWaterCostsCalculator;
        private readonly ICostCalculator _heatingWaterCostsCalculator;
        private readonly ICostCalculator _administrationCostsCalculator;
        private readonly ICostCalculator _garbageCostsCalculator;

        public RentCostsBuilder()
        {
            _payment = new PaymentDetail();
            _hotWaterCostsCalculator = new HotWaterCostsCalculator();
            _coldWaterCostsCalculator = new ColdWaterCostsCalculator();
            _heatingWaterCostsCalculator = new HeatingCostsCalculator();
            _administrationCostsCalculator = new AdministrationCostsCalculator();
            _garbageCostsCalculator = new GarbageCostsCalculator();
        }

        public PaymentDetail Build()
        {
            return _payment;
        }
        public RentCostsBuilder CalculatePaymentDetails(Flat flat)
        {
            _hotWaterCostsCalculator.Initialize(flat);
            _coldWaterCostsCalculator.Initialize(flat);
            _heatingWaterCostsCalculator.Initialize(flat);
            _administrationCostsCalculator.Initialize(flat);
            _garbageCostsCalculator.Initialize(flat);

            _payment.AdministrationValue = _administrationCostsCalculator.CalculateCost();
            _payment.AdministrationDescription = _administrationCostsCalculator.GetDescription();
            _payment.GarbageValue = _garbageCostsCalculator.CalculateCost();
            _payment.GarbageDescription = _garbageCostsCalculator.GetDescription();
                _payment.ColdWaterValue = _coldWaterCostsCalculator.CalculateCost();
            _payment.ColdWaterDescription = _coldWaterCostsCalculator.GetDescription();
            _payment.HotWaterValue = _hotWaterCostsCalculator.CalculateCost();
                _payment.HotWaterDescription = _hotWaterCostsCalculator.GetDescription();
            _payment.HeatingValue = _heatingWaterCostsCalculator.CalculateCost();
                _payment.HeatingDescription = _heatingWaterCostsCalculator.GetDescription();

            return this;
        }

        public RentCostsBuilder CalculatePaymentRefunds(IEnumerable<Media> mediaUsageInLastPeriod, Flat flat)
        {
            var unitCosts = flat.Building.Cost;
            var coldWaterEstimatedUsage = flat.ColdWaterEstimatedUsage;
            var hotWaterEstimatedUsage = flat.HotWaterEstimatedUsage;
            var heatingEstimatedUsage = flat.HeatingEstimatedUsage;
            var residentsAmount = flat.ResidentsAmount;
            var flatArea = flat.Area;

            var hotWaterRealUsageInLastPeriod = 0.0;
            var hotWaterEstimatedUsageInLastPeriodExceptCurrent = 0.0;
            var coldWaterRealUsageInLastPeriod = 0.0;
            var coldWaterEstimatedUsageInLastPeriodExceptCurrent = 0.0;
            var heatingRealUsageInLastPeriod = 0.0;
            var heatingEstimatedUsageInLastPeriodExceptCurrent = 0.0;

            hotWaterRealUsageInLastPeriod = mediaUsageInLastPeriod.Where(p => p.MediaType == MediaEnum.HotWater).Sum(p => p.CurrentValue);
            coldWaterRealUsageInLastPeriod = mediaUsageInLastPeriod.Where(p => p.MediaType == MediaEnum.ColdWater).Sum(p => p.CurrentValue);
            heatingRealUsageInLastPeriod = mediaUsageInLastPeriod.Where(p => p.MediaType == MediaEnum.Heat).Sum(p => p.CurrentValue);

            _payment.HeatingRefundValue = Math.Round((heatingRealUsageInLastPeriod - heatingEstimatedUsageInLastPeriodExceptCurrent - heatingEstimatedUsage) * unitCosts.HeatingUnitCost, 2);
            _payment.HeatingRefundDescription = $"(Rzeczywiste zużycie energii: {heatingRealUsageInLastPeriod}GJ (ostatnie pół roku) - Opłacone zużycie energii (ostatnie pół roku): {heatingEstimatedUsageInLastPeriodExceptCurrent + heatingEstimatedUsage}GJ) * koszt jednostkowy: {unitCosts.HeatingUnitCost}zł/GJ";
            _payment.WaterRefundValue = Math.Round((hotWaterRealUsageInLastPeriod - hotWaterEstimatedUsageInLastPeriodExceptCurrent - hotWaterEstimatedUsage) * unitCosts.HotWaterUnitCost +
                               (coldWaterRealUsageInLastPeriod - coldWaterEstimatedUsageInLastPeriodExceptCurrent - coldWaterEstimatedUsage) * unitCosts.ColdWaterUnitCost, 2);
            _payment.WaterRefundDescription = $"(Rzeczywiste zużycie wody ciepłej (ostatnie pół roku): {hotWaterRealUsageInLastPeriod}m3 - Opłacone zużycie wody ciepłej (ostatnie pół roku): {hotWaterEstimatedUsageInLastPeriodExceptCurrent + hotWaterEstimatedUsage}m3) * koszt jednostkowy: {unitCosts.HotWaterUnitCost}zł/m3 + " +
            $"(Rzeczywiste zużycie wody zimnej (ostatnie pół roku): {coldWaterRealUsageInLastPeriod}m3 - Opłacone zużycie wody zimnej (ostatnie pół roku): {coldWaterEstimatedUsageInLastPeriodExceptCurrent + coldWaterEstimatedUsage}m3) * koszt jednostkowy: {unitCosts.ColdWaterUnitCost}zł/m3";

            return this;
        }
    }


}
