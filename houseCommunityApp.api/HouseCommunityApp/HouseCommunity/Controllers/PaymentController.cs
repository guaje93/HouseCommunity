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
using HouseCommunity.Data.Interfaces;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _repo;
        private readonly IRentCostsBuilder _paymentCostsBuilder;
        private readonly IPayURepository _paymentProviderRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IUserRepository _userRepository;
        private readonly IBuildingRepository _buildingRepository;

        public PaymentController(IPaymentRepository paymentRepository, IPayURepository payURepository, IAuthRepository authRepository, IMapper mapper, IMailService mailService, IUserRepository userRepository, IBuildingRepository buildingRepository, IRentCostsBuilder paymentCostsBuilder
            )
        {
            _repo = paymentRepository;
            _paymentProviderRepository = payURepository;
            _authRepository = authRepository;
            _mapper = mapper;
            _mailService = mailService;
            _userRepository = userRepository;
            _buildingRepository = buildingRepository;
            _paymentCostsBuilder = paymentCostsBuilder;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentsByUserId(int id)
        {

            var payments = await _repo.GetPayments(id);

            if (payments == null)
                return NoContent();

            return Ok(
               payments
               );
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
                        var payment = await _paymentProviderRepository.UpdatePaymentOrderId(request.Id, orderId);
                    }
                }
                return Ok(new { path = responseData });
            }
        }

        [AllowAnonymous]
        [HttpPost("update-order-status")]
        public async Task<IActionResult> UpdateOrderStatus(PayUResponseDTO request)
        {
            var payment = await _paymentProviderRepository.UpdatePaymentOrderStatus(request.Order.OrderId, request.Order.Status);
            return Ok();
        }

        [HttpPost("remove-payment")]
        public async Task<IActionResult> DeletePayment(PaymentStatusUpdateDTO request)
        {
            var user = await _userRepository.GetUserById(request.UserId);
            var access = await _authRepository.HasAdministrationRights(request.UserId);

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
            var user = await _userRepository.GetUserById(request.UserId);
            var access = await _authRepository.HasAdministrationRights(request.UserId);

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
            var user = await _userRepository.GetUserById(request.UserId);
            var access = await _authRepository.HasAdministrationRights(request.UserId);

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
            var builder = new RentCostsBuilder();
            var flat = await _buildingRepository.GetFlat(flatId);
            builder.CalculatePaymentDetails(flat);

            if (date.Month == 1 || date.Month == 7)
            {
                var mediaUsageInLastPeriod = await _repo.GetMediaFromLastPeriod(flatId, date);
                builder.CalculatePaymentRefunds(mediaUsageInLastPeriod, flat);
            }

            return Ok(builder.Build());
        }

        [HttpPost("create-new-payment")]
        public async Task<IActionResult> CreateNewPayment(PaymentDetailsToCreateEmptyDTO paymentDetailsToCreateEmptyDTO)
        {
            var user = await _userRepository.GetUserById(paymentDetailsToCreateEmptyDTO.UserId);
            var access = await _authRepository.HasAdministrationRights(paymentDetailsToCreateEmptyDTO.UserId);

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
            var user = await _userRepository.GetUserById(customPaymentDetails.UserId);
            var access = await _authRepository.HasAdministrationRights(customPaymentDetails.UserId);

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

        private async Task<string> GetAccessToken()
        {
            var baseAddress = new Uri("https://secure.snd.payu.com/");
            string responseData = "";
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                using (var content = new StringContent($"grant_type=client_credentials&client_id={_paymentProviderRepository.GetClientId()}&client_secret={_paymentProviderRepository.GetClientSecret()}", System.Text.Encoding.Default, "application/x-www-form-urlencoded"))
                {
                    using (var response = await httpClient.PostAsync("pl/standard/user/oauth/authorize", content))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return responseData;
        }
    }
}