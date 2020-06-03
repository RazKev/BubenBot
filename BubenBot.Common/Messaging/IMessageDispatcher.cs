using System.Threading.Tasks;

namespace BubenBot.Common.Messaging
{
    public interface IMessageDispatcher
    {
        Task DispatchAsync<TNotification>(TNotification notification) where TNotification : INotification;
    }
}