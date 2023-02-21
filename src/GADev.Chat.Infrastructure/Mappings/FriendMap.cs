using Dapper.FluentMap.Dommel.Mapping;
using GADev.Chat.Domain.Models;

namespace GADev.Chat.Infrastructure.Mappings
{
    public class FriendMap  : DommelEntityMap<Friend>
    {
        public FriendMap()
        {
            ToTable("TB_Friend");
            Map(p => p.UserId).IsKey();
        }
    }
}