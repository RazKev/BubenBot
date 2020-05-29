using System;
using System.Threading.Tasks;
using BubenBot.Services.Prefix;
using Microsoft.Extensions.Configuration;
using Moq.AutoMock;
using Xunit;

namespace BubenBot.Services.Tests.Prefix
{
    public class ConfigurationPrefixTests
    {
        private const string Prefix = "!";
        private const string CommandPrefixConfigPath = ConfigurationPrefixService.CommandPrefixConfigPath;
        private const string TagPrefixConfigPath = ConfigurationPrefixService.TagPrefixConfigPath;
        
        private readonly IConfiguration _configuration;
        private readonly ConfigurationPrefixService _prefixService;

        public ConfigurationPrefixTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();

            var mocker = new AutoMocker();
            
            mocker.Use(_configuration);
            _prefixService = mocker.CreateInstance<ConfigurationPrefixService>();
        }

        [Fact]
        public async Task GetCommandPrefix_NoPrefixSet_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _prefixService.GetCommandPrefixAsync(0));
        }

        [Fact]
        public async Task GetCommandPrefix_ReturnsProperConfigurationValue_Valid()
        {
            _configuration[CommandPrefixConfigPath] = Prefix;
            Assert.True(await _prefixService.GetCommandPrefixAsync(0) == Prefix);
        }

        [Fact]
        public async Task SetCommandPrefix_PrefixIsNull_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _prefixService.SetCommandPrefixAsync(0, null));
        }
        
        [Fact]
        public async Task SetCommandPrefix_PrefixIsWhiteSpace_Throws()
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => _prefixService.SetCommandPrefixAsync(0, " "));
        }
        
        [Fact]
        public async Task SetCommandPrefix_ReturnsProperConfigurationValue_Valid()
        {
            const string newPrefix = "?";
            await _prefixService.SetCommandPrefixAsync(0, newPrefix);
            Assert.True(_configuration[CommandPrefixConfigPath] == newPrefix);
        }
        
        [Fact]
        public async Task GetTagPrefix_NoPrefixSet_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _prefixService.GetTagPrefixAsync(0));
        }

        [Fact]
        public async Task GetTagPrefix_ReturnsProperConfigurationValue_Valid()
        {
            _configuration[TagPrefixConfigPath] = Prefix;
            Assert.True(await _prefixService.GetTagPrefixAsync(0) == Prefix);
        }
        
        [Fact]
        public async Task SetTagPrefix_PrefixIsNull_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _prefixService.SetTagPrefixAsync(0, null));
        }
        
        [Fact]
        public async Task SetTagPrefix_PrefixIsWhiteSpace_Throws()
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => _prefixService.SetTagPrefixAsync(0, " "));
        }
        
        [Fact]
        public async Task SetTagPrefix_ReturnsProperConfigurationValue_Valid()
        {
            const string newPrefix = "?";
            await _prefixService.SetTagPrefixAsync(0, newPrefix);
            Assert.True(_configuration[TagPrefixConfigPath] == newPrefix);
        }
    }
}