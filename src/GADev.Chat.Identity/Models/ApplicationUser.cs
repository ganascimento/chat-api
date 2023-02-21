namespace GADev.Chat.Identity.Models
{
    public class ApplicationUser : Microsoft.AspNetCore.Identity.IdentityUser<int>
    {        
        public string Name { get; set; }
    }
}