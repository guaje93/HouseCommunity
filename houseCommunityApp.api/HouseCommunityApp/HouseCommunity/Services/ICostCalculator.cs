using HouseCommunity.Model;

namespace HouseCommunity.Services
{
    public interface ICostCalculator
    {
        double CalculateCost();
        string GetDescription();
        void Initialize(Flat flat);
    }
}
