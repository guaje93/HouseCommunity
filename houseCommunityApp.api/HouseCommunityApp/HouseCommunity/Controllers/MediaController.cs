﻿using HouseCommunity.Data.Interfaces;
using HouseCommunity.Request;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaRepository _repo;

        public MediaController(IMediaRepository mediaRepository)
        {
            _repo = mediaRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> LoadMediaForUser(int id)
        {

            var media = await _repo.GetAllMediaForUser(id);
            if (media == null)
                return BadRequest();

            return Ok(media);
        }

        [HttpPost("add-media")]
        public async Task<IActionResult> AddMediaForUser(AddMediaToDbRequest addMediaToDbRequest)
        {
            var user = await _repo.AddMediaForUser(addMediaToDbRequest);

            if (user == null)
                return BadRequest();

            return Ok(user);
        }

    }
}