using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class ReadMessageDTO
    {
        public int UserId { get; set; }
        public int ConversationId { get; set; }
    }
}
