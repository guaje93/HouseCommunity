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

        public async Task<Payment> CreateNewPayment(int flatId, DateTime date, PaymentDetail paymentDetails)
        {
            var flat = await _context.Flats.Include(p => p.Payments).FirstOrDefaultAsync(p => p.Id == flatId);
            var payment = new Payment()
            {
                Month = date.Month,
                Year = date.Year,
                PaymentDeadline = date.AddMonths(1),
                Details = paymentDetails,
                Name = "Czynsz",
                Value = Math.Round(paymentDetails.AdministrationValue +
                        paymentDetails.GarbageValue +
                        paymentDetails.OperatingCostValue +
                        paymentDetails.ColdWaterValue +
                        paymentDetails.HotWaterValue +
                        paymentDetails.HeatingValue +
                        paymentDetails.HeatingRefundValue +
                        paymentDetails.WaterRefundValue, 2)
            };
            flat.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<double> GetEstimatedMediaUsage(int flatId, MediaEnum media)
        {
            var flat = await _context.Flats.Include(p => p.Building).FirstOrDefaultAsync(p => p.Id == flatId);
            switch (media)
            {
                default:
                    return 0;

                case MediaEnum.ColdWater:
                    {
                        if (flat.ColdWaterEstimatedUsageForOneHuman > 0)
                            return flat.ColdWaterEstimatedUsageForOneHuman;
                        else
                            return flat.Building.ColdWaterEstimatedUsageForOneHuman;
                    }
                case MediaEnum.HotWater:
                    {
                        if (flat.HotWaterEstimatedUsageForOneHuman > 0)
                            return flat.HotWaterEstimatedUsageForOneHuman;
                        else
                            return flat.Building.HotWaterEstimatedUsageForOneHuman;
                    }
                case MediaEnum.Heat:
                    {
                        if (flat.HeatingEstimatedUsageForOneHuman > 0)
                            return flat.HeatingEstimatedUsageForOneHuman;
                        else
                            return flat.Building.HeatingEstimatedUsageForOneHuman;
                    }
            }
        }

        public async Task<double> GetFlatArea(int flatId)
        {
            var flat = await _context.Flats.FirstOrDefaultAsync(p => p.Id == flatId);
            return flat.Area;
        }

        public async Task<ICollection<Media>> GetMediaFromLastPeriod(int flatId, DateTime date)
        {
            var flat = await _context.Flats.Include(p => p.MediaHistory).FirstOrDefaultAsync(p => p.Id == flatId);
            return flat.MediaHistory.Where(p => p.CreationDate <= date && p.EndPeriodDate >= date).ToList();
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
                PaymentStatus = p.PaymentStatus
            }).ToList();
        }

        public async Task<int> GetResidentsAmount(int flatId)
        {
            var flat = await _context.Flats.FirstOrDefaultAsync(p => p.Id == flatId);
            return flat.ResidentsAmount;
        }

        public async Task<Cost> GetUnitCostsForFlat(int flatId)
        {
            var flat = await _context.Flats.Include(p => p.Building).ThenInclude(p => p.Cost).FirstOrDefaultAsync(p => p.Id == flatId);
            return flat.Building.Cost;
        }

        public async Task<double> GetWholeBuildingArea(int flatId)
        {
            var flat = await _context.Flats.Include(p => p.Building)
                                           .ThenInclude(p => p.Flats)
                                           .FirstOrDefaultAsync(p => p.Id == flatId);

            return flat.Building.Flats.Sum(p => p.Area);
        }

        public async Task<Payment> UpdateOrderStatus(string orderid, string status)
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
