using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Model
{
    public class Conversation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserConversation> Users { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ChatType Type{ get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
