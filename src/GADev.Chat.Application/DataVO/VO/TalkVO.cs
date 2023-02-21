using System;
using System.Collections.Generic;

namespace GADev.Chat.Application.DataVO.VO
{
    public class TalkVO
    {
        public List<TalkFriend> Friends { get; set; }
        public List<TalkChat> Chats { get; set; }
        public List<TalkInvitation> Invitations { get; set; }
        public List<TalkUserInvitation> UsersInvitations { get; set; }
    }

    public class TalkFriend {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool? IsOnline { get; set; }
        public string Avatar { get; set; }
    }

    public class TalkChat {
        public string ConversationId { get; set; }
        public List<TalkMessage> Messages { get; set; }
        public int? FriendId { get; set; }
    }

    public class TalkMessage {
        public long? Id { get; set; }
        public int? FriendId { get; set; }
        public bool EhSent { get; set; }
        public string Text { get; set; }
        public bool Pending { get; set; }
        public DateTime SendDate { get; set; }
    }

    public class TalkInvitation {
        public int Id { get; set; }
        public int UserSentId { get; set; }
    }

    public class TalkUserInvitation {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
    }
}