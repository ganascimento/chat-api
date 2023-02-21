using Dapper.FluentMap.Dommel.Mapping;
using GADev.Chat.Domain.Models;

namespace GADev.Chat.Infrastructure.Mappings
{
    public class InvitationMap  : DommelEntityMap<Invitation>
    {
        public InvitationMap()
        {
            ToTable("TB_Invitation");
            Map(p => p.Id).IsKey().IsIdentity();
        }
    }
}