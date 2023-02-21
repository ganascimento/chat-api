using System;

namespace GADev.Chat.Domain.Models
{
    public class Message
    {
        public long? Id { get; set; }
        public int UserSentId { get; set; }
        public string ConversationId { get; set; }
        public string Text { get; set; }
        public bool Pending { get; set; }
        public DateTime SendDate { get; set; }
    }
}