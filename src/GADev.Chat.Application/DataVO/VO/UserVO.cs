namespace GADev.Chat.Application.DataVO.VO
{
    public class UserVO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool? IsOnline { get; set; }
        public string ConnectionId { get; set; }
        public string Avatar { get; set; }
    }
}