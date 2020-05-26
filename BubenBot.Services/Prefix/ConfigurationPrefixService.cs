using Microsoft.Extensions.Configuration;

namespace BubenBot.Services.Prefix
{
    public class ConfigurationPrefixService : IPrefixService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationPrefixService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string GetPrefix(ulong guildId)
        {
            var prefix = _configuration["Discord:CommandPrefix"];
            if (string.IsNullOrWhiteSpace(prefix))
                prefix = "!";

            return prefix;
        }
        
        public void SetPrefix(ulong guildId, string prefix)
        {
            _configuration["Discord:CommandPrefix"] = prefix;
        }
    }
}