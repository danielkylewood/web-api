using System;
using Dapper;
using System.Threading.Tasks;

namespace WebApiTemplate.Domain.Repositories
{
    public class ApiAuthenticationRepository : BaseRepository
    {
        public ApiAuthenticationRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<int> GetNumberOfMatchesByApiKey(Guid apiKey)
        {
            using (var connection = await CreateConnection())
            {
                var parameters = new
                {
                    apiKey
                };

                const string sql = @" 
                    SELECT COUNT(1)
                    FROM dbo.ApiAuthentication
                    WHERE ApiKey = @apiKey;
                ";

                var numberOfMatches = await connection.QueryFirstOrDefaultAsync<int>(sql, parameters);

                return numberOfMatches;
            }
        }
    }
}
