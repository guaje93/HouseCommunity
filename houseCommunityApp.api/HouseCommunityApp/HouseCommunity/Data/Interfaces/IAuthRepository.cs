using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Request;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public interface IAuthRepository
    {
        Task<User> LogIn(string userName, string password);
        Task<User> RegisterUser(UserForRegisterDTO userForRegisterDTO);
        Task<User> ResetPassword(string username, string password);
        Task<User> ChangePassword(PasswordChangeRequest passwordChangeRequest);
        Task<bool> HasAdministrationRights(int userId);
        Task<bool> HasHouseManagerRights(int userId);

    }
}
