using System.Collections.Generic;
using System.Threading.Tasks;
using GADev.Chat.Domain.Models;

namespace GADev.Chat.Application.Repositories
{
    public interface IFriendRepository
    {
        Task Insert(Friend friend);
        Task Remove(Friend friend);
    }
}