using System.Threading.Tasks;

namespace BubenBot.Common.Messaging
{
    public interface INotificationHandler<TNotification> 
        where TNotification : INotification
    {
        Task HandleNotificationAsync(TNotification notification);
    }
}