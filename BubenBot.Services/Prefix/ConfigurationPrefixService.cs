using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BubenBot.Services.Prefix
{
    public class ConfigurationPrefixService : IPrefixService
    {
        internal const string CommandPrefixConfigPath = "Discord:CommandPrefix";
        internal const string TagPrefixConfigPath = "Discord:TagPrefix";
        
        private readonly IConfiguration _configuration;

        public ConfigurationPrefixService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public Task<string> GetCommandPrefixAsync(ulong guildId)
        {
            var prefix = _configuration[CommandPrefixConfigPath];
            if (prefix is null)
                throw new InvalidOperationException("No command prefix was set");

            return Task.FromResult(prefix);
        }

        public Task<string> GetTagPrefixAsync(ulong guildId)
        {
            var prefix = _configuration[TagPrefixConfigPath];
            if (prefix is null)
                throw new InvalidOperationException("No tag prefix was set");

            return Task.FromResult(prefix);
        }

        public Task SetCommandPrefixAsync(ulong guildId, string? prefix)
        {
            if (prefix is null)
                throw new ArgumentNullException(nameof(prefix));
            if (string.IsNullOrWhiteSpace(prefix))
                throw new ArgumentException("The command prefix cannot be blank", nameof(prefix));
            
            _configuration[CommandPrefixConfigPath] = prefix;
            return Task.CompletedTask;
        }

        public Task SetTagPrefixAsync(ulong guildId, string? prefix)
        {
            if (prefix is null)
                throw new ArgumentNullException(nameof(prefix));
            if (string.IsNullOrWhiteSpace(prefix))
                throw new ArgumentException("The tag prefix cannot be blank", nameof(prefix));
            
            _configuration[TagPrefixConfigPath] = prefix;
            return Task.CompletedTask;
        }
    }
}