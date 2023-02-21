using System;
using System.Collections.Generic;

namespace GADev.Chat.Domain.Models
{
    public class Talk
    {
        public List<Message> Messages { get; set; }
        public List<User> Users { get; set; }
        public List<Friend> Friends { get; set; }
        public List<Invitation> Invitations { get; set; }
        public List<User> UsersInvitations { get; set; }
        public int UserId { get; set; }
    }
}