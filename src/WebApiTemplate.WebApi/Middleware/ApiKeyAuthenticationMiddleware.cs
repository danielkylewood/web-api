using System;
using Serilog;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WebApiTemplate.Domain.Configuration;
using WebApiTemplate.Domain.Repositories;

namespace WebApiTemplate.WebApi.Middleware
{
    public class ApiKeyAuthenticationMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly ApiAuthenticationRepository _apiAuthenticationRepository;

        public ApiKeyAuthenticationMiddleware(RequestDelegate next, IOptions<DatabaseOptions> databaseOptions, ILogger logger)
        {
            _next = next;
            _apiAuthenticationRepository = new ApiAuthenticationRepository(databaseOptions.Value.DatabaseConnectionString);
            _logger = logger.ForContext<ApiKeyAuthenticationMiddleware>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var apiKey = httpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                if (!Guid.TryParse(apiKey, out var apiKeyGuid))
                {
                    _logger.Warning($"Attempted call to method with API key: {apiKey}.");
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                var numberOfMatches = await _apiAuthenticationRepository.GetNumberOfMatchesByApiKey(apiKeyGuid);

                if (numberOfMatches != 1)
                {
                    _logger.Warning($"There were {numberOfMatches} matches found for API key: {apiKey}");
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                await _next(httpContext);
                return;
            }

            _logger.Warning($"Attempted call to method with API key: {apiKey}.");
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
