using HouseCommunity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public interface IUserRepository
    {
        Task<UserForInfoDTO> GetUser(int id);
    }
}
