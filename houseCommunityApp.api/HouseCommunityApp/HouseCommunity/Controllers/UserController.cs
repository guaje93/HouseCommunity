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

        [HttpGet("get-all-residents")]
        public async Task<IActionResult> GetAllUsers()
        {

            var usersFromRepo = await _repo.GetResidents();

            if (usersFromRepo == null)
                return BadRequest();

            return Ok(
               usersFromRepo);
        }


        [HttpPut("update-contact-data")]
        public async Task<IActionResult> UpdateUserContactData(UserDefinedData userContactData)
        {
            var userFromRepo = await _repo.UpdateUserDefinedData(userContactData);

            if (userFromRepo == null)
                return Unauthorized();

            return Ok(
               userFromRepo
               );
        }
    }
}