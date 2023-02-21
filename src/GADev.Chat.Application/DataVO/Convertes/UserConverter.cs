using System.Collections.Generic;
using GADev.Chat.Application.DataVO.Converter;
using GADev.Chat.Application.DataVO.VO;
using GADev.Chat.Domain.Models;
using System.Linq;

namespace GADev.Chat.Application.DataVO.Convertes
{
    public class UserConverter : IParser<User, UserVO>, IParser<UserVO, User>
    {
        public User Parse(UserVO origin)
        {
            return new User {
                ConnectionId = origin.ConnectionId,
                Email = origin.Email,
                Name = origin.Name,
                Id = origin.Id,
                IsOnline = origin.IsOnline
            };
        }

        public UserVO Parse(User origin)
        {
            return new UserVO {
                ConnectionId = origin.ConnectionId,
                Email = origin.Email,
                Name = origin.Name,
                Id = origin.Id,
                IsOnline = origin.IsOnline
            };
        }

        public UserVO Parse(User origin, string fileAvatar)
        {
            return new UserVO {
                ConnectionId = origin.ConnectionId,
                Email = origin.Email,
                Name = origin.Name,
                Id = origin.Id,
                IsOnline = origin.IsOnline,
                Avatar = fileAvatar
            };
        }

        public List<User> ParseList(List<UserVO> origin)
        {
            return origin.Select(x => Parse(x)).ToList();
        }

        public List<UserVO> ParseList(List<User> origin)
        {
            return origin.Select(x => Parse(x)).ToList();
        }
    }
}