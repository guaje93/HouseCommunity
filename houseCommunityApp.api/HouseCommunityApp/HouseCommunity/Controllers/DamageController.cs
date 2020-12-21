using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HouseCommunity.Data;
using HouseCommunity.Data.Interfaces;
using HouseCommunity.DTOs;
using HouseCommunity.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DamageController : ControllerBase
    {
        #region Fields

        private readonly IDamageRepository _repo;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public DamageController(IDamageRepository repo, IMapper mapper)
        {
            this._repo = repo;
            this._mapper = mapper;
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> InsertDamage(AddDamageDTO addDamageDTO)
        {
            var damage = await _repo.AddDamage(addDamageDTO);
            return Ok(new
            {
                damage.Id
            }
            );
        }

        [HttpPost("update-image")]
        public async Task<IActionResult> UpdateImage(AddImageDTO addDamageDTO)
        {
            var damage = await _repo.AddImage(addDamageDTO);
            return Ok(new
            {
                damage.Id
            }
            );
        }

        [HttpGet("get-damages-for-building/{id}")]
        public IActionResult GetDamages(int id)
        {
            var damages = _repo.GetDamagesForHouseManager(id, DamageStatus.WaitingForFix);
            return Ok(damages.Select( p => _mapper.Map<GetDamageForHouseManagerDTO>(p)
            ));
        }

        [HttpGet("get-fixed-damages-for-building/{id}")]
        public IActionResult GetFixedDamages(int id)
        {
            var damages = _repo.GetDamagesForHouseManager(id, DamageStatus.Fixed);
            return Ok(damages.Select(p => _mapper.Map<GetDamageForHouseManagerDTO>(p)
            ));
        }

        [HttpPost("fix-damage")]
        public async Task<IActionResult> FixDamage([FromBody]int id)
        {
            var damage = await _repo.ChangeStatus(id, DamageStatus.Fixed);
            return Ok(_mapper.Map<GetDamageForHouseManagerDTO>(damage)
            );
        }

        [HttpPost("revert-fix")]
        public async Task<IActionResult> RevertDamage([FromBody]int id)
        {
            var damage = await _repo.ChangeStatus(id, DamageStatus.WaitingForFix);
            return Ok(_mapper.Map<GetDamageForHouseManagerDTO>(damage)
            );
        }
    }
}
