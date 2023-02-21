using System.Collections.Generic;
using System.Threading.Tasks;
using GADev.Chat.Domain.Models;

namespace GADev.Chat.Application.Repositories
{
    public interface IUserRepository
    {
        Task<string> GetConnectionId(int userId);
        Task<User> GetUser(int userId);
        Task RemoveConnectionId(int userId);
        Task SetConnectionId(int userId, string conectionId);
        Task<bool> SetAvatar(int userId, string fileNameAvatar);
        Task<List<string>> GetConnectionIdFriendsOnline(int userId);
    }
}