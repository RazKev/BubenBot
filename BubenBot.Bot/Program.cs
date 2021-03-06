﻿using System;
using System.IO;
using System.Threading.Tasks;
using BubenBot.Common.Messaging;
using BubenBot.Data;
using BubenBot.Services;
using BubenBot.Services.Core.Notifications;
using BubenBot.Services.Prefix;
using BubenBot.Services.Tag;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BubenBot.Bot
{
    internal static class Program
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
                .AddDbContext<BotContext>(builder =>
                    builder
                        .UseNpgsql(context.Configuration.GetConnectionString("BotContext"))
                        .UseSnakeCaseNamingConvention()
                )
                .AddSingleton<IMessageDispatcher, MessageDispatcher>()
                .AddScoped<IPrefixService, ConfigurationPrefixService>()
                .AddScoped<ITagService, TagService>()
                .AddScoped<INotificationHandler<MessageReceivedNotification>, CommandHandlingService>()
                .AddScoped<INotificationHandler<MessageReceivedNotification>, TagHandlingService>()
                .AddHostedService<StartupService>()
                .AddHostedService<NotificationSubscriberService>()
                .AddHostedService<LogService>();
        }
    }
}