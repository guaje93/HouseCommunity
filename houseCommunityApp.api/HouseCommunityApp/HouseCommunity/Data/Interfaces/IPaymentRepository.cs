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
        Task<ICollection<Media>> GetMediaFromLastPeriod(int flatId, DateTime date);
        Task<Payment> CreateNewPayment(PaymentDetailsToCreateEmptyDTO paymentDetailsToCreateEmptyDTO);
        Task<Payment> CreateNewPayment(CustomPaymentDetailsDTO customPaymentDetails);
        Task<Payment> UpdatePaymentStatus(int paymentId, PaymentStatus paymentCompleted);
        Task RemovePayment(int paymentId);
    }
}