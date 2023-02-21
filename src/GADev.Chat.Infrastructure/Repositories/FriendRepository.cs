using System.Threading.Tasks;
using Dommel;
using GADev.Chat.Application.Repositories;
using GADev.Chat.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GADev.Chat.Infrastructure.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private readonly string _connectionString;

        public FriendRepository(IConfiguration configuration)
        {
            RegisterMappings.Register();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Insert(Friend friend)
        {
            using (var connection = new SqlConnection(_connectionString)){
                await connection.InsertAsync(friend);
            }
        }

        public async Task Remove(Friend friend)
        {
            using (var connection = new SqlConnection(_connectionString)){
                await connection.DeleteAsync(friend);
            }
        }
    }
}