using System;

namespace GADev.Chat.Domain.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public int UserSentId { get; set; }
        public int UserReceivedId { get; set; }
        public DateTime RequestDate { get; set; }
    }
}