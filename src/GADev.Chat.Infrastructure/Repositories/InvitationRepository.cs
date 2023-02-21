using System.Threading.Tasks;
using System.Collections.Generic;
using Dommel;
using GADev.Chat.Application.Repositories;
using GADev.Chat.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text;
using Dapper;
using System.Data;
using System.Linq;

namespace GADev.Chat.Infrastructure.Repositories
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly string _connectionString;

        public InvitationRepository(IConfiguration configuration)
        {
            RegisterMappings.Register();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Invitation> GetInvitation(int invitationId)
        {
            Invitation invitation = new Invitation();

            StringBuilder commandText = new StringBuilder();
            commandText.Append("SELECT");
            commandText.Append(" [Id],");
            commandText.Append(" [UserSentId],");
            commandText.Append(" [UserReceivedId],");
            commandText.Append(" [RequestDate]");
            commandText.Append(" FROM [TB_Invitation]");
            commandText.Append(" WHERE [Id] = @InvitationId");

            using (var connection = new SqlConnection(_connectionString)) {
                invitation = await connection.QueryFirstOrDefaultAsync<Invitation>(commandText.ToString(), new { InvitationId = invitationId }, commandTimeout: int.MaxValue, commandType: CommandType.Text);
            }

            return invitation;
        }

        public async Task<int> Insert(Invitation invitation)
        {
            int invitationId;
            using (var connection = new SqlConnection(_connectionString)){
                invitationId = int.Parse((await connection.InsertAsync(invitation)).ToString());
            }

            return invitationId;
        }

        public async Task Remove(Invitation invitation)
        {
            using (var connection = new SqlConnection(_connectionString)){
                await connection.DeleteAsync(invitation);
            }
        }

        public async Task<List<User>> GetInvitesByUserName(string name, int userId)
        {
            List<User> users;
            var query = @"SELECT	TOP 10
                        [Id],
                        [Name],
                        [FileNameAvatar]
                        FROM	[TB_ApplicationUser] WITH (NOLOCK)
                        WHERE	(
                                [Name] LIKE @NameEmail + '%' OR
                                [Email] LIKE @NameEmail + '%'
                            ) AND
                            [Id] <> @UserId AND 
                            [Id] NOT IN (
                                SELECT	[UserReceivedId] 
                                FROM	[TB_Invitation]
                                WHERE	[UserSentId] = @UserId
                            ) AND 
                            [Id] NOT IN (
                                SELECT	[UserSentId]
                                FROM	[TB_Invitation]
                                WHERE	[UserReceivedId] = @UserId
                            ) AND
                            [Id] NOT IN (
                                SELECT	[UserId]
                                FROM	[TB_Friend]
                                WHERE	[FriendId] = @UserId
                            ) AND
                            [Id] NOT IN (
                                SELECT	[FriendId]
                                FROM	[TB_Friend]
                                WHERE	[UserId] = @UserId
                            )
            ";

            using (var connection = new SqlConnection(_connectionString)) {
                users = (await connection.QueryAsync<User>(query, new { NameEmail = name, UserId = userId }, commandTimeout: int.MaxValue, commandType: CommandType.Text)).ToList();
            }

            return users;
        }
    }
}