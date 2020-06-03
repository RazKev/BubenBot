using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BubenBot.Common.Messaging
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;
        
        public MessageDispatcher(IServiceScopeFactory serviceScopeFactory, ILogger<MessageDispatcher> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
        
        public async Task DispatchAsync<TNotification>(TNotification notification) where TNotification : INotification
        {
            try
            {
                using var serviceScope = _serviceScopeFactory.CreateScope();
                
                foreach (var notificationHandler in serviceScope.ServiceProvider.GetServices<INotificationHandler<TNotification>>())
                {
                    await notificationHandler.HandleNotificationAsync(notification);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "An error occured while dispatching a notification: {Notification}", notification);
            }
        }
    }
}