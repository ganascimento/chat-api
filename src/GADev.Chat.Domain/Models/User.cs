namespace GADev.Chat.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool? IsOnline { get; set; }
        public string ConnectionId { get; set; }
        public string FileNameAvatar { get; set; }
    }
}