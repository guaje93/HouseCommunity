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

        public async Task<Payment> CreateNewPayment(PaymentDetailsToCreateEmptyDTO paymentDTO)
        {
            var flat = await _context.Flats.Include(p => p.Payments).Include(p => p.Residents).FirstOrDefaultAsync(p => p.Id == paymentDTO.FlatId);

            var payment = new Payment()
            {
                Month = paymentDTO.Period.Month,
                Year = paymentDTO.Period.Year,
                PaymentDeadline = paymentDTO.Deadline,
                PaymentType = PaymentType.RENT,
                
                Details = new PaymentDetail()
                {
                    AdministrationDescription = paymentDTO.AdministrationDescription,
                    AdministrationValue = paymentDTO.AdministrationValue,
                    ColdWaterDescription = paymentDTO.ColdWaterDescription,
                    ColdWaterValue = paymentDTO.ColdWaterValue,
                    GarbageDescription = paymentDTO.GarbageDescription,
                    GarbageValue = paymentDTO.GarbageValue,
                    HeatingDescription = paymentDTO.HeatingDescription,
                    HeatingRefundDescription = paymentDTO.HeatingRefundDescription,
                    HeatingRefundValue = paymentDTO.HeatingRefundValue,
                    HeatingValue = paymentDTO.HeatingValue,
                    HotWaterDescription = paymentDTO.HotWaterDescription,
                    HotWaterValue = paymentDTO.HotWaterValue,
                    WaterRefundDescription = paymentDTO.WaterRefundDescription,
                    WaterRefundValue = paymentDTO.WaterRefundValue
                },
                Name = "Czynsz",
                PaymentStatus = PaymentStatus.WaitingForUser,
                Value = Math.Round(paymentDTO.AdministrationValue +
                        paymentDTO.GarbageValue +
                        paymentDTO.OperatingCostValue +
                        paymentDTO.ColdWaterValue +
                        paymentDTO.HotWaterValue +
                        paymentDTO.HeatingValue +
                        paymentDTO.HeatingRefundValue +
                        paymentDTO.WaterRefundValue, 2)
            };
            flat.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> CreateNewPayment(CustomPaymentDetailsDTO customPaymentDetails)
        {
            var flat = await _context.Flats.Include(p => p.Payments).Include(p => p.Residents).FirstOrDefaultAsync(p => p.Id == customPaymentDetails.FlatId);
            var payment = new Payment()
            {
                Month = customPaymentDetails.Period.Month,
                Year = customPaymentDetails.Period.Year,
                Name = customPaymentDetails.Name,
                PaymentStatus = PaymentStatus.WaitingForUser,
                PaymentType = PaymentType.CUSTOM,
                PaymentDeadline = customPaymentDetails.Deadline,
                Value = customPaymentDetails.Value,
                Description = customPaymentDetails.Description
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
            var flat = await _context.Flats.Include(p => p.Residents)
                                           .ThenInclude(p => p.Flat)
                                           .ThenInclude(p => p.Payments)
                                           .ThenInclude(p => p.Details)
                                           .Include(p => p.Residents)
                                           .ThenInclude(p => p.Flat)
                                           .ThenInclude(p => p.Building)
                                           .ThenInclude(p => p.Address)
                                           .FirstOrDefaultAsync(p => p.Id == id);
            return flat.Residents?.SelectMany(p => p.Flat.Payments).Select(p => new PaymentForPerformDTO()
            {
                Id = p.Id,
                Name = p.Name,
                FlatAddress = p.Flat.Building.Address.ToString() + " m." + p.Flat.FlatNumber,
                Description = p.Description,
                Details = p.Details,
                PaymentDeadline = p.PaymentDeadline,
                PaymentBookDate = p.PaymentBookDate,
                Value = p.Value,
                Period = $"M{p.Month}Y{p.Year}",
                Type = p.PaymentType,
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

        public async Task RemovePayment(int paymentId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
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

        public async Task<Payment> UpdatePaymentStatus(int paymentId, PaymentStatus paymentStatus)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
            payment.PaymentStatus = paymentStatus;
            if (payment.PaymentStatus == PaymentStatus.PaymentBooked)
                payment.PaymentBookDate = DateTime.Now;
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
