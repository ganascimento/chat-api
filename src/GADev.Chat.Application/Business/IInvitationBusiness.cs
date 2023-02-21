using System.Collections.Generic;
using System.Threading.Tasks;
using GADev.Chat.Application.DataVO.VO;

namespace GADev.Chat.Application.Business
{
    public interface IInvitationBusiness
    {
        Task<int?> SendInvite(InvitationVO invitationVO);
        Task<FriendVO> AceptInvite(int invitationId);
        Task DeclineInvite(int invitationId);
        Task<List<UserVO>> GetInvitesByUserName(string name, int userId);
    }
}