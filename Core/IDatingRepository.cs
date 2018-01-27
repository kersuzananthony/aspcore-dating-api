using System.Collections.Generic;
using System.Threading.Tasks;
using DatingAPI.Models;

namespace DatingAPI.Core
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;

        void Remove<T>(T entity) where T : class;

        Task<IEnumerable<User>> GetUsers();

        Task<User> GetUser(int id);

        Task<Photo> GetPhoto(string id);

        Task<Photo> GetMainPhotoForUser(int userId);
    }
}