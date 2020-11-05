﻿using System;
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
            var user = await _context.Users.Include(p => p.Payments).FirstOrDefaultAsync(p => p.Id == id);
            return user.Payments.Select(p => new PaymentForPerformDTO()
            {
                Id = p.Id,
                Name = p.Name,
                Details = p.Details,
                PaymentDeadline = p.PaymentDeadline,
                Value = p.Value,
                Status = p.Status
            }).ToList();
            }

        public async Task<Payment> UpdateOrderStatus(string orderid, string status)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderid);
            payment.Status = status;
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> UpdatePayUOrderId(int userId, string orderId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.UserId == userId);
            payment.OrderId = orderId;
            await _context.SaveChangesAsync();
            return payment;
       
        }
    }

    #endregion

}

