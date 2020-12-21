using HouseCommunity.DTOs;
using HouseCommunity.Model;
using System;
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
        Task<Cost> GetUnitCostsForFlat(int flatId);
        Task<int> GetResidentsAmount(int flatId);
        Task <double> GetWholeBuildingArea(int flatId);
        Task <double> GetFlatArea(int flatId);
        Task <double> GetEstimatedMediaUsage(int flatId, MediaEnum media);
        Task<ICollection<Media>> GetMediaFromLastPeriod(int flatId, DateTime date);
        Task<Payment> CreateNewPayment(int flatId, PaymentDetailsToCreateEmptyDTO paymentDetailsToCreateEmptyDTO);
    }
}