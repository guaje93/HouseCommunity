using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Model
{
    public class UserConversation
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Conversation Conversation { get; set; }
    }
}
