using Dapper.FluentMap.Dommel.Mapping;
using GADev.Chat.Domain.Models;

namespace GADev.Chat.Infrastructure.Mappings
{
    public class MessageMap : DommelEntityMap<Message>
    {
        public MessageMap()
        {
            ToTable("TB_Message");
            Map(p => p.Id).IsKey().IsIdentity();
        }
    }
}