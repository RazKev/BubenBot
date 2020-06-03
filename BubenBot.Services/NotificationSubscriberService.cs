using System.Threading;
using System.Threading.Tasks;
using BubenBot.Common.Messaging;
using BubenBot.Services.Core.Notifications;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;

namespace BubenBot.Services
{
    public class NotificationSubscriberService : IHostedService
    {
        private readonly DiscordSocketClient _client;
        private readonly IMessageDispatcher _messageDispatcher;
        
        public NotificationSubscriberService(DiscordSocketClient client, IMessageDispatcher messageDispatcher)
        {
            _client = client;
            _messageDispatcher = messageDispatcher;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += OnMessageReceived;
            
            return Task.CompletedTask;;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived -= OnMessageReceived;
            
            return Task.CompletedTask;
        }

        private Task OnMessageReceived(SocketMessage message)
        {
            _messageDispatcher.DispatchAsync(new MessageReceivedNotification(message));
            
            return Task.CompletedTask;
        }
    }
}