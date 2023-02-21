using System.Collections.Generic;
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
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<string> GetConnectionId(int userId)
        {
            string connectionId;

            var query = @"SELECT   [ConnectionId]
                        FROM    [TB_ApplicationUser]
                        WHERE   [Id] = @UserId
            ";

            using (var connection = new SqlConnection(_connectionString)){
                connectionId = await connection.QueryFirstAsync<string>(query, new { UserId = userId }, commandTimeout: int.MaxValue, commandType: CommandType.Text);
            }

            return connectionId;
        }

        public async Task<User> GetUser(int userId)
        {
            User user;

            var query = @"SELECT  [Id],
                                [Email],
                                [Name],
                                [IsOnline],
                                [ConnectionId],
                                [FileNameAvatar]
                        FROM    [TB_ApplicationUser]
                        WHERE   [Id] = @UserId
            ";

            using (var connection = new SqlConnection(_connectionString)){
                user = await connection.QueryFirstAsync<User>(query, new { UserId = userId }, commandTimeout: int.MaxValue, commandType: CommandType.Text);
            }

            return user;
        }

        public async Task<List<string>> GetConnectionIdFriendsOnline(int userId) {
            List<string> connectionsIds = null;

            var query = @"WITH FRIENDS_USER           
                        (
                            [UserId]
                        )
                        AS
                        (
                            SELECT	[UserId]
                            FROM	[TB_Friend]
                            WHERE	[FriendId] = @UserId
                        ),
                        FRIENDS_FRIEND
                        (
                            [FriendId]
                        )
                        AS
                        (
                            SELECT	[FriendId]
                            FROM	[TB_Friend]
                            WHERE	[UserId] = @UserId
                        )
                        SELECT	[TB_ApplicationUser].[ConnectionId]
                        FROM	[TB_ApplicationUser]
                        LEFT	JOIN FRIENDS_USER
                        ON		FRIENDS_USER.[UserId] = [TB_ApplicationUser].[Id]
                        LEFT	JOIN FRIENDS_FRIEND
                        ON		FRIENDS_FRIEND.[FriendId] = [TB_ApplicationUser].[Id]
                        WHERE	[TB_ApplicationUser].[ConnectionId] IS NOT NULL AND
                                (
                                    FRIENDS_USER.[UserId] IS NOT NULL OR
                                    FRIENDS_FRIEND.[FriendId] IS NOT NULL
                                )
            ";

            using (var connection = new SqlConnection(_connectionString)){
                connectionsIds = (await connection.QueryAsync<string>(query, new { UserId = userId }, commandTimeout: int.MaxValue, commandType: CommandType.Text)).ToList();
            }

            return connectionsIds;
        }

        public async Task RemoveConnectionId(int userId)
        {
            var query = @"UPDATE   [TB_ApplicationUser]
                        SET     [ConnectionId] = NULL,
                                [IsOnline] = 0
                        WHERE   [Id] = @UserId
            ";

            using (var connection = new SqlConnection(_connectionString)){
                await connection.ExecuteAsync(query, new { UserId = userId }, commandTimeout: int.MaxValue, commandType: CommandType.Text);
            }
        }

        public async Task SetConnectionId(int userId, string conectionId)
        {
            var query = @"UPDATE   [TB_ApplicationUser]
                        SET     [ConnectionId] = @ConnectionId,
                                [IsOnline] = 1
                        WHERE   [Id] = @UserId
            ";

            using (var connection = new SqlConnection(_connectionString)){
                await connection.ExecuteAsync(query, new { UserId = userId, ConnectionId = conectionId }, commandTimeout: int.MaxValue, commandType: CommandType.Text);
            }
        }

        public async Task<bool> SetAvatar(int userId, string fileNameAvatar)
        {
            var query = @"UPDATE   [TB_ApplicationUser]
                        SET     [FileNameAvatar] = @FileNameAvatar
                        WHERE   [Id] = @UserId
            ";

            using (var connection = new SqlConnection(_connectionString)){
                int rows = await connection.ExecuteAsync(query, new { UserId = userId, FileNameAvatar = fileNameAvatar }, commandTimeout: int.MaxValue, commandType: CommandType.Text);

                if (rows > 0) return true;

                return false;
            }
        }
    }
}