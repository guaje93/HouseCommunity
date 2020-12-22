using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class SaveMessageDTO
    {
        public int ConversationId { get; set; }
        public int UserId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
    }
}
