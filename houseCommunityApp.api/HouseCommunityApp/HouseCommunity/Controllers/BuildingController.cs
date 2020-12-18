using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseCommunity.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private IBuildingRepository _repo;

        public BuildingController(IBuildingRepository flatRepository)
        {
            _repo = flatRepository;
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
    }
}