using System.Collections.Generic;
using System.Threading.Tasks;
using GADev.Chat.Domain.Models;

namespace GADev.Chat.Application.Repositories
{
    public interface IInvitationRepository
    {
        Task<int> Insert(Invitation invitation);
        Task Remove(Invitation invitation);
        Task<Invitation> GetInvitation(int invitationId);
        Task<List<User>> GetInvitesByUserName(string name, int userId);
    }
}