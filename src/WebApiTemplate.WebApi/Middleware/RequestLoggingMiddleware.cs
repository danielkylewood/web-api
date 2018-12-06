using System;
using Serilog;
using System.Linq;
using Serilog.Events;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApiTemplate.WebApi.Middleware
{
    public class RequestLoggingMiddleware
    {
        private const string _messageTemplate = "HTTP {RequestMethod} {RequestPath} requested with content body length {ContentBodyLength} and responded {StatusCode} in {Elapsed:0.0000} ms";

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLoggingMiddleware(ILogger logger, RequestDelegate next)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger.ForContext<RequestLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));
            var start = Stopwatch.GetTimestamp();

            try
            {
                await _next(httpContext);
                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
                var statusCode = httpContext.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;
                var log = level == LogEventLevel.Error ? LogForErrorContext(httpContext) : _logger;
                log.Write(level, _messageTemplate, httpContext.Request.Method, httpContext.Request.Path, httpContext.Request.ContentLength ?? 0, statusCode, elapsedMs);
            }
            catch (Exception ex)
            {
                LogForErrorContext(httpContext).Error(ex, _messageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500, Stopwatch.GetTimestamp());
            }
        }

        private ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = _logger
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            if (request.HasFormContentType)
            {
                result = result.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));
            }

            return result;
        }

        private static double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
    }
}
