using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public interface IUserRepository
    {
        Task<User> GetUser(int id);
        ICollection<User> GetUsers();
        Task<User> UpdateUserDefinedData(UserDefinedData userContactData);
        Task<ICollection<User>> GetUsersWithRole(UserRole userRole);
    }
}
