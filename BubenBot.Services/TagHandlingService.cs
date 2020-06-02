using System;
using System.Threading;
using System.Threading.Tasks;
using BubenBot.Services.Prefix;
using BubenBot.Services.Tag;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BubenBot.Services
{
    public class TagHandlingService : IHostedService
    {
        private const string UnicodeQuestionMark = "\u2753"; // '❓'

        private readonly DiscordSocketClient _client;
        private readonly ILogger _logger;
        private readonly IServiceProvider _provider;

        public TagHandlingService(
            DiscordSocketClient client,
            ILogger<TagHandlingService> logger,
            IServiceProvider provider)
        {
            _client = client;
            _logger = logger;
            _provider = provider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += HandleTagAsync;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived -= HandleTagAsync;
            return Task.CompletedTask;
        }

        private async Task HandleTagAsync(SocketMessage message)
        {
            if (message.Author.IsBot)
                return;
            if (!(message is SocketUserMessage userMessage))
                return;
            if (!(message.Channel is IGuildChannel channel))
                return;
            
            using var scope = _provider.CreateScope();
            var prefixService = scope.ServiceProvider.GetRequiredService<IPrefixService>();
            var tagService = scope.ServiceProvider.GetRequiredService<ITagService>();

            var guildId = channel.GuildId;
            var prefix = await prefixService.GetTagPrefixAsync(guildId);

            if (message.Content.Substring(0, prefix.Length) != prefix)
                return;

            var tagName = userMessage.Content.Substring(prefix.Length);

            if (!await tagService.TagExistsAsync(channel.GuildId, tagName))
                await message.AddReactionAsync(new Emoji(UnicodeQuestionMark));
            else
                await tagService.PostTagAsync(guildId, channel.Id, tagName);
        }
    }
}