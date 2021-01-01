using AutoMapper;
using HouseCommunity.Data;
using HouseCommunity.Data.Interfaces;
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
        private readonly IBuildingRepository _buildingRepository;

        public UserController(IUserRepository userRepository, IMapper mapper, IBuildingRepository buildingRepository)
        {
            _repo = userRepository;
            _mapper = mapper;
            _buildingRepository = buildingRepository;
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
            var flatsFromRepo = await _buildingRepository.GetFlats();
            var users = flatsFromRepo.SelectMany(prop => prop.Residents);

            if (flatsFromRepo == null)
                return BadRequest();

            return Ok(users.Select(p =>
               _mapper.Map<FlatForFilteringDTO>(p)));

        }

        [HttpGet("get-residents-list")]
        public async Task<IActionResult> GetAllResidentsForREgister()
        {

            var usersFromRepo = await _repo.GetUsersWithRole(UserRole.Resident);

            if (usersFromRepo == null)
                return BadRequest();

            return Ok(
               usersFromRepo.Select(p => _mapper.Map<ResidentsForRegisterDTO>(p))
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