using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using GADev.Chat.Infrastructure.Mappings;

namespace GADev.Chat.Infrastructure
{
    public static class RegisterMappings
    {
        public static void Register() {
            if (FluentMapper.EntityMaps.IsEmpty) {
                FluentMapper.Initialize(config => {
                    config.AddMap(new FriendMap());
                    config.AddMap(new InvitationMap());
                    config.AddMap(new MessageMap());
                    config.ForDommel();
                });
            }
        }
    }
}