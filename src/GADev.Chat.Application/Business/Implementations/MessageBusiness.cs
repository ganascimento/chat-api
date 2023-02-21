using System.Collections.Generic;
using System.Threading.Tasks;
using GADev.Chat.Application.DataVO.Convertes;
using GADev.Chat.Application.DataVO.VO;
using GADev.Chat.Application.Repositories;
using GADev.Chat.Domain.Models;

namespace GADev.Chat.Application.Business.Implementations
{
    public class MessageBusiness : IMessageBusiness
    {
        private IMessageRepository _messageRepository;
        private MessageConverter messageConverter;

        public MessageBusiness(IMessageRepository messageRepository)
        {
            messageConverter = new MessageConverter();
            _messageRepository = messageRepository;
        }

        public async Task<int?> SendMessage(MessageVO messageVO)
        {
            try {
                return await _messageRepository.Insert(messageConverter.Parse(messageVO));
            } catch {
                return null;
            }
        }

        public async Task ViewMessage(string conversationId) {
            try {
                await _messageRepository.UpdatePending(conversationId);
            } catch {}
        }
    }
}