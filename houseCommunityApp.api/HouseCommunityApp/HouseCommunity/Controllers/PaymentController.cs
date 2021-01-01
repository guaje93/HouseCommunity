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
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using HouseCommunity.Services;

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
        private readonly IMailService _mailService;
        private readonly IUserRepository _userRepository;

        public PaymentController(IPaymentRepository paymentRepository, IPayURepository payURepository, IAuthRepository authRepository, IMapper mapper, IMailService mailService, IUserRepository userRepository
            )
        {
            _repo = paymentRepository;
            this._payURepository = payURepository;
            this._authRepository = authRepository;
            this._mapper = mapper;
            this._mailService = mailService;
            this._userRepository = userRepository;
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

                using (var content = new StringContent("{  \"notifyUrl\": \"https://2be5a1148f6e.ngrok.io/api/payment/update-order-status\",  \"customerIp\": \"127.0.0.1\",  \"merchantPosId\": \"398178\",  \"description\": \"RTV market\",  \"currencyCode\": \"PLN\",  \"totalAmount\": \"" + request.Price * 100 + "\",  \"products\": [    {      \"name\": \"Wireless mouse\",      \"unitPrice\": \"15000\",      \"quantity\": \"1\"    },    {      \"name\": \"HDMI cable\",      \"unitPrice\": \"6000\",      \"quantity\": \"1\"    }  ]}", System.Text.Encoding.Default, "application/json"))
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

        [HttpPost("remove-payment")]
        public async Task<IActionResult> DeletePayment(PaymentStatusUpdateDTO request)
        {
            var user = await _userRepository.GetUser(request.UserId);
            var access = await _authRepository.HasAccessToAdministration(request.UserId);

            if (!access)
                return BadRequest("User has no proper priviliges");
            
            await _repo.RemovePayment(request.PaymentId);

            var messageSubject = "Płatność usunięta";
            var messageContent = $"Płatnośc została usunięta przez użytkownika: {user.FirstName} {user.LastName}.\n" +
                                 $"W razie zastrzeżeń skontaktuj się z pracownikiem administracji. \n\n" +
                                 $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";
            _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");

            return Ok();
        }

        [HttpPost("unlock-payment")]
        public async Task<IActionResult> UnlockPayment(PaymentStatusUpdateDTO request)
        {
            var user = await _userRepository.GetUser(request.UserId);
            var access = await _authRepository.HasAccessToAdministration(request.UserId);

            if (!access)
                return BadRequest("User has no proper priviliges");

            var messageSubject = "Zmieniono status płatności";
            var messageContent = $"Płatnośc została zmieniona przez użytkownika: {user.FirstName} {user.LastName}.\n" +
                                 $"W razie zastrzeżeń skontaktuj się z pracownikiem administracji. \n\n" +
                                 $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";
            _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");

            var payment = await _repo.UpdatePaymentStatus(request.PaymentId, (PaymentStatus)(request.Status));
            return Ok();
        }

        [HttpPost("book-payment")]
        public async Task<IActionResult> BookPayment(PaymentStatusUpdateDTO request)
        {
            var user = await _userRepository.GetUser(request.UserId);
            var access = await _authRepository.HasAccessToAdministration(request.UserId);

            if (!access)
                return BadRequest("User has no proper priviliges");

            var messageSubject = "Płatność została zaksięgowana";
            var messageContent = $"Płatnośc została zaksięgowana przez użytkownika: {user.FirstName} {user.LastName}.\n" +
                                 $"W razie zastrzeżeń skontaktuj się z pracownikiem administracji. \n\n" +
                                 $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";
            _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");


            var payment = await _repo.UpdatePaymentStatus(request.PaymentId, PaymentStatus.PaymentBooked);
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
            var user = await _userRepository.GetUser(paymentDetailsToCreateEmptyDTO.UserId);
            var access = await _authRepository.HasAccessToAdministration(paymentDetailsToCreateEmptyDTO.UserId);

            if (!access)
                return BadRequest("User has no proper priviliges");

            var payments = await _repo.GetPayments(paymentDetailsToCreateEmptyDTO.FlatId);
            if (payments != null && payments.Where(p => p.Type == PaymentType.RENT).Any(p => p.Month == paymentDetailsToCreateEmptyDTO.Period.Month && p.Year == paymentDetailsToCreateEmptyDTO.Period.Year))
                return BadRequest("Czynsz za ten miesiąc został już wygenerowany");

            var payment = await _repo.CreateNewPayment(paymentDetailsToCreateEmptyDTO);

            var messageSubject = "Stworzono polecenie płatności";
            var messageContent = $"Płatnośc została stworzona przez użytkownika: {user.FirstName} {user.LastName}.\n" +
                                 $"W razie zastrzeżeń skontaktuj się z pracownikiem administracji. \n\n" +
                                 $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";
            _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");


            //_mapper.Map<UserDto>(user);
            return Ok();
        }

        [HttpPost("create-new-custom-payment")]
        public async Task<IActionResult> CreateNewCustomPayment(CustomPaymentDetailsDTO customPaymentDetails)
        {
            var user = await _userRepository.GetUser(customPaymentDetails.UserId);
            var access = await _authRepository.HasAccessToAdministration(customPaymentDetails.UserId);

            if (!access)
                return BadRequest("User has no proper priviliges");
            
            var payment = await _repo.CreateNewPayment(customPaymentDetails);

            var messageSubject = "Stworzono polecenie płatności";
            var messageContent = $"Płatnośc została stworzona przez użytkownika: {user.FirstName} {user.LastName}.\n" +
                                 $"W razie zastrzeżeń skontaktuj się z pracownikiem administracji. \n\n" +
                                 $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";
            _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");

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

            var hotWaterRealUsageInLastPeriod = 0.0;
            var hotWaterEstimatedUsageInLastPeriodExceptCurrent = 0.0;
            var coldWaterRealUsageInLastPeriod = 0.0;
            var coldWaterEstimatedUsageInLastPeriodExceptCurrent = 0.0;
            var heatingRealUsageInLastPeriod = 0.0;
            var heatingEstimatedUsageInLastPeriodExceptCurrent = 0.0;

            var paymentDetails = new PaymentDetail()
            {
                AdministrationValue = Math.Round(flatArea * unitCosts.AdministrationUnitCost, 2),
                AdministrationDescription = $"Powierzchnia mieszkania: {flatArea}m2 * koszt jednostkowy: {unitCosts.AdministrationUnitCost}zł",
                GarbageValue = Math.Round(residentsAmount * unitCosts.GarbageUnitCost, 2),
                GarbageDescription = $"Liczba mieszkańców: {residentsAmount} * koszt jednostkowy: {unitCosts.GarbageUnitCost}zł",
                ColdWaterValue = Math.Round(coldWaterEstimatedUsage * unitCosts.ColdWaterUnitCost, 2),
                ColdWaterDescription = $"Prognozowane zużycie zimnej wody/msc: {coldWaterEstimatedUsage}m3 * koszt jednostkowy: {unitCosts.ColdWaterUnitCost}zł/m3",
                HotWaterValue = Math.Round(hotWaterEstimatedUsage * (unitCosts.HotWaterUnitCost + unitCosts.ColdWaterUnitCost), 2),
                HotWaterDescription = $"Prognozowane zużycie ciepłej wody/msc: {hotWaterEstimatedUsage}m3 * (koszt jednostkowy: {unitCosts.ColdWaterUnitCost}zł/m3 + koszt ogrzania wody: {unitCosts.HotWaterUnitCost}zł/m3)",
                HeatingValue = Math.Round(heatingEstimatedUsage * flatArea * unitCosts.HeatingUnitCost, 2),
                HeatingDescription = $"Prognozowane zużycie energii: {heatingEstimatedUsage}GJ/m2 * Powierzchnia mieszkania: {flatArea}m2 * koszt jednostkowy: {unitCosts.HeatingUnitCost}zł/GJ",
            };

            if (date.Month == 1 || date.Month == 7)
            {
                var mediaUsageInLastPeriod = await _repo.GetMediaFromLastPeriod(flatId, date);
                hotWaterRealUsageInLastPeriod = mediaUsageInLastPeriod.Where(p => p.MediaType == MediaEnum.HotWater).Sum(p => p.CurrentValue);
                coldWaterRealUsageInLastPeriod = mediaUsageInLastPeriod.Where(p => p.MediaType == MediaEnum.ColdWater).Sum(p => p.CurrentValue);
                heatingRealUsageInLastPeriod = mediaUsageInLastPeriod.Where(p => p.MediaType == MediaEnum.Heat).Sum(p => p.CurrentValue);

                paymentDetails.HeatingRefundValue = Math.Round((heatingRealUsageInLastPeriod - heatingEstimatedUsageInLastPeriodExceptCurrent - heatingEstimatedUsage) * unitCosts.HeatingUnitCost, 2);
                paymentDetails.HeatingRefundDescription = $"(Rzeczywiste zużycie energii: {heatingRealUsageInLastPeriod}GJ (ostatnie pół roku) - Opłacone zużycie energii (ostatnie pół roku): {heatingEstimatedUsageInLastPeriodExceptCurrent + heatingEstimatedUsage}GJ) * koszt jednostkowy: {unitCosts.HeatingUnitCost}zł/GJ";
                paymentDetails.WaterRefundValue = Math.Round((hotWaterRealUsageInLastPeriod - hotWaterEstimatedUsageInLastPeriodExceptCurrent - hotWaterEstimatedUsage) * unitCosts.HotWaterUnitCost +
                                   (coldWaterRealUsageInLastPeriod - coldWaterEstimatedUsageInLastPeriodExceptCurrent - coldWaterEstimatedUsage) * unitCosts.ColdWaterUnitCost, 2);
                paymentDetails.WaterRefundDescription = $"(Rzeczywiste zużycie wody ciepłej (ostatnie pół roku): {hotWaterRealUsageInLastPeriod}m3 - Opłacone zużycie wody ciepłej (ostatnie pół roku): {hotWaterEstimatedUsageInLastPeriodExceptCurrent + hotWaterEstimatedUsage}m3) * koszt jednostkowy: {unitCosts.HotWaterUnitCost}zł/m3 + " +
                $"(Rzeczywiste zużycie wody zimnej (ostatnie pół roku): {coldWaterRealUsageInLastPeriod}m3 - Opłacone zużycie wody zimnej (ostatnie pół roku): {coldWaterEstimatedUsageInLastPeriodExceptCurrent + coldWaterEstimatedUsage}m3) * koszt jednostkowy: {unitCosts.ColdWaterUnitCost}zł/m3";

            }
            return paymentDetails;
        }
    }
}