﻿using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BubenBot.Common.Messaging;
using BubenBot.Services.Core.Notifications;
using BubenBot.Services.Prefix;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BubenBot.Services
{
    public class CommandHandlingService : INotificationHandler<MessageReceivedNotification>
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
            IServiceProvider provider)
        {
            _client = client;
            _commandService = commandService;
            _logger = logger;
            _prefixService = prefixService;
            _provider = provider;
        }

        public async Task HandleNotificationAsync(MessageReceivedNotification notification)
        {
            var message = notification.Message;
            
            if (!(message is SocketUserMessage userMessage))
                return;
            if (!(message.Channel is IGuildChannel))
                return;

            var argPos = 0;

            var guildId = ((IGuildChannel) userMessage.Channel).Guild.Id;
            if (!(userMessage.HasStringPrefix(await _prefixService.GetCommandPrefixAsync(guildId), ref argPos) ||
                  userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos)))
                return;

            var context = new SocketCommandContext(_client, userMessage);

            var result = await _commandService.ExecuteAsync(context, argPos, _provider);

            if (result.IsSuccess)
                return;

            _logger.LogError("Error executing {Command}. Error: {Error}", userMessage.Content.Substring(argPos),
                result.ErrorReason);
        }
    }
}