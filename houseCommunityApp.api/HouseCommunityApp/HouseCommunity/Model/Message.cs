using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Model
{
    public class Message : IEntity
    {
        public int Id { get; set; }
        public User Sender { get; set; }
        public Conversation Conversation { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    
    }
}
