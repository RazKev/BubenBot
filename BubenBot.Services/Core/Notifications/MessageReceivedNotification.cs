using BubenBot.Common.Messaging;
using Discord.WebSocket;

namespace BubenBot.Services.Core.Notifications
{
    public class MessageReceivedNotification : INotification
    {
        public MessageReceivedNotification(SocketMessage message)
        {
            Message = message;
        }

        public SocketMessage Message { get; }
    }
}