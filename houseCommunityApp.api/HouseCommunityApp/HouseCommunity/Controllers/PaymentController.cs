using HouseCommunity.Data;
using HouseCommunity.DTOs;
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

        public PaymentController(IPaymentRepository paymentRepository, IPayURepository payURepository)
        {
            _repo = paymentRepository;
            this._payURepository = payURepository;
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
                
                using (var content = new StringContent("{  \"notifyUrl\": \"https://64c9a06c0868.ngrok.io/api/payment/update-order-status\",  \"customerIp\": \"127.0.0.1\",  \"merchantPosId\": \"398178\",  \"description\": \"RTV market\",  \"currencyCode\": \"PLN\",  \"totalAmount\": \"" + request.Price * 100 +"\",  \"products\": [    {      \"name\": \"Wireless mouse\",      \"unitPrice\": \"15000\",      \"quantity\": \"1\"    },    {      \"name\": \"HDMI cable\",      \"unitPrice\": \"6000\",      \"quantity\": \"1\"    }  ]}", System.Text.Encoding.Default, "application/json"))
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
            var payment = await _repo.UpdateOrderStatus(request.Order.OrderId,request.Order.Status);
            return Ok();
        }

    }
}