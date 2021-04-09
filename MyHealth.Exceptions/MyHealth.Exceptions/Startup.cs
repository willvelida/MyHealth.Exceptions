using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Exceptions;
using MyHealth.Exceptions.Services;
using SendGrid;
using System.IO;

[assembly: FunctionsStartup(typeof(Startup))]
namespace MyHealth.Exceptions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton<IConfiguration>(config);
            builder.Services.AddLogging();

            builder.Services.AddSingleton((s) => new SendGridClient(config["SendGridAPIKey"]));
            builder.Services.AddScoped<ISendGridService, SendGridService>();
        }
    }
}
