using System;

namespace GADev.Chat.Application.DataVO.VO
{
    public class InvitationVO
    {
        public int Id { get; set; }
        public int UserSentId { get; set; }
        public int UserReceivedId { get; set; }
        public DateTime RequestDate { get; set; }
    }
}