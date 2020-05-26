using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BubenBot.Services.Prefix;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BubenBot.Services
{
    public class CommandHandlingService : IHostedService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IPrefixService _prefixService;
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;

        public CommandHandlingService(
            DiscordSocketClient client,
            CommandService commandService,
            ILogger<CommandHandlingService> logger,
            IPrefixService prefixService,
            IServiceProvider provider
        )
        {
            _client = client;
            _commandService = commandService;
            _logger = logger;
            _prefixService = prefixService;
            _provider = provider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived -= HandleCommandAsync;
            return Task.CompletedTask;
        }

        private async Task HandleCommandAsync(SocketMessage message)
        {
            if (!(message is SocketUserMessage userMessage))
                return;

            var argPos = 0;
            
            if (!(userMessage.HasStringPrefix(_prefixService.GetPrefix(), ref argPos, StringComparison.Ordinal) || 
                  userMessage.HasMentionPrefix(_client.CurrentUser as IUser, ref argPos)))
                return;

            var context = new SocketCommandContext(_client, userMessage);

            var result = await _commandService.ExecuteAsync(context, argPos, _provider);
            
            if (result.IsSuccess)
                return;
            
            _logger.LogError("Error executing {Command}. Error: {Error}", userMessage.Content.Substring(argPos), result.ErrorReason);
        }
    }
}