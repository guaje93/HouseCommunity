﻿using HouseCommunity.Model;

namespace HouseCommunity.DTOs
{
    public class UserForConversationDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole UserRole { get; set; }
        public bool IsBuildingSame { get; internal set; }
    }
}