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
        #region Fields

        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IBuildingRepository _buildingRepository;

        #endregion

        #region Constructor

        public UserController(IUserRepository userRepository, IMapper mapper, IBuildingRepository buildingRepository)
        {
            _repo = userRepository;
            _mapper = mapper;
            _buildingRepository = buildingRepository;
        }

        #endregion //Constructor

        #region Methods

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var userFromRepo = await _repo.GetUserById(id);

            if (userFromRepo == null)
                return Unauthorized();

            return Ok(_mapper.Map<UserForInfoDTO>(userFromRepo));
        }

        [HttpGet("get-all-residents")]
        public async Task<IActionResult> GetAllResidents()
        {
            var users = await _repo.GetUsersByRole(UserRole.Resident);

            if (users == null)
                return BadRequest();

            return Ok(
                users.SelectMany(p => p.UserFlats).Distinct().Select(p =>_mapper.Map<FlatForFilteringDTO>(p))
                );

        }

        [HttpGet("get-residents-for-register")]
        public async Task<IActionResult> GetAllResidentsForRegister()
        {
            var usersFromRepo = await _repo.GetUsersByRole(UserRole.Resident);

            if (usersFromRepo == null)
                return BadRequest();

            return Ok(
               usersFromRepo.Select(p => _mapper.Map<ResidentsForRegisterDTO>(p))
               );
        }

        [HttpPut("update-contact-data")]
        public async Task<IActionResult> UpdateUserContactData(UserDefinedData userContactData)
        {
            var user = await _repo.GetUserById(userContactData.Id);

            if(user== null)
                return BadRequest();

            user.PhoneNumber = userContactData.PhoneNumber;
            user.Email = userContactData.Email;
            user.AvatarUrl = userContactData.AvatarUrl;

            foreach (var flatInfo in userContactData.UserFlats)
            {
                var flat = await _buildingRepository.GetFlat(flatInfo.Id);

                if (flat != null)
                {
                    flat.ResidentsAmount = flatInfo.ResidentsAmount;
                    flat.ColdWaterEstimatedUsage = flatInfo.ColdWaterEstimatedUsage;
                    flat.HotWaterEstimatedUsage = flatInfo.HotWaterEstimatedUsage;
                    flat.HeatingEstimatedUsage = flatInfo.HeatingEstimatedUsage;
                }
    
                var flatFromRepo = await _buildingRepository.UpdateFlat(flat);
            }

            var userFromRepo = await _repo.UpdateUser(user);

            return Ok(
               _mapper.Map<UserForInfoDTO>(userFromRepo)
               );
        }

        #endregion //Methods
    }
}