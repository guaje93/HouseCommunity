using HouseCommunity.Helpers;
using HouseCommunity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public class PayURepository : BaseRepository<Payment>, IPayURepository
    {
        #region Fields

        private readonly PayUMetadata _payUMetadata;

        #endregion //Fields

        #region Constructors

        public PayURepository(PayUMetadata payUMetadata, DataContext dataContext) : base(dataContext)
        {
            _payUMetadata = payUMetadata;
        }

        #endregion //Constructors

        #region Methods

        public string GetClientId() => _payUMetadata.ClientID;
        public string GetClientSecret() => _payUMetadata.ClientSecret;

        public async Task<Payment> UpdatePaymentOrderStatus(string orderid, string status)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderid);
            switch (status)
            {
                case "PENDING":
                case "WAITING_FOR_CONFIRMATION":
                    payment.PaymentStatus = PaymentStatus.PaymentStarted;
                    break;
                case "CANCELED":
                    payment.PaymentStatus = PaymentStatus.PaymentCancelled;
                    break;

                case "COMPLETED":
                    payment.PaymentStatus = PaymentStatus.PaymentCompleted;
                    break;

            }

            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> UpdatePaymentOrderId(int id, string orderId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);
            payment.OrderId = orderId;
            await _context.SaveChangesAsync();
            return payment;
        }

        #endregion //Methods

    }
}
