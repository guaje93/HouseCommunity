using AutoMapper;
using HouseCommunity.Data;
using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _repo;
        private readonly IPayURepository _payURepository;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentRepository paymentRepository, IPayURepository payURepository, IAuthRepository authRepository, IMapper mapper)
        {
            _repo = paymentRepository;
            this._payURepository = payURepository;
            this._authRepository = authRepository;
            this._mapper = mapper;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotPerformedPaymentsByUserId(int id)
        {

            var payments = await _repo.GetPayments(id);

            if (payments == null)
                return NoContent();

            return Ok(
               payments
               );
        }

        [HttpPost("get-access-token")]
        public async Task<string> GetAccessToken()
        {
            var baseAddress = new Uri("https://secure.snd.payu.com/");
            string responseData = "";
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                //using (var content = new StringContent($"grant_type=client_credentials&client_id=398178&client_secret=a6022f5602805e6919af0e71f8885e3b", System.Text.Encoding.Default, "application/x-www-form-urlencoded"))


                using (var content = new StringContent($"grant_type=client_credentials&client_id={_payURepository.GetClientId()}&client_secret={_payURepository.GetClientSecret()}", System.Text.Encoding.Default, "application/x-www-form-urlencoded"))
                {
                    using (var response = await httpClient.PostAsync("pl/standard/user/oauth/authorize", content))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return responseData;
        }

        [HttpPost("create-new-order")]
        public async Task<IActionResult> CreateNewOrder(NewOrderRequest request)
        {
            var baseAddress = new Uri("https://secure.snd.payu.com/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                var tokenResponse = await GetAccessToken();
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenResponse);
                string responseData = "";
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", $"Bearer {values["access_token"]}");

                using (var content = new StringContent("{  \"notifyUrl\": \"https://64c9a06c0868.ngrok.io/api/payment/update-order-status\",  \"customerIp\": \"127.0.0.1\",  \"merchantPosId\": \"398178\",  \"description\": \"RTV market\",  \"currencyCode\": \"PLN\",  \"totalAmount\": \"" + request.Price * 100 + "\",  \"products\": [    {      \"name\": \"Wireless mouse\",      \"unitPrice\": \"15000\",      \"quantity\": \"1\"    },    {      \"name\": \"HDMI cable\",      \"unitPrice\": \"6000\",      \"quantity\": \"1\"    }  ]}", System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("api/v2_1/orders/", content))
                    {
                        string orderId = "";
                        responseData = response.RequestMessage.RequestUri.AbsoluteUri;
                        var parameters = responseData.Split('?')[1].Split('&').ToDictionary(p => p.Split('=')[0], g => g.Split('=')[1]);
                        orderId = parameters["orderId"];
                        var payment = await _repo.UpdatePayUOrderId(request.Id, orderId);
                    }
                }
                return Ok(new { path = responseData });
            }
        }

        [AllowAnonymous]
        [HttpPost("update-order-status")]
        public async Task<IActionResult> UpdateOrderStatus(PayUResponseDTO request)
        {
            var payment = await _repo.UpdateOrderStatus(request.Order.OrderId, request.Order.Status);
            return Ok();
        }

        [HttpPost("calculate-costs/{flatId}")]
        public async Task<IActionResult> GetCalculatedCostsForPayment(int flatId, [FromBody] DateTime date)
        {
            PaymentDetail paymentDetails = await GeneratePaymentDetails(flatId, date);

            return Ok(paymentDetails);
        }

        [HttpPost("create-new-payment")]
        public async Task<IActionResult> CreateNewPayment(PaymentDetailsToCreateEmptyDTO paymentDetailsToCreateEmptyDTO)
        {
            var access = await _authRepository.HasAccessToAdministration(paymentDetailsToCreateEmptyDTO.UserId);

            if (!access)
                return BadRequest("User has no proper priviliges");

            var payments = await _repo.GetPayments(paymentDetailsToCreateEmptyDTO.UserId);
            if (payments.Any(p => p.Month == paymentDetailsToCreateEmptyDTO.Period.Month && p.Year == paymentDetailsToCreateEmptyDTO.Period.Year))
                return BadRequest("One payment already created");

            var payment = await _repo.CreateNewPayment(paymentDetailsToCreateEmptyDTO.FlatId, paymentDetailsToCreateEmptyDTO);
            //_mapper.Map<UserDto>(user);
            return Ok();
        }

        private async Task<PaymentDetail> GeneratePaymentDetails(int flatId, DateTime date)
        {
            var unitCosts = await _repo.GetUnitCostsForFlat(flatId);
            var coldWaterEstimatedUsage = await _repo.GetEstimatedMediaUsage(flatId, MediaEnum.ColdWater);
            var hotWaterEstimatedUsage = await _repo.GetEstimatedMediaUsage(flatId, MediaEnum.HotWater);
            var heatingEstimatedUsage = await _repo.GetEstimatedMediaUsage(flatId, MediaEnum.Heat);
            var residentsAmount = await _repo.GetResidentsAmount(flatId);
            var buildingArea = await _repo.GetWholeBuildingArea(flatId);
            var flatArea = await _repo.GetFlatArea(flatId);
            var flatAreaRatio = Math.Round(flatArea / buildingArea, 2);

            var hotWaterRealUsageInLastPeriod = 0.0;
            var hotWaterEstimatedUsageInLastPeriodExceptCurrent = 0.0;
            var coldWaterRealUsageInLastPeriod = 0.0;
            var coldWaterEstimatedUsageInLastPeriodExceptCurrent = 0.0;
            var heatingRealUsageInLastPeriod = 0.0;
            var heatingEstimatedUsageInLastPeriodExceptCurrent = 0.0;

            if (date.Month == 1 || date.Month == 7)
            {
                var mediaUsageInLastPeriod = await _repo.GetMediaFromLastPeriod(flatId, date);
                hotWaterRealUsageInLastPeriod = mediaUsageInLastPeriod.Where(p => p.MediaType == MediaEnum.HotWater).Sum(p => p.CurrentValue);
                coldWaterRealUsageInLastPeriod = mediaUsageInLastPeriod.Where(p => p.MediaType == MediaEnum.ColdWater).Sum(p => p.CurrentValue);
                heatingRealUsageInLastPeriod = mediaUsageInLastPeriod.Where(p => p.MediaType == MediaEnum.Heat).Sum(p => p.CurrentValue);
            }

            var paymentDetails = new PaymentDetail()
            {
                AdministrationValue = Math.Round(flatAreaRatio * unitCosts.AdministrationUnitCost, 2),
                AdministrationDescription = $"Udział powierzchni mieszkania w powierzchni budynku: {flatAreaRatio} * koszt jednostkowy: {unitCosts.AdministrationUnitCost}zł",
                GarbageValue = Math.Round(residentsAmount * unitCosts.GarbageUnitCost, 2),
                GarbageDescription = $"Liczba mieszkańców: {residentsAmount} * koszt jednostkowy: {unitCosts.GarbageUnitCost}zł",
                OperatingCostValue = Math.Round(residentsAmount * unitCosts.OperatingUnitCost, 2),
                OperatingCostDescription = $"Udział powierzchni mieszkania w powierzchni budynku: {flatAreaRatio} * koszt jednostkowy: {unitCosts.OperatingUnitCost}zł",
                ColdWaterValue = Math.Round(coldWaterEstimatedUsage * unitCosts.ColdWaterUnitCost, 2),
                ColdWaterDescription = $"Prognozowane zużycie zimnej wody/msc: {coldWaterEstimatedUsage}m3 * koszt jednostkowy: {unitCosts.ColdWaterUnitCost}zł/m3",
                HotWaterValue = Math.Round(hotWaterEstimatedUsage * unitCosts.HotWaterUnitCost, 2),
                HotWaterDescription = $"Prognozowane zużycie ciepłej wody/msc: {hotWaterEstimatedUsage}m3 * koszt jednostkowy: {unitCosts.HotWaterUnitCost}zł/m3",
                HeatingValue = Math.Round(heatingEstimatedUsage * unitCosts.HeatingUnitCost, 2),
                HeatingDescription = $"Prognozowane zużycie energii: {heatingEstimatedUsage}GJ * koszt jednostkowy: {unitCosts.HeatingUnitCost}zł/GJ",
                HeatingRefundValue = Math.Round((heatingRealUsageInLastPeriod - heatingEstimatedUsageInLastPeriodExceptCurrent - heatingEstimatedUsage) * unitCosts.HeatingUnitCost, 2),
                HeatingRefundDescription = $"(Rzeczywiste zużycie energii: {heatingRealUsageInLastPeriod}GJ (ostatnie pół roku) - Opłacone zużycie energii (ostatnie pół roku): {heatingEstimatedUsageInLastPeriodExceptCurrent + heatingEstimatedUsage}GJ) * koszt jednostkowy: {unitCosts.HeatingUnitCost}zł/GJ",

                WaterRefundValue = Math.Round((hotWaterRealUsageInLastPeriod - hotWaterEstimatedUsageInLastPeriodExceptCurrent - hotWaterEstimatedUsage) * unitCosts.HotWaterUnitCost +
                                   (coldWaterRealUsageInLastPeriod - coldWaterEstimatedUsageInLastPeriodExceptCurrent - coldWaterEstimatedUsage) * unitCosts.ColdWaterUnitCost, 2),
                WaterRefundDescription = $"(Rzeczywiste zużycie wody ciepłej (ostatnie pół roku): {hotWaterRealUsageInLastPeriod}m3 - Opłacone zużycie wody ciepłej (ostatnie pół roku): {hotWaterEstimatedUsageInLastPeriodExceptCurrent + hotWaterEstimatedUsage}m3) * koszt jednostkowy: {unitCosts.HotWaterUnitCost}zł/m3 + " +
                $"(Rzeczywiste zużycie wody zimnej (ostatnie pół roku): {coldWaterRealUsageInLastPeriod}m3 - Opłacone zużycie wody zimnej (ostatnie pół roku): {coldWaterEstimatedUsageInLastPeriodExceptCurrent + coldWaterEstimatedUsage}m3) * koszt jednostkowy: {unitCosts.ColdWaterUnitCost}zł/m3"
            };
            return paymentDetails;
        }
    }
}