using System.Collections.Generic;
using System.Threading.Tasks;
using GADev.Chat.Application.DataVO.VO;

namespace GADev.Chat.Application.Business
{
    public interface IUserBusiness
    {
        Task<string> GetConnectionId(int userId);
        Task RemoveConnectionId(int userId);
        Task SetConnectionId(int userId, string conectionId);
        Task<UserVO> GetUser(int userId);
        Task SaveAvatar(string avatar, int userId);
        Task<List<string>> GetConnectionIdFriendsOnline(int userId);
    }
}