using Serilog;
using System.Net;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApiTemplate.Domain.Configuration;
using WebApiTemplate.Domain.Services;
using WebApiTemplate.WebApi.Filters;
using WebApiTemplate.WebApi.Middleware;
using ILogger = Serilog.ILogger;

namespace WebApiTemplate.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("HostName", GetHostName())
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                .CreateLogger();
        }

        public ILogger Logger { get; }
        public IConfiguration Configuration { get; }
        private IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DatabaseOptions>(Configuration.GetSection("Database"));
            
            services.AddSingleton(Logger);
            services.AddTransient<ICustomersService, CustomersService>();

            services
                .AddMvcCore(x =>
                {
                    x.Filters.Add(new ValidateModelStateFilter());
                })
                .AddJsonFormatters(x =>
                {
                    x.NullValueHandling = NullValueHandling.Ignore;
                    x.ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                })
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog(Logger);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ApiKeyAuthenticationMiddleware>();
            app.UseMvc();
        }

        private static string GetHostName()
        {
            var hostName = string.Empty;

            try
            {
                hostName = Dns.GetHostName();
            }
            catch
            {
                // ignored
            }

            return hostName;
        }
    }
}
