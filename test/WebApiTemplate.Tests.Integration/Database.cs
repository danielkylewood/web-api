using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Respawn;

namespace WebApiTemplate.Tests.Integration
{
    public static class Database
    {
        private static Checkpoint _checkpoint;

        private static string _connectionString;
        public static string ConnectionString 
        {
             get 
             {
                 if (!string.IsNullOrWhiteSpace(_connectionString)) return _connectionString;

                 var builder = new ConfigurationBuilder()
                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                     .Build();

                 _connectionString = builder.GetSection("Database:DatabaseConnectionString").Value;

                 return _connectionString;
             }
        }

        public static async Task SetUp() 
        {
            using(var connection = await CreateConnection())
            {
                if(_checkpoint == null)
                {
                    _checkpoint = new Checkpoint();
                }
   
                await _checkpoint.Reset(connection);
                await AddApiKey(connection);
            }
        }

        private static async Task AddApiKey(IDbConnection connection)
        {
            var seed = $@"
                        IF NOT EXISTS (SELECT * FROM dbo.ApiAuthentication WHERE ApiKey = '{ApiKeys.Valid}')
	                        INSERT INTO dbo.ApiAuthentication(CreatedDate, ApiKey, KeyHolder) VALUES(GETUTCDATE(), '{ApiKeys.Valid}', 'Test')
                    ";

            await connection.ExecuteAsync(seed);
        }

        private static async Task<SqlConnection> CreateConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            return connection;
        }
    }
}