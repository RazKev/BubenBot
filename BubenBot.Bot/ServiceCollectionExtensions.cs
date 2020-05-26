using BubenBot.Services;
using BubenBot.Services.Prefix;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace BubenBot.Bot
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscordClient(this IServiceCollection @this)
        {
            return @this.AddSingleton(
                new DiscordSocketClient(
                    new DiscordSocketConfig()
                    {
                        LogLevel = LogSeverity.Verbose
                    }
                )
            );
        }

        public static IServiceCollection AddCommandService(this IServiceCollection @this)
        {
            return @this.AddSingleton(
                new CommandService(
                    new CommandServiceConfig()
                    {
                        LogLevel = LogSeverity.Verbose
                    }));
        }

        public static IServiceCollection AddStartupService(this IServiceCollection @this)
            => @this.AddHostedService<StartupService>();

        public static IServiceCollection AddPrefixService(this IServiceCollection @this)
            => @this.AddSingleton<IPrefixService, PrefixService>();

        public static IServiceCollection AddCommandHandlingService(this IServiceCollection @this)
            => @this.AddHostedService<CommandHandlingService>();

        public static IServiceCollection AddLogService(this IServiceCollection @this)
            => @this.AddHostedService<LogService>();
    }
}