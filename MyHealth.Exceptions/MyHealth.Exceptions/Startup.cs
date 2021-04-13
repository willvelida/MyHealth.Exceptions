using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Exceptions;
using MyHealth.Exceptions.Helpers;
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

            builder.Services.AddLogging();
            builder.Services.AddSingleton<IConfiguration>(config);
            builder.Services.AddOptions<FunctionOptions>().Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("FunctionOptions").Bind(settings);
            });

            builder.Services.AddSingleton(sp =>
            {
                IConfiguration configuration = sp.GetService<IConfiguration>();
                return new SendGridClient(configuration.GetConnectionString("SendGridAPIKey"));
            });           
            builder.Services.AddScoped<ISendGridService, SendGridService>();
        }
    }
}
