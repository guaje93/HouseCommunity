using HouseCommunity.Model;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public interface IAuthRepository
        {
            Task<User> Register(User user, string password);
            Task<User> LogIn(string userName, string password);
            Task<bool> UserExists(string username);
        }
    }
