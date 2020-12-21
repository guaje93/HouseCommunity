using AutoMapper;
using HouseCommunity.Data;
using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Request;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _repo = userRepository;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {

            var userFromRepo = await _repo.GetUser(id);

            if (userFromRepo == null)
                return Unauthorized();

            return Ok(
               _mapper.Map<UserForInfoDTO>(userFromRepo)
               );
        }

        [HttpGet("get-all-residents")]
        public async Task<IActionResult> GetAllUsers()
        {

            var usersFromRepo = await _repo.GetUsersWithRole(UserRole.Resident);

            if (usersFromRepo == null)
                return BadRequest();

            return Ok(
               usersFromRepo.Select(p => _mapper.Map<ResidentsForListDTO>(p))
               );
        }


        [HttpPut("update-contact-data")]
        public async Task<IActionResult> UpdateUserContactData(UserDefinedData userContactData)
        {
            var userFromRepo = await _repo.UpdateUserDefinedData(userContactData);

            if (userFromRepo == null)
                return BadRequest();

            return Ok(
               _mapper.Map<UserForInfoDTO>(userFromRepo)
               );
        }
    }
}