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

        public async Task<Payment> CreateNewPayment(int flatId, PaymentDetailsToCreateEmptyDTO p)
        {
            var flat = await _context.Flats.Include(p => p.Payments).FirstOrDefaultAsync(p => p.Id == flatId);

            var payment = new Payment()
            {
                Month = p.Period.Month,
                Year = p.Period.Year,
                PaymentDeadline = p.Period.AddMonths(1),
                Details = new PaymentDetail()
                {
                    AdministrationDescription = p.AdministrationDescription,
                    AdministrationValue = p.AdministrationValue,
                    ColdWaterDescription = p.ColdWaterDescription,
                    ColdWaterValue = p.ColdWaterValue,
                    GarbageDescription = p.GarbageDescription,
                    GarbageValue = p.GarbageValue,
                    HeatingDescription = p.HeatingDescription,
                    HeatingRefundDescription = p.HeatingRefundDescription,
                    HeatingRefundValue = p.HeatingRefundValue,
                    HeatingValue = p.HeatingValue,
                    HotWaterDescription = p.HotWaterDescription,
                    HotWaterValue = p.HotWaterValue,
                    OperatingCostDescription = p.OperatingCostDescription,
                    OperatingCostValue = p.OperatingCostValue,
                    WaterRefundDescription = p.WaterRefundDescription,
                    WaterRefundValue = p.WaterRefundValue
                },
                Name = "Czynsz",
                PaymentStatus = PaymentStatus.WaitingForUser,
                Value = Math.Round(p.AdministrationValue +
                        p.GarbageValue +
                        p.OperatingCostValue +
                        p.ColdWaterValue +
                        p.HotWaterValue +
                        p.HeatingValue +
                        p.HeatingRefundValue +
                        p.WaterRefundValue, 2)
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
                        if (flat.ColdWaterEstimatedUsage > 0)
                            return flat.ColdWaterEstimatedUsage;
                        else
                            return flat.Building.ColdWaterEstimatedUsageForOneHuman;
                    }
                case MediaEnum.HotWater:
                    {
                        if (flat.HotWaterEstimatedUsage > 0)
                            return flat.HotWaterEstimatedUsage;
                        else
                            return flat.Building.HotWaterEstimatedUsageForOneHuman;
                    }
                case MediaEnum.Heat:
                    {
                        if (flat.HeatingEstimatedUsage > 0)
                            return flat.HeatingEstimatedUsage;
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
                PaymentStatus = p.PaymentStatus,
                Month = p.Month,
                Year = p.Year
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
