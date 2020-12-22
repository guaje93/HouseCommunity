using HouseCommunity.DTOs;
using HouseCommunity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data.Interfaces
{
    public interface IChatRepository
    {
        Task<ICollection<UserConversation>> GetConversations(int userId);
        Task<ICollection<Message>> GetMessages(int id);
        Task<Conversation> GetConversation(int conversationId);
        Task<Message> SaveMessage(Conversation conversation, SaveMessageDTO message, User userFromRepo);
        Task<Conversation> CreateConversation(int userId, int receiverId);
    }
}
