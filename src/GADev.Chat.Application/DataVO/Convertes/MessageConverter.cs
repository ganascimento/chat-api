using System.Collections.Generic;
using GADev.Chat.Application.DataVO.Converter;
using GADev.Chat.Application.DataVO.VO;
using GADev.Chat.Domain.Models;
using System.Linq;

namespace GADev.Chat.Application.DataVO.Convertes
{
    public class MessageConverter : IParser<Message, MessageVO>, IParser<MessageVO, Message>
    {
        public Message Parse(MessageVO origin)
        {
            return new Message {
                Id = origin.Id,
                UserSentId = origin.UserSentId,
                Pending = origin.Pending,
                SendDate = origin.SendDate,
                Text = origin.Text,
                ConversationId = origin.ConversationId
            };
        }

        public MessageVO Parse(Message origin)
        {
            return new MessageVO {
                Id = origin.Id,
                UserSentId = origin.UserSentId,
                Pending = origin.Pending,
                SendDate = origin.SendDate,
                Text = origin.Text,
                ConversationId = origin.ConversationId
            };
        }

        public List<Message> ParseList(List<MessageVO> origin)
        {
            return origin.Select(x => Parse(x)).ToList();
        }

        public List<MessageVO> ParseList(List<Message> origin)
        {
            return origin.Select(x => Parse(x)).ToList();
        }
    }
}