namespace BubenBot.Services.Prefix
{
    public interface IPrefixService
    {
        string GetPrefix(ulong guildId);
        void SetPrefix(ulong guildId, string prefix);
    }
}