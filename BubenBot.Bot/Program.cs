using System;
using System.IO;
using System.Threading.Tasks;
using BubenBot.Services;
using BubenBot.Services.Prefix;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                    .ConfigureServices(ConfigureServices)
                    .RunConsoleAsync();

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton(
                    new DiscordSocketClient(
                        new DiscordSocketConfig()
                        {
                            LogLevel = LogSeverity.Verbose
                        }
                    )
                )
                .AddSingleton(
                    new CommandService(
                        new CommandServiceConfig()
                        {
                            LogLevel = LogSeverity.Verbose
                        }
                    )
                )
                .AddSingleton<IPrefixService, PrefixService>()
                .AddHostedService<StartupService>()
                .AddHostedService<CommandHandlingService>()
                .AddHostedService<LogService>();
        }
    }
}