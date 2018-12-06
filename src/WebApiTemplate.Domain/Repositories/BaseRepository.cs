using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebApiTemplate.Domain.Repositories
{
    public abstract class BaseRepository
    {
        private string ConnectionString { get; set; }

        protected BaseRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected async Task<IDbConnection> CreateConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
