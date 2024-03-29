﻿using AutoMapper;
using HouseCommunity.Data;
using HouseCommunity.Data.Interfaces;
using HouseCommunity.DTOs;
using HouseCommunity.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private readonly MessageHub _hub;

        public ChatController(IMapper mapper,
                              IChatRepository chatRepository,
                              IUserRepository userRepository,
                              MessageHub hub)
        {
            _mapper = mapper;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _hub = hub;
        }

        private async Task NotifyPlayers(string groupName, object messages)
        {
            await _hub.NewMessage(groupName, messages);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetConversations(int userId)
        {
            var userFromRepo = await _userRepository.GetUserById(userId);
            var users = (_userRepository.GetUsers()).Where(p => p.Id != userId);

            var conversations = (await _chatRepository.GetConversations(userId)).Select(p => p.Conversation);
            return Ok(users.Select(user =>
            {
                var inner = (_chatRepository.GetConversations(user.Id)).GetAwaiter().GetResult().Select(p => p.Conversation);

                var conversation = inner.Intersect(conversations).FirstOrDefault();
                var lastReadMessage = conversation?.Users.FirstOrDefault(p => p.User.Id == userId)?.LastMessageRead;
                return new UserForConversationDTO()
                {
                    Id = user.Id,
                    ConversationId = conversation?.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    UserRole = user.UserRole,
                    IsBuildingSame = user.UserFlats.Any(p =>
                    {
                        return userFromRepo.UserFlats.Select(p => p.Flat.BuildingId).Any(b => b == p.Flat?.BuildingId);
                    }),
                    AvatarUrl = user.AvatarUrl ?? @"https://housecommunitystorage.blob.core.windows.net/avatarcontainer/user_avatar.png",
                    NotReadMessages = conversation?.Messages.Where(p => p.Sender.Id == user.Id && p.Date > (lastReadMessage?.Date ?? DateTime.MinValue)).Count(),
                    ModificationDate = conversation?.ModificationDate
                };
            }
            ));
        }

        [HttpPost("read-message")]
        public async Task<IActionResult> ReadMessage(ReadMessageDTO readMessageDTO)
        {
            var userFromRepo = await _userRepository.GetUserById(readMessageDTO.UserId);
            var conversation = await _chatRepository.GetConversation(readMessageDTO.ConversationId);

            var userConservation = await _chatRepository.UpdateLastReadMessage(conversation, userFromRepo, conversation.Messages.Where(p => p.Sender.Id != readMessageDTO.UserId).OrderByDescending(p => p.Date).FirstOrDefault());
            return Ok();
        }

        [HttpGet("not-read-messages/{userId}")]
        public async Task<IActionResult> GetNotReadMessage(int userId)
        {
            var userFromRepo = await _userRepository.GetUserById(userId);
            var lastReadMessages = userFromRepo.UserConversations.Select(p => p.LastMessageRead);
            var conversations = userFromRepo.UserConversations.Select(p => p.Conversation);
            var amount = 0;
            foreach (var conversation in conversations)
            {
                var messages = conversation.Messages.Where(p => p.Sender.Id != userId).OrderBy(p => p.Date);
                if (messages == null || messages.Count() == 0)
                    continue;

                else
                {
                    if (conversation.Users.SingleOrDefault(p => p.User.Id == userId).LastMessageRead != messages.Last())
                        amount++;
                }
            }
            return Ok(new
            {
                amount = amount
            });
        }

        [HttpPost("save-message")]
        public async Task<IActionResult> SaveMessage(SaveMessageDTO message)
        {

            var userFromRepo = await _userRepository.GetUserById(message.UserId);
            var conversation = await _chatRepository.GetConversation(message.ConversationId);
            if (conversation != null)
            {
                await _chatRepository.SaveMessage(conversation, message, userFromRepo);
            }
            else
            {
                conversation = await _chatRepository.CreateConversation(message.UserId, message.ReceiverId);
                await _chatRepository.SaveMessage(conversation, message, userFromRepo);
            }
            var messagesToSent = conversation.Messages.Select(p =>
            {
                var msg = _mapper.Map<MessageDTO>(p);
                msg.Type = p.Sender.Id == message.UserId ? "sent" : "received";
                return msg;
            });
            await NotifyPlayers($"CONV_{conversation.Id}", new
            {
                Id = conversation.Id,
                Messages = messagesToSent
            });

            var users = (_userRepository.GetUsers()).Where(p => p.Id != message.UserId);
            return Ok(users.Select(user => new UserForConversationDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                UserRole = user.UserRole,
                AvatarUrl = user.AvatarUrl ?? @"https://housecommunitystorage.blob.core.windows.net/avatarcontainer/user_avatar.png",
                IsBuildingSame = user.UserFlats.Any(p =>
                {
                    return userFromRepo.UserFlats.Select(p => p.Flat.BuildingId).Any( b => b == p.Flat?.BuildingId);
                })

            }).ToList()
            ) ;
        }

        [HttpGet("join-groups/{id}")]
        public async Task<IActionResult> JoinGroups(int id)
        {
            var conversations = await _chatRepository.GetConversations(id);
            foreach (var conv in conversations.Select(p => p.Conversation).Distinct())
            {
                await _hub.JoinGroup($"CONV_{conv.Id}");
            }
            return Ok();
        }

        [HttpGet("leave-groups/{id}")]
        public async Task<IActionResult> LeaveGroups(int id)
        {
            var conversations = await _chatRepository.GetConversations(id);
            foreach (var conv in conversations)
            {
                await _hub.LeaveGroup($"CONV_{conv.Id}");
            }
            return Ok();
        }

        [HttpGet("get-messages/{id}/{userId}")]
        public async Task<IActionResult> GetMessages(int id, int userId)
        {
            var receiver = await _userRepository.GetUserById(id);
            var userFromRepo = await _userRepository.GetUserById(userId);

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
                //await _chatRepository.UpdateLastReadMessage(conversation.FirstOrDefault(), userFromRepo, messages.Where(p => p.Sender.Id != userId).OrderByDescending(p => p.Date).FirstOrDefault());
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