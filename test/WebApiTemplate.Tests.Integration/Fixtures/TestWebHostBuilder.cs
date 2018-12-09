using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Serilog;
using WebApiTemplate.Domain.Configuration;
using WebApiTemplate.Domain.Services;

namespace WebApiTemplate.Tests.Integration.Fixtures
{
    public static class TestWebHostBuilder
    {
        public static IWebHostBuilder BuildTestWebHostForStartUp<TStartUp>() where TStartUp : class
        {
            var logger = new Mock<ILogger>();

            logger.Setup(x => x.ForContext<object>()).Returns(logger.Object);
            logger.Setup(x => x.ForContext(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>())).Returns(logger.Object);

            var builder = new WebHostBuilder()
                .UseEnvironment("Testing")
                .ConfigureServices(x =>
                {
                    x.AddTransient<ICustomersService, CustomersService>();
                    x.AddSingleton(logger.Object);
                    x.Configure<DatabaseOptions>(options => {
                        options.DatabaseConnectionString = Database.ConnectionString;
                    });
                })
                .UseStartup<TStartUp>();

            return builder;
        }
    }
}