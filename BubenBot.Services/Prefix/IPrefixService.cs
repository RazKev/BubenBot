using System.Threading.Tasks;

namespace BubenBot.Services.Prefix
{
    public interface IPrefixService
    {
        Task<string> GetCommandPrefixAsync(ulong guildId);
        Task<string> GetTagPrefixAsync(ulong guildId);
        Task SetCommandPrefixAsync(ulong guildId, string? prefix);
        Task SetTagPrefixAsync(ulong guildId, string? prefix);
    }
}