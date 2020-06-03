using System.Threading.Tasks;
using BubenBot.Common.Messaging;
using BubenBot.Services.Core.Notifications;
using BubenBot.Services.Prefix;
using BubenBot.Services.Tag;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace BubenBot.Services
{
    public class TagHandlingService : INotificationHandler<MessageReceivedNotification>
    {
        private const string UnicodeQuestionMark = "\u2753"; // '❓'

        private readonly DiscordSocketClient _client;
        private readonly ITagService _tagService;
        private readonly  IPrefixService _prefixService;
        private readonly ILogger _logger;

        public TagHandlingService(
            DiscordSocketClient client,
            IPrefixService prefixService,
            ITagService tagService,
            ILogger<TagHandlingService> logger)
        {
            _client = client;
            _logger = logger;
            _prefixService = prefixService;
            _tagService = tagService;
        }

        public async Task HandleNotificationAsync(MessageReceivedNotification notification)
        {
            var message = notification.Message;
            
            if (message.Author.IsBot)
                return;
            if (!(message is SocketUserMessage userMessage))
                return;
            if (!(message.Channel is IGuildChannel channel))
                return;

            var guildId = channel.GuildId;
            var prefix = await _prefixService.GetTagPrefixAsync(guildId);

            if (message.Content.Substring(0, prefix.Length) != prefix)
                return;

            var tagName = userMessage.Content.Substring(prefix.Length);

            if (!await _tagService.TagExistsAsync(channel.GuildId, tagName))
                await message.AddReactionAsync(new Emoji(UnicodeQuestionMark));
            else
                await _tagService.PostTagAsync(guildId, channel.Id, tagName);
        }
    }
}