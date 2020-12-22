using HouseCommunity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class ConversationForListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserForConversationDTO> Users { get; set; }
        public ChatType Type { get; set; }
    }
}
