using AutoMapper;
using HouseCommunity.Data;
using HouseCommunity.Data.Interfaces;
using HouseCommunity.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;

        public ChatController(IMapper mapper, IChatRepository chatRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetConversations(int userId)
        {
            var userFromRepo = await _userRepository.GetUser(userId);
            var users = (await _userRepository.GetUsers()).Where(p => p.Id != userId);
            return Ok(users.Select(user => new UserForConversationDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                UserRole = user.UserRole,
                IsBuildingSame = user.Flat?.BuildingId == userFromRepo.Flat?.BuildingId

            }).ToList()
            );
        }

        [HttpPost("save-message")]
        public async Task<IActionResult> SaveMessage(SaveMessageDTO message)
        {

            var userFromRepo = await _userRepository.GetUser(message.UserId);
            var conversation = await _chatRepository.GetConversation(message.ConversationId);  
            if(conversation != null)
            {
                await _chatRepository.SaveMessage(conversation, message, userFromRepo);
            }
            else
            {
                conversation = await _chatRepository.CreateConversation(message.UserId, message.ReceiverId);
                await _chatRepository.SaveMessage(conversation, message, userFromRepo);

            }
            var users = (await _userRepository.GetUsers()).Where(p => p.Id != message.UserId);
            return Ok(users.Select(user => new UserForConversationDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                UserRole = user.UserRole,
                IsBuildingSame = user.Flat?.BuildingId == userFromRepo.Flat?.BuildingId

            }).ToList()
            );
        }


        [HttpGet("get-messages/{id}/{userId}")]
        public async Task<IActionResult> GetMessages(int id, int userId)
        {
            var receiver = await _userRepository.GetUser(id);
            var userFromRepo = await _userRepository.GetUser(userId);

            //check if any conversation is pending
            var userConversations = await _chatRepository.GetConversations(userId);
            var receiverConversations = await _chatRepository.GetConversations(id);
            var conversation = userConversations.Select(p => p.Conversation).Intersect(receiverConversations.Select(p => p.Conversation));
            if (conversation?.Count() == 1)
            {
                var messages = await _chatRepository.GetMessages(conversation.First().Id);
                var messagesToSent = messages.Select(p =>
                {
                    var msg = _mapper.Map<MessageDTO>(p);
                    msg.Type = p.Sender.Id == userId ? "sent" : "received";
                    return msg;
                });

                return Ok(new
                {
                    Id = conversation.First().Id,
                    Messages = messagesToSent
                });
            }
            else
                return Ok();
        }
    }
}