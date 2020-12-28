using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public BuildingController(IBuildingRepository flatRepository, IMapper mapper)
        {
            _repo = flatRepository;
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

        [HttpGet("get-flats/{userId}")]
        public async Task<IActionResult> GetAllFlatsForBuildingManager(int userId)
        {

            var flatsFromRepo = await _repo.GetFlats(userId);

            if (flatsFromRepo == null)
                return BadRequest();

            return Ok(
               flatsFromRepo);
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