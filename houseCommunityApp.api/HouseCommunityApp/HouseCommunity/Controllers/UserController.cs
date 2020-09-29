using HouseCommunity.Data;
using HouseCommunity.Request;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpPut("update-contact-data")]
        public async Task<IActionResult> UpdateUserContactData(UserContactData userContactData)
        {
            var userFromRepo = await _repo.UpdateUserContactData(userContactData);

            if (userFromRepo == null)
                return Unauthorized();

            return Ok(
               userFromRepo
               );
        }
    }
}