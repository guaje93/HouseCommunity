using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseCommunity.Data;
using HouseCommunity.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UserController(IUserRepository userRepository)
        {
            _repo = userRepository;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
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