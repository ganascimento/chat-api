using System;

namespace GADev.Chat.Domain.Models
{
    public class Friend
    {
        public int? UserId { get; set; }
        public int? FriendId { get; set; }
        public string ConversationId { get; set; }
    }
}