using System;
using System.Linq;
using System.Threading.Tasks;
using BubenBot.Data;
using BubenBot.Data.Models;
using BubenBot.Services.Tag;
using Microsoft.EntityFrameworkCore;
using Moq.AutoMock;
using Xunit;

namespace BubenBot.Services.Tests.Tags
{
    public class ModifyTagTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly TagService _tagService;

        public ModifyTagTests()
        {
            var options = new DbContextOptionsBuilder<BotContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N"));
            var botContext = new BotContext(options.Options);
            
            _autoMocker = new AutoMocker();
            _autoMocker.Use(botContext);
            _tagService = _autoMocker.CreateInstance<TagService>();
        }

        [Fact]
        public async Task ModifyTag_NameIsNull_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _tagService.ModifyTagAsync(0, 0, null, string.Empty));
        }

        [Fact]
        public async Task ModifyTag_NameIsWhiteSpace_Throws()
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => _tagService.ModifyTagAsync(0, 0, " ", string.Empty));
        }

        [Fact]
        public async Task ModifyTag_ContentIsNull_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _tagService.ModifyTagAsync(0, 0, "name", null));
        }

        [Fact]
        public async Task ModifyTag_ContentIsWhiteSpace_Throws()
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => _tagService.ModifyTagAsync(0, 0, "name", " "));
        }

        [Fact]
        public async Task ModifyTag_ModificationIsCaseInsensitive_Valid()
        {
            var botContext = _autoMocker.Get<BotContext>();

            var entity = new TagEntity()
            {
                Name = "tag"
            };
            
            await botContext.Tags.AddAsync(entity);
            await botContext.SaveChangesAsync();

            await _tagService.ModifyTagAsync(0, 0, "TAG", "newContent");
            Assert.True(entity.Content == "newContent");
        }

        [Fact]
        public async Task ModifyTag_TagDoesntExist_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _tagService.ModifyTagAsync(0, 0, "name", "newContent"));
        }

        [Fact]
        public async Task ModifyTag_TagExists_Valid()
        {
            var botContext = _autoMocker.Get<BotContext>();

            var entity = new TagEntity()
            {
                Name = "tag"
            };
            await botContext.AddAsync(entity);
            await botContext.SaveChangesAsync();

            await _tagService.ModifyTagAsync(0, 0, "tag", "newContent");
            Assert.True(entity.Content == "newContent");
        }

        [Fact]
        public async Task ModifyTag_UnmatchingIdCantModify_Throws()
        {
            var botContext = _autoMocker.Get<BotContext>();

            var entity = new TagEntity()
            {
                OwnerId = 1,
                Name = "tag"
            };
            await botContext.Tags.AddAsync(entity);
            await botContext.SaveChangesAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _tagService.ModifyTagAsync(0, 2, "tag", "newContent"));
        }
    }
}