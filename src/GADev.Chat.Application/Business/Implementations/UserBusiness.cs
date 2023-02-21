using System.Threading.Tasks;
using GADev.Chat.Application.DataVO.VO;
using GADev.Chat.Application.Repositories;
using System;
using GADev.Chat.Application.DataVO.Convertes;
using GADev.Chat.Application.Util;
using System.Collections.Generic;

namespace GADev.Chat.Application.Business.Implementations
{
    public class UserBusiness : IUserBusiness
    {
        private IUserRepository _userRepository;
        private UserConverter _userConverter;
        private IImageStorage _imageStorage;

        public UserBusiness(IUserRepository userRepository, IImageStorage imageStorage)
        {
            _userRepository = userRepository;
            _userConverter = new UserConverter();
            _imageStorage = imageStorage;
        }

        public async Task SaveAvatar(string avatar, int userId) {
            string nameImage = Guid.NewGuid().ToString() + ".png";

            try {
                _imageStorage.SaveImage(nameImage, avatar);

                bool success = await _userRepository.SetAvatar(userId, nameImage);

                if (!success) _imageStorage.RemoveImage(nameImage);
            } catch {
                _imageStorage.RemoveImage(nameImage);
            }
        }

        public async Task<string> GetConnectionId(int userId)
        {
            try {
                return await _userRepository.GetConnectionId(userId);
            } catch {
                return null;
            }
        }

        public async Task<UserVO> GetUser(int userId)
        {
            try {
                var user = await _userRepository.GetUser(userId);

                if (!string.IsNullOrEmpty(user.FileNameAvatar)){
                    string imageBase64 = _imageStorage.GetImage(user.FileNameAvatar);

                    if (!string.IsNullOrEmpty(imageBase64)) return _userConverter.Parse(user, imageBase64);
                }
                
                return _userConverter.Parse(user);
            } catch {
                return null;
            }
        }

        public async Task<List<string>> GetConnectionIdFriendsOnline(int userId) {
            try {
                return await _userRepository.GetConnectionIdFriendsOnline(userId);
            }
            catch {
                return null;
            }
        }

        public async Task RemoveConnectionId(int userId)
        {
            try {
                await _userRepository.RemoveConnectionId(userId);
            } catch {}
        }

        public async Task SetConnectionId(int userId, string conectionId)
        {
            try {
                await _userRepository.SetConnectionId(userId, conectionId);
            } catch {}
        }
    }
}