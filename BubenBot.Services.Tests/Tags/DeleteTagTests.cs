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
    public class DeleteTagTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly TagService _tagService;
        
        public DeleteTagTests()
        {
            var options = new DbContextOptionsBuilder<BotContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N"));
            var botContext = new BotContext(options.Options);
            
            _autoMocker = new AutoMocker();
            _autoMocker.Use(botContext);
            _tagService = _autoMocker.CreateInstance<TagService>();
        }

        [Fact]
        public async Task DeleteTag_NameIsNull_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _tagService.DeleteTagAsync(0, 0, null));
        }

        [Fact]
        public async Task DeleteTag_NameIsWhiteSpace_Throws()
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => _tagService.DeleteTagAsync(0, 0, " "));
        }

        [Fact]
        public async Task DeleteTag_TagNotFound_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _tagService.DeleteTagAsync(0, 0, "name"));
        }

        [Fact]
        public async Task DeleteTag_OwnerIdDoesntMatch_Throws()
        {
            var botContext = _autoMocker.Get<BotContext>();
            
            var entity = new TagEntity()
            {
                Name = "name",
                OwnerId = 0
            };
            await botContext.Tags.AddAsync(entity);
            await botContext.SaveChangesAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _tagService.DeleteTagAsync(0, 1, "name"));
        }

        [Fact]
        public async Task DeleteTag_TagWasRemoved_Valid()
        {
            var botContext = _autoMocker.Get<BotContext>();
            
            var entity = new TagEntity()
            {
                Name = "name",
                OwnerId = 0
            };
            await botContext.Tags.AddAsync(entity);
            await botContext.SaveChangesAsync();

            await _tagService.DeleteTagAsync(0, 0, "name");
            Assert.Empty(botContext.Tags);
        }
    }
}