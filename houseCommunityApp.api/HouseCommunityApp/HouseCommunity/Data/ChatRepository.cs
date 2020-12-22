using AutoMapper;
using HouseCommunity.Data.Interfaces;
using HouseCommunity.DTOs;
using HouseCommunity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public class ChatRepository : IChatRepository
    {
        private readonly DataContext _dataContext;

        public ChatRepository(DataContext _dataContext)
        {
            this._dataContext = _dataContext;
        }

        public async Task<Conversation> CreateConversation(int userId, int receiverId)
        {
            var user = await _dataContext.Users.Include(p => p.UserConversations).ThenInclude(p => p.Conversation).FirstOrDefaultAsync(p => p.Id == userId);
            var receiver = await _dataContext.Users.Include(p => p.UserConversations).ThenInclude(p => p.Conversation).FirstOrDefaultAsync(p => p.Id == receiverId);
            var conversation = new Conversation()
            {

            };
            if (user.UserConversations == null)
                user.UserConversations = new List<UserConversation>();
            user.UserConversations.Add(new UserConversation()
            {
                Conversation = conversation
            });

            if (receiver.UserConversations == null)
                receiver.UserConversations = new List<UserConversation>();
            receiver.UserConversations.Add(new UserConversation()
            {
                Conversation = conversation
            });

            await _dataContext.SaveChangesAsync();
            return conversation;
        }

            public async Task<Conversation> GetConversation(int conversationId)
        {
            var conversation = await _dataContext.Conversations
                                                  .Include(p => p.Users)
                                                  .ThenInclude(p => p.User)
                                                  .ThenInclude(p => p.Flat)
                                                  .Include(p => p.Users)
                                                  .ThenInclude(p => p.Conversation)
                                                  .ThenInclude(p => p.Users)
                                                  .ThenInclude(p => p.User)
                                                  .ThenInclude(p => p.Flat)
                                                  //.Include(p => p.Messages)
                                                  .FirstOrDefaultAsync(p => p.Id == conversationId);
            return conversation;
        }

        public async Task<ICollection<UserConversation>> GetConversations(int userId)
        {
            var conversations = _dataContext.UserConversations
                                            .Include(p => p.User)
                                            .ThenInclude(p => p.Flat)
                                            .Include(p => p.Conversation)
                                            .ThenInclude(p => p.Users)
                                            .ThenInclude(p => p.User)
                                            .ThenInclude(p => p.Flat)
                                            .Include(p => p.Conversation)
                                            .ThenInclude(p => p.Messages)
                                            .Where(p => p.User.Id == userId);

            return await conversations.ToListAsync();
        }

        public async Task<ICollection<Message>> GetMessages(int id)
        {
            var conversation = await _dataContext.Conversations
                                                    .Include(p => p.Messages).ThenInclude(p => p.Sender).FirstOrDefaultAsync(p => p.Id == id);
            return conversation.Messages;
        }

        public async Task<Message> SaveMessage(Conversation conversation, SaveMessageDTO message, User user)
        {
            if (conversation.Messages == null)
                conversation.Messages = new List<Message>();

            var newMessage = new Message()
            {
                Date = DateTime.Now,
                Content = message.Message,
                Sender = user
            };

            conversation.Messages.Add(newMessage);
            await _dataContext.SaveChangesAsync();
            return newMessage;

        }
    }
}
