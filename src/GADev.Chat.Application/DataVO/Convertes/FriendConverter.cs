using System.Collections.Generic;
using GADev.Chat.Application.DataVO.Converter;
using GADev.Chat.Application.DataVO.VO;
using GADev.Chat.Domain.Models;
using System.Linq;

namespace GADev.Chat.Application.DataVO.Convertes
{
    public class FriendConverter : IParser<Friend, FriendVO>, IParser<FriendVO, Friend>
    {
        public Friend Parse(FriendVO origin)
        {
            return new Friend {
                UserId = origin.UserId,
                FriendId = origin.FriendId,
                ConversationId = origin.ConversationId
            };
        }

        public FriendVO Parse(Friend origin)
        {
            return new FriendVO {
                UserId = origin.UserId,
                FriendId = origin.FriendId,
                ConversationId = origin.ConversationId
            };
        }

        public List<Friend> ParseList(List<FriendVO> origin)
        {
            return origin.Select(x => Parse(x)).ToList();
        }

        public List<FriendVO> ParseList(List<Friend> origin)
        {
            return origin.Select(x => Parse(x)).ToList();
        }
    }
}