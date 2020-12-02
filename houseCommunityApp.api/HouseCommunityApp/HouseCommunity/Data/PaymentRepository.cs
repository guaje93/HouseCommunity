using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseCommunity.DTOs;
using HouseCommunity.Model;
using Microsoft.EntityFrameworkCore;

namespace HouseCommunity.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        #region Fields

        private readonly DataContext _context;

        #endregion //Fields

        #region Constructors

        public PaymentRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<Payment> GetPaymentById(int id)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);
            return payment;
        }

        public async Task<List<PaymentForPerformDTO>> GetPayments(int id)
        {
            var user = await _context.Users.Include(p => p.Flat)
                                           .ThenInclude(p => p.Payments)
                                           .ThenInclude(p => p.Details)
                                           .FirstOrDefaultAsync(p => p.Id == id);
            return user.Flat.Payments.Select(p => new PaymentForPerformDTO()
            {
                Id = p.Id,
                Name = p.Name,
                Details = p.Details,
                PaymentDeadline = p.PaymentDeadline,
                Value = p.Value,
                PaymentStatus = p.BookStatus
            }).ToList();
            }

        public async Task<Cost> GetUnitCostsForFlat(int flatId)
        {
            var flat = await _context.Flats.Include(p => p.Building).ThenInclude(p => p.Cost).FirstOrDefaultAsync(p => p.Id == flatId);
            return flat.Building.Cost;
        }

        public async Task<Payment> UpdateOrderStatus(string orderid, string status)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderid);
            payment.BookStatus = status;
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> UpdatePayUOrderId(int id, string orderId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);
            payment.OrderId = orderId;
            await _context.SaveChangesAsync();
            return payment;
       
        }
    }

    #endregion

}

