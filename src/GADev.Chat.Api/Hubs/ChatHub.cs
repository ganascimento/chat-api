using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GADev.Chat.Application.Business;
using GADev.Chat.Application.DataVO.VO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace GADev.Chat.Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageBusiness _messageBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IInvitationBusiness _invitationBusiness;

        public ChatHub(IMessageBusiness messageBusiness, IUserBusiness userBusiness, IInvitationBusiness invitationBusiness)
        {
            _messageBusiness = messageBusiness;
            _userBusiness = userBusiness;
            _invitationBusiness = invitationBusiness;
        }

        public async override Task OnConnectedAsync(){
            try {
                string userIdentifier = Context.ConnectionId;
                int userId = GetUserId();

                await _userBusiness.SetConnectionId(GetUserId(), userIdentifier);

                List<string> connectionIdFriends = await _userBusiness.GetConnectionIdFriendsOnline(userId);

                if (connectionIdFriends != null && connectionIdFriends.Count() > 0) {
                    foreach (var connectionId in connectionIdFriends) {
                        await Clients.Client(connectionId).SendAsync("UserConnected", userId);
                    }
                }
            } catch {}
        }

        public async override Task OnDisconnectedAsync(Exception exception){
            try {
                int userId = GetUserId();

                await _userBusiness.RemoveConnectionId(userId);

                List<string> connectionIdFriends = await _userBusiness.GetConnectionIdFriendsOnline(userId);

                if (connectionIdFriends != null && connectionIdFriends.Count() > 0) {
                    foreach (var connectionId in connectionIdFriends) {
                        await Clients.Client(connectionId).SendAsync("UserDesconnected", userId);
                    }
                }
            }
            catch {}
        }

        public async Task SendMessage(MessageVO message, int? destinationUserId) {
            try {
                if (destinationUserId == null || destinationUserId <= 0) return;

                string destinationConnId = await _userBusiness.GetConnectionId(destinationUserId.Value);

                if (destinationConnId != null){
                    await Clients.Client(destinationConnId).SendAsync("ReceivedMessage", message);
                }

                await _messageBusiness.SendMessage(message);
            }
            catch {}
        }

        public async Task ViewMessage(string conversationId) {
            try {
                if (!string.IsNullOrEmpty(conversationId)) {
                    await _messageBusiness.ViewMessage(conversationId);
                }
            } catch {}
        }

        public async Task SendInvite(InvitationVO invitation){
            try {
                int? idInvite = await _invitationBusiness.SendInvite(invitation);

                if (idInvite != null && idInvite > 0) {
                    string destinationConnId = await _userBusiness.GetConnectionId(invitation.UserReceivedId);

                    if (destinationConnId != null) {
                        var user = await _userBusiness.GetUser(invitation.UserSentId);
                        invitation.Id = idInvite.GetValueOrDefault();
                        await Clients.Client(destinationConnId).SendAsync("ReceivedInvite", invitation, user);
                    }
                }
            }
            catch {}
        }

        public async Task AceptInvite(int invitationId){
            try {
                FriendVO friend = await _invitationBusiness.AceptInvite(invitationId);

                if (friend != null){
                    UserVO userSent = await _userBusiness.GetUser(friend.UserId.GetValueOrDefault());
                    UserVO receivedUser = await _userBusiness.GetUser(friend.FriendId.GetValueOrDefault());

                    await Clients.Caller.SendAsync("NewFriend", friend.ConversationId, userSent);

                    if (userSent != null && !string.IsNullOrEmpty(userSent.ConnectionId)) 
                        await Clients.Client(userSent.ConnectionId).SendAsync("NewFriend", friend.ConversationId, receivedUser);
                }
            }
            catch {}
        }

        public async Task DeclineInvite(int invitationId){
            try {
                await _invitationBusiness.DeclineInvite(invitationId);
            }
            catch {}
        }

        private int GetUserId(){
            var _identity = (ClaimsIdentity)Context.User.Identity;
            return int.Parse(_identity.FindFirst("userId").Value);
        }
    }
}