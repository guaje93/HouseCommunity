using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseCommunity.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _repo;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _repo = paymentRepository;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotPerformedPaymentsByUserId(int id)
        {

            var userFromRepo = await _repo.GetUser(id);

            if (userFromRepo == null)
                return Unauthorized();

            return Ok(
               userFromRepo
               );
        }
    }
}