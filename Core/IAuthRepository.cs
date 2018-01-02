using System.Threading.Tasks;
using DatingAPI.Models;

namespace DatingAPI.Core
{
    public interface IAuthRepository
    {
        Task<User> RegisterAsync(User user, string password);

        Task<User> LoginAsync(string username, string password);

        Task<bool> UserExistsAsync(string username);
    }
}