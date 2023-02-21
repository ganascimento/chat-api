using System.Collections.Generic;
using System.Threading.Tasks;
using GADev.Chat.Application.DataVO.VO;

namespace GADev.Chat.Application.Business
{
    public interface IMessageBusiness
    {
        Task<int?> SendMessage(MessageVO messageVO);
        Task ViewMessage(string conversationId);
    }
}