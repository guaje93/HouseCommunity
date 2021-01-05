using HouseCommunity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Services
{
    public class HotWaterCostsCalculator : ICostCalculator
    {
        private Cost unitCosts;
        private double hotWaterEstimatedUsage;

        public void Initialize(Flat flat)
        {
            unitCosts = flat.Building.Cost;
            if (flat.HotWaterEstimatedUsage > 0)
                hotWaterEstimatedUsage = flat.HotWaterEstimatedUsage;
            hotWaterEstimatedUsage = flat.Building.HotWaterEstimatedUsageForOneHuman;
        }
        public double CalculateCost()
        {
            return Math.Round(hotWaterEstimatedUsage * (unitCosts.HotWaterUnitCost + unitCosts.ColdWaterUnitCost), 2);
        }

        public string GetDescription()
        {
            return $"Prognozowane zużycie ciepłej wody/msc: {hotWaterEstimatedUsage}m3 * (koszt jednostkowy: {unitCosts.ColdWaterUnitCost}zł/m3 + koszt ogrzania wody: {unitCosts.HotWaterUnitCost}zł/m3)";
        }
    }

    public class ColdWaterCostsCalculator : ICostCalculator
    {
        private Cost unitCosts;
        private double coldWaterEstimatedUsage;

        public void Initialize(Flat flat)
        {
            unitCosts = flat.Building.Cost;
            if (flat.ColdWaterEstimatedUsage > 0)
                coldWaterEstimatedUsage = flat.ColdWaterEstimatedUsage;
            coldWaterEstimatedUsage = flat.Building.ColdWaterEstimatedUsageForOneHuman;
        }
        public double CalculateCost()
        {
            return Math.Round(coldWaterEstimatedUsage * unitCosts.ColdWaterUnitCost, 2);
        }

        public string GetDescription()
        {
            return $"Prognozowane zużycie zimnej wody/msc: {coldWaterEstimatedUsage}m3 * koszt jednostkowy: {unitCosts.ColdWaterUnitCost}zł/m3";
        }
    }

    public class GarbageCostsCalculator : ICostCalculator
    {
        private Cost unitCosts;
        private int residentsAmount;

        public void Initialize(Flat flat)
        {
            unitCosts = flat.Building.Cost;
            residentsAmount = flat.ResidentsAmount;
        }
        public double CalculateCost()
        {
            return Math.Round(residentsAmount * unitCosts.GarbageUnitCost, 2);
        }

        public string GetDescription()
        {
            return $"Liczba mieszkańców: {residentsAmount} * koszt jednostkowy: {unitCosts.GarbageUnitCost}zł";
        }
    }

    public class AdministrationCostsCalculator : ICostCalculator
    {
        private Cost unitCosts;
        private double flatArea;

        public void Initialize(Flat flat)
        {
            unitCosts = flat.Building.Cost;
            flatArea = flat.Area;
        }
        public double CalculateCost()
        {
            return Math.Round(flatArea * unitCosts.AdministrationUnitCost, 2);
        }

        public string GetDescription()
        {
            return $"Powierzchnia mieszkania: {flatArea}m2 * koszt jednostkowy: {unitCosts.AdministrationUnitCost}zł";
        }
    }

    public class HeatingCostsCalculator : ICostCalculator
    {
        private Cost unitCosts;
        private double heatingEstimatedUsage;
        private double flatArea;

        public void Initialize(Flat flat)
        {
            unitCosts = flat.Building.Cost;
            flatArea = flat.Area;
            if (flat.HeatingEstimatedUsage > 0)
                heatingEstimatedUsage = flat.HeatingEstimatedUsage;
            heatingEstimatedUsage = flat.Building.HeatingEstimatedUsageForOneHuman;
        }
        public double CalculateCost()
        {
            return Math.Round(heatingEstimatedUsage * flatArea * unitCosts.HeatingUnitCost, 2);
        }

        public string GetDescription()
        {
            return $"Prognozowane zużycie energii: {heatingEstimatedUsage}GJ/m2 * Powierzchnia mieszkania: {flatArea}m2 * koszt jednostkowy: {unitCosts.HeatingUnitCost}zł/GJ";
        }
    }
}
