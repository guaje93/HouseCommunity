using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);
        ICollection<User> GetUsers();
        Task<User> UpdateUser(User user);
        Task<ICollection<User>> GetUsersByRole(UserRole userRole);
        Task<bool> UserExists(string username);
        Task<bool> UserExists(int id);
        Task<User> GetUserByEmail(string email);
    }
}
