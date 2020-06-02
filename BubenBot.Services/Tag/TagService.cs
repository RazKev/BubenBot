using System;
using System.Linq;
using System.Threading.Tasks;
using BubenBot.Data;
using BubenBot.Data.Models;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;

namespace BubenBot.Services.Tag
{
    public class TagService : ITagService
    {
        private readonly DiscordSocketClient _client;
        private readonly BotContext _botContext;

        public TagService(DiscordSocketClient client, BotContext botContext)
        {
            _client = client;
            _botContext = botContext;
        }

        public async Task CreateTagAsync(ulong guildId, ulong authorId, string? name, string? content)
        {
            if (name is null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The tag name cannot be empty", nameof(name));

            if (content is null)
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("The tag content cannot be empty", nameof(content));

            name = name.Trim().ToLower();
            content = content.Trim();

            if (await (_botContext.Tags as IQueryable<TagEntity>)
                      .Where(x => x.GuildId == guildId).AnyAsync(x => x.Name == name))
                throw new InvalidOperationException($"A tag with the name {name} already exists.");

            var tag = new TagEntity()
            {
                GuildId = guildId,
                OwnerId = authorId,
                Name = name,
                Content = content,
                Created = DateTime.Now
            };

            await _botContext.Tags.AddAsync(tag);
            await _botContext.SaveChangesAsync();
        }

        public async Task ModifyTagAsync(ulong guildId, ulong modifierId, string? name, string? newContent)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The tag name cannot be empty", nameof(name));

            if (newContent is null)
                throw new ArgumentNullException(nameof(newContent));
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("The tag content cannot be empty", nameof(newContent));

            name = name.Trim().ToLower();
            newContent = newContent.Trim();

            var tag = await (_botContext.Tags as IQueryable<TagEntity>)
                            .Where(x => x.GuildId == guildId)
                            .Where(x => x.Name == name)
                            .SingleOrDefaultAsync();

            if (tag is null)
                throw new InvalidOperationException("The given tag doesn't exist");

            // Gotta extend this so it's permission based
            if (tag.OwnerId != modifierId)
                throw new InvalidOperationException("Only the owner can modify the tag");

            tag.Content = newContent;
            _botContext.Tags.Update(tag);
            await _botContext.SaveChangesAsync();
        }

        public async Task DeleteTagAsync(ulong guildId, ulong deleterId, string? name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The tag name cannot be empty", nameof(name));

            name = name.Trim().ToLower();

            var tag = await (_botContext.Tags as IQueryable<TagEntity>)
                            .Where(x => x.GuildId == guildId)
                            .Where(x => x.Name == name)
                            .SingleOrDefaultAsync();

            if (tag is null)
                throw new InvalidOperationException("The given tag doesn't exist");

            if (tag.OwnerId != deleterId)
                throw new InvalidOperationException("Only the owner can delete the tag");

            _botContext.Tags.Remove(tag);
            await _botContext.SaveChangesAsync();
        }

        public async Task<bool> TagExistsAsync(ulong guildId, string? name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The tag name cannot be empty", nameof(name));

            name = name.Trim().ToLower();

            return await (_botContext.Tags as IQueryable<TagEntity>)
                         .Where(x => x.GuildId == guildId)
                         .Where(x => x.Name == name)
                         .AnyAsync();
        }

        public async Task<TagEntity?> GetTagAsync(ulong guildId, string? name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The tag name cannot be empty", nameof(name));

            name = name.Trim().ToLower();

            return await (_botContext.Tags as IQueryable<TagEntity>)
                         .Where(x => x.GuildId == guildId)
                         .Where(x => x.Name == name)
                         .SingleOrDefaultAsync();
        }

        public async Task PostTagAsync(ulong guildId, ulong channelId, string? name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The tag name cannot be empty", nameof(name));

            var channel = _client.GetChannel(channelId);

            if (!(channel is IMessageChannel messageChannel))
                throw new InvalidOperationException($"The channel {channel.Id} is not a message channel");

            var tag = await GetTagAsync(guildId, name.Trim().ToLower());

            if (tag is null)
                throw new InvalidOperationException("The given tag doesn't exist");

            await messageChannel.SendMessageAsync(tag.Content);
        }
    }
}