using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Request;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> LogIn(string userName, string password);
        Task<bool> UserExists(string username);
        Task<User> GetUserForReset(string v);
        Task<User> ResetPassword(string username, string password);
        Task<string> GetUserNameById(int id);
        Task<User> ChangePassword(PasswordChangeRequest passwordChangeRequest);
        Task<User> RegisterUser(UserForRegisterDTO userForRegisterDTO);
    }
}
