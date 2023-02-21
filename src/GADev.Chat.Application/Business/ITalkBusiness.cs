using System.Threading.Tasks;
using GADev.Chat.Application.DataVO.VO;

namespace GADev.Chat.Application.Business
{
    public interface ITalkBusiness
    {
         Task<TalkVO> GetInitialData(int userId);
    }
}