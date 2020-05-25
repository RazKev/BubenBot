using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BubenBot.Bot
{
    class Program
    {
        private static async Task Main()
            => await
                Host.CreateDefaultBuilder()
                    .ConfigureAppConfiguration((context, configBuilder) =>
                        configBuilder
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddEnvironmentVariables("Bubenbot:")
                    )
                    .UseSerilog((context, configuration) =>
                        configuration
                            .ReadFrom.Configuration(context.Configuration)
                    )
                    .ConfigureServices((context, serviceCollection) =>
                        serviceCollection
                            .AddDiscordClient()
                            .AddCommandService()
                            .AddStartupService()
                            .AddPrefixService()
                            .AddCommandHandlingService()
                            .AddLogService()
                    )
                    .RunConsoleAsync();
    }
}