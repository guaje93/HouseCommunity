namespace HouseCommunity.Data
{
    public interface IPayURepository
    {
        string GetClientId();
        string GetClientSecret();
        System.Threading.Tasks.Task<Model.Payment> UpdatePaymentOrderStatus(string orderid, string status);
        System.Threading.Tasks.Task<Model.Payment> UpdatePaymentOrderId(int id, string orderId);
    }
}
