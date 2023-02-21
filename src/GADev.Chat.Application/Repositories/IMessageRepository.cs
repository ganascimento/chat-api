using System.Collections.Generic;
using System.Threading.Tasks;
using GADev.Chat.Domain.Models;

namespace GADev.Chat.Application.Repositories
{
    public interface IMessageRepository
    {
        Task<int> Insert(Message message);
        Task UpdatePending(string conversationId);
    }
}