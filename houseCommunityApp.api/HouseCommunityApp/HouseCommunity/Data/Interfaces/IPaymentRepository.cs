using HouseCommunity.DTOs;
using HouseCommunity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public interface IPaymentRepository
    {
        Task<List<PaymentForPerformDTO>> GetPayments(int id);
        Task<Payment> GetPaymentById(int id);
        Task<Payment> UpdatePayUOrderId(int id, string orderId); 
        Task<Payment> UpdateOrderStatus(string orderid, string status);
    }
}