using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GADev.Chat.Application.DataVO.Convertes;
using GADev.Chat.Application.DataVO.VO;
using GADev.Chat.Application.Repositories;
using GADev.Chat.Domain.Models;

namespace GADev.Chat.Application.Business.Implementations
{
    public class InvitationBusiness : IInvitationBusiness
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IFriendRepository _friendRepository;
        private readonly InvitationConverter _invitationConverter;
        private readonly FriendConverter _friendConverter;
        private readonly UserConverter _userConverter;

        public InvitationBusiness(IInvitationRepository invitationRepository, IFriendRepository friendRepository)
        {
            _invitationConverter = new InvitationConverter();
            _userConverter = new UserConverter();
            _invitationRepository = invitationRepository;
            _friendRepository = friendRepository;
            _friendConverter = new FriendConverter();
        }

        public async Task<FriendVO> AceptInvite(int invitationId)
        {
            try {
                Invitation invitation = await _invitationRepository.GetInvitation(invitationId);

                Friend friend = new Friend {
                    UserId = invitation.UserSentId,
                    FriendId = invitation.UserReceivedId,
                    ConversationId = Guid.NewGuid().ToString()
                };

                await _friendRepository.Insert(friend);
                await _invitationRepository.Remove(invitation);
                

                return _friendConverter.Parse(friend);
            }
            catch {
                return null;
            }
        }

        public async Task DeclineInvite(int invitationId)
        {
            try {
                Invitation invitation = await _invitationRepository.GetInvitation(invitationId);

                await _invitationRepository.Remove(invitation);
            }
            catch {}
        }

        public async Task<int?> SendInvite(InvitationVO invitationVO)
        {
            try {
                return await _invitationRepository.Insert(_invitationConverter.Parse(invitationVO));
            }
            catch {
                return null;
            }
        }

        public async Task<List<UserVO>> GetInvitesByUserName(string name, int userId)
        {
            try {
                return _userConverter.ParseList(await _invitationRepository.GetInvitesByUserName(name, userId));
            } catch {
                return null;
            }
        }
    }
}