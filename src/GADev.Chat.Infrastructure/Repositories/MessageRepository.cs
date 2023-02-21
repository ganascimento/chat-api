using System.Text;
using System.Threading.Tasks;
using Dommel;
using Dapper;
using GADev.Chat.Application.Repositories;
using GADev.Chat.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace GADev.Chat.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly string _connectionString;

        public MessageRepository(IConfiguration configuration)
        {
            RegisterMappings.Register();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> Insert(Message message)
        {
            int id;
            
            using (var connection = new SqlConnection(_connectionString)) {
                id = (int)await connection.InsertAsync(message);
            }

            return id;
        }

        public async Task UpdatePending(string conversationId) {
            var query = @"UPDATE  [TB_Message]
                          SET [Pending] = 0
                          WHERE [ConversationId] = @ConversationId
            ";

            using (var connection = new SqlConnection(_connectionString)) {
                await connection.ExecuteAsync(query, new { ConversationId = conversationId }, commandTimeout: int.MaxValue, commandType: CommandType.Text);
            }
        }
    }
}