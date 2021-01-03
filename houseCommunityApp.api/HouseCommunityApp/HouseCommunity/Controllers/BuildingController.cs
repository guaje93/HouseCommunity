using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HouseCommunity.Data;
using HouseCommunity.Data.Interfaces;
using HouseCommunity.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingRepository _repo;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public BuildingController(IBuildingRepository flatRepository, IUserRepository userRepository, IMapper mapper)
        {
            _repo = flatRepository;
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        [HttpGet("get-building-for-manager/{userId}")]
        public async Task<IActionResult> GetBuildingDataForUser(int userId)
        {

            var buildingFromRepo = await _repo.GetBuilding(userId);

            if (buildingFromRepo == null)
                return BadRequest();

            return Ok(
               new
               {
                   Id = buildingFromRepo.Id,
                   City = buildingFromRepo.Address.City,
                   Street = buildingFromRepo.Address.Street,
                   Number = buildingFromRepo.Address.Number,

               });
        }

        [HttpGet("get-buildings")]
        public async Task<IActionResult> GetBuildings()
        {

            var buildingsFromRepo = await _repo.GetBuildings();

            if (buildingsFromRepo == null)
                return BadRequest();

            return Ok(
                buildingsFromRepo.Select(p =>
               new
               {
                   Id = p.Id,
                   Address = p.Address.ToString()
               }));
        }

        [HttpGet("get-flats/{userId}")]
        public async Task<IActionResult> GetAllFlatsForBuildingManager(int userId)
        {

            var flatsFromRepo = await _repo.GetFlats();
            var building = await _repo.GetBuilding(userId);
            flatsFromRepo = flatsFromRepo.Where(prop => prop.Building == building).ToList();
            if (flatsFromRepo == null)
                return BadRequest();

            return Ok(
               flatsFromRepo.Select(p =>
               _mapper.Map<FlatsForListDTO>(p)));
        }

        [HttpGet("get-flats-for-filtering/{userId}")]
        public async Task<IActionResult> GetFlatsForFiltering(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            var flatsFromRepo = await _repo.GetFlats();

            if (user.UserRole == Model.UserRole.HouseManager)
                flatsFromRepo = flatsFromRepo.Where(p => p.Building.HouseManager == user).ToList();

            var users = flatsFromRepo.SelectMany(prop => prop.Residents);

            if (flatsFromRepo == null)
                return BadRequest();

            return Ok(users.Select(p =>
               _mapper.Map<FlatForFilteringDTO>(p)));
        }

        [HttpGet("get-flat-residents/{flatId}")]
        public async Task<IActionResult> GetFlatResidents(int flatId)
        {

            var flat = await _repo.GetFlat(flatId);

            if (flat == null)
                return BadRequest();
            var users = flat.Residents.Select(p => _mapper.Map<ResidentToContactDTO>(p));

            return Ok(
               users);
        }


    }
}