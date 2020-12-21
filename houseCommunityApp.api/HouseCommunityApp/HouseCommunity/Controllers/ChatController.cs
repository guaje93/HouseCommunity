using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HouseCommunity.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChatRepository _chatRepository;

        public ChatController(IMapper mapper, IChatRepository chatRepository)
        {
            _mapper = mapper;
            _chatRepository = chatRepository;
        }


    }
}