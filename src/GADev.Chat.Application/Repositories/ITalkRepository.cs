using System.Threading.Tasks;
using GADev.Chat.Domain.Models;

namespace GADev.Chat.Application.Repositories
{
    public interface ITalkRepository
    {
        Task<Talk> GetInitialData(int userId);
    }
}