using System.Collections.Generic;
using GADev.Chat.Application.DataVO.Converter;
using GADev.Chat.Application.DataVO.VO;
using GADev.Chat.Domain.Models;
using System.Linq;
using GADev.Chat.Application.Util;

namespace GADev.Chat.Application.DataVO.Convertes
{
    public class TalkConverter : IParser<TalkVO, Talk>
    {
        private IImageStorage _imageStorage;

        public TalkConverter(IImageStorage imageStorage)
        {
            _imageStorage = imageStorage;            
        }

        public TalkVO Parse(Talk origin)
        {
            var friends = new List<DataVO.VO.TalkFriend>();
            var chats = new List<DataVO.VO.TalkChat>();
            var invitations = new List<DataVO.VO.TalkInvitation>();
            var userInvitations = new List<DataVO.VO.TalkUserInvitation>();

            foreach(var item in origin.Users){
                friends.Add(new DataVO.VO.TalkFriend {
                    Email = item.Email,
                    Name = item.Name,
                    Id = item.Id,
                    IsOnline = item.IsOnline,
                    Avatar = _imageStorage.GetImage(item.FileNameAvatar)
                });
            }
            
            var convIds = origin.Friends.Select(x => x.ConversationId).ToList();

            foreach (var item in convIds){
                int? friend = origin.Friends.First(x => x.ConversationId == item).FriendId;

                chats.Add(new DataVO.VO.TalkChat{
                    ConversationId = item,
                    FriendId = friend
                });
            }

            foreach(var chat in chats){
                var listMessages = new List<DataVO.VO.TalkMessage>();

                foreach(var message in origin.Messages){
                    if (chat.ConversationId == message.ConversationId) {
                        listMessages.Add(new DataVO.VO.TalkMessage{
                            Id = message.Id,
                            Pending = message.Pending,
                            Text = message.Text,
                            SendDate = message.SendDate,
                            EhSent = origin.UserId == message.UserSentId
                        });
                    }
                }

                chat.Messages = listMessages;
            }

            foreach (var invite in origin.Invitations) {
                invitations.Add(new TalkInvitation {
                    Id = invite.Id,
                    UserSentId = invite.UserSentId
                });
            }

            foreach (var userInvitation in origin.UsersInvitations) {
                userInvitations.Add(new TalkUserInvitation {
                    Id = userInvitation.Id,
                    Avatar = _imageStorage.GetImage(userInvitation.FileNameAvatar),
                    Name = userInvitation.Name
                });
            }

            return new TalkVO {
                Friends = friends,
                Chats = chats,
                Invitations = invitations,
                UsersInvitations = userInvitations
            };
        }

        public List<TalkVO> ParseList(List<Talk> origin)
        {
            return origin.Select(x => Parse(x)).ToList();
        }
    }
}