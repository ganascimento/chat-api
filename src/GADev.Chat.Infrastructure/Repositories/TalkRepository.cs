using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GADev.Chat.Application.Repositories;
using GADev.Chat.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GADev.Chat.Infrastructure.Repositories
{
    public class TalkRepository : ITalkRepository
    {
        private readonly string _connectionString;

        public TalkRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Talk> GetInitialData(int userId)
        {
            Talk talk = new Talk();

            var query = @"SP_GET_MESSAGES
                        @UserId = @Id
                        WITH RESULT SETS (
                        (
                            Id BIGINT,
                            UserSentId INT,
                            ConversationId VARCHAR(36),
                            Text VARCHAR(500),
                            Pending BIT,
                            SendDate DATETIME
                        ),(
                            Id INT,
                            Name VARCHAR(16),
                            IsOnline BIT,
                            FileNameAvatar VARCHAR(50),
                            Email NVARCHAR(512)
                        ),(
                            FriendId INT,
                            ConversationId VARCHAR(36)
                        ),(
                            Id INT,
                            UserSentId INT
                        ),(
                            Id INT,
                            Name VARCHAR(16),
                            FileNameAvatar VARCHAR(50)
                        ))
            ";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var multi = (connection.QueryMultipleAsync(query, new { Id = userId }, commandTimeout: int.MaxValue, commandType: CommandType.Text).Result)) {
                    var messages = await multi.ReadAsync<Message>();
                    var users = await multi.ReadAsync<User>();
                    var friends = await multi.ReadAsync<Friend>();
                    var invitations = await multi.ReadAsync<Invitation>();
                    var userInvitation = await multi.ReadAsync<User>();
                    
                    talk.Messages = messages.ToList();
                    talk.Users = users.ToList();
                    talk.Friends = friends.ToList();
                    talk.Invitations = invitations.ToList();
                    talk.UsersInvitations = userInvitation.ToList();
                }
            }

            talk.UserId = userId;
            return talk;
        }
    }
}