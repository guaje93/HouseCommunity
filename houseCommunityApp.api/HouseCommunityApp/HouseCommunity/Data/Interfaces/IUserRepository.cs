using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public interface IUserRepository
    {
        Task<UserForInfoDTO> GetUser(int id);
        Task<User> UpdateUserContactData(UserContactData userContactData);
        Task<ICollection<ResidentsForListDTO>> GetResidents();
    }
}
