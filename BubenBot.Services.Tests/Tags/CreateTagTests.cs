using System;
using System.Threading.Tasks;
using BubenBot.Data;
using BubenBot.Data.Models;
using BubenBot.Services.Tag;
using Microsoft.EntityFrameworkCore;
using Moq.AutoMock;
using Xunit;

namespace BubenBot.Services.Tests.Tags
{
    public class CreateTagTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly TagService _tagService;
        
        public CreateTagTests()
        {
            var options = new DbContextOptionsBuilder<BotContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N"));
            var botContext = new BotContext(options.Options);
            
            _autoMocker = new AutoMocker();
            _autoMocker.Use(botContext);
            _tagService = _autoMocker.CreateInstance<TagService>();
        }

        [Fact]
        public async Task CreateTag_NameIsNull_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _tagService.CreateTagAsync(0, 0, null, string.Empty));
        }

        [Fact]
        public async Task CreateTag_NameIsWhiteSpace_Throws()
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => _tagService.CreateTagAsync(0, 0, " ", string.Empty));
        }

        [Fact]
        public async Task CreateTag_ContentIsNull_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _tagService.CreateTagAsync(0, 0, "name", null));
        }

        [Fact]
        public async Task CreateTag_ContentIsEmpty_Throws()
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => _tagService.CreateTagAsync(0, 0, "name", " "));
        }

        [Fact]
        public async Task CreateTag_TagAlreadyExistsWithSameCasing_Throws()
        {
            var botContext = _autoMocker.Get<BotContext>();
            await botContext.Tags.AddAsync(new TagEntity()
            {
                Name = "tag"
            });
            await botContext.SaveChangesAsync();
            
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _tagService.CreateTagAsync(0, 0, "tag", "content"));
        }

        [Fact]
        public async Task CreateTag_TagAlreadyExistsWithDifferentCasing_Throws()
        {
            var botContext = _autoMocker.Get<BotContext>();
            await botContext.Tags.AddAsync(new TagEntity()
            {
                Name = "tag"
            });
            await botContext.SaveChangesAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _tagService.CreateTagAsync(0, 0, "TAG", "content"));
        }

        [Fact]
        public async Task CreateTag_AddValidTag_Valid()
        {
            var botContext = _autoMocker.Get<BotContext>();

            await _tagService.CreateTagAsync(2, 3, "tag", "content");
            var tag = await botContext.Tags.FirstAsync();
            
            Assert.False(tag is null);
            Assert.True(tag.TagId == 1);
            Assert.True(tag.GuildId == 2);
            Assert.True(tag.OwnerId == 3);
            Assert.True(tag.Name == "tag");
            Assert.True(tag.Content == "content");
            Assert.True(tag.Created <= DateTime.Now);
        }
    }
}