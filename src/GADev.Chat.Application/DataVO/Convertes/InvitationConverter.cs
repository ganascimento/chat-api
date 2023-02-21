using System.Collections.Generic;
using GADev.Chat.Application.DataVO.Converter;
using GADev.Chat.Application.DataVO.VO;
using GADev.Chat.Domain.Models;
using System.Linq;

namespace GADev.Chat.Application.DataVO.Convertes
{
    public class InvitationConverter : IParser<Invitation, InvitationVO>, IParser<InvitationVO, Invitation>
    {
        public Invitation Parse(InvitationVO origin)
        {
            return new Invitation {
                Id = origin.Id,
                UserReceivedId = origin.UserReceivedId,
                UserSentId = origin.UserSentId,
                RequestDate = origin.RequestDate
            };
        }

        public InvitationVO Parse(Invitation origin)
        {
            return new InvitationVO {
                Id = origin.Id,
                UserReceivedId = origin.UserReceivedId,
                UserSentId = origin.UserSentId,
                RequestDate = origin.RequestDate
            };
        }

        public List<Invitation> ParseList(List<InvitationVO> origin)
        {
            return origin.Select(x => Parse(x)).ToList();
        }

        public List<InvitationVO> ParseList(List<Invitation> origin)
        {
            return origin.Select(x => Parse(x)).ToList();
        }
    }
}