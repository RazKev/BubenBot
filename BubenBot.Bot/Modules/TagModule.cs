using System.Threading.Tasks;
using BubenBot.Services.Tag;
using Discord.Commands;

namespace BubenBot.Bot.Modules
{
    [Name("Tags")]
    [Group("tag")]
    [Alias("tags")]
    public class TagModule : ModuleBase
    {
        private readonly ITagService _tagService;

        public TagModule(ITagService tagService)
        {
            _tagService = tagService;
        }

        [Command("create")]
        [Alias("add")]
        public async Task CreateTagAsync(string name, [Remainder] string content)
        {
            await _tagService.CreateTagAsync(Context.Guild.Id, Context.User.Id, name, content);
        }
    }
}