using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace BubenBot.Services.Tag
{
    public interface ITagService
    {
        Task CreateTagAsync(ulong guildId, ulong authorId, string? name, string? content);
        Task ModifyTagAsync(ulong guildId, ulong modifierId, string? name, string? newContent);
        Task DeleteTagAsync(ulong guildId, ulong deleterId, string? name);
        Task<bool> TagExistsAsync(ulong guildId, string? name);
    }
}