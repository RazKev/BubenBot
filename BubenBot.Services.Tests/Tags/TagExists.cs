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
    public class TagExists
    {
        private readonly AutoMocker _autoMocker;
        private readonly TagService _tagService;
        
        public TagExists()
        {
            var options = new DbContextOptionsBuilder<BotContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N"));
            var botContext = new BotContext(options.Options);
            
            _autoMocker = new AutoMocker();
            _autoMocker.Use(botContext);
            _tagService = _autoMocker.CreateInstance<TagService>();
        }

        [Fact]
        public async Task TagExists_NameIsNull_Throw()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _tagService.TagExistsAsync(0, null));
        }

        [Fact]
        public async Task TagExists_NameIsWhiteSpace_Throw()
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => _tagService.TagExistsAsync(0, " "));
        }

        [Fact]
        public async Task TagExists_NoTagFound_Valid()
        {
            Assert.False(await _tagService.TagExistsAsync(0, "name"));
        }

        [Fact]
        public async Task TagExists_TagWasFound_Valid()
        {
            var botContext = _autoMocker.Get<BotContext>();
            var entity = new TagEntity()
            {
                GuildId = 0,
                Name = "name"
            };
            await botContext.Tags.AddAsync(entity);
            await botContext.SaveChangesAsync();
            
            Assert.True(await _tagService.TagExistsAsync(0, "name"));
        }
    }
}