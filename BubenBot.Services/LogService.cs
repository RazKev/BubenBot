using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BubenBot.Services
{
    public class LogService : IHostedService
    {
        private readonly DiscordSocketClient _client;
        private readonly ILogger<LogService> _logger;
        private readonly CommandService _commandService;

        public LogService(DiscordSocketClient client, ILogger<LogService> logger, CommandService commandService)
        {
            _client = client;
            _logger = logger;
            _commandService = commandService;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _client.Log += ClientOnLog;
            _commandService.Log += ClientOnLog;

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _client.Log -= ClientOnLog;
            _commandService.Log -= ClientOnLog;
            
            return Task.CompletedTask;
        }

        private Task ClientOnLog(LogMessage logMessage)
        {
            _logger.Log(TranslateLogSeverity(logMessage.Severity), logMessage.Message);
            return Task.CompletedTask;
        }

        private static LogLevel TranslateLogSeverity(LogSeverity severity)
        {
            return severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Debug => LogLevel.Debug,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Info => LogLevel.Information,
                LogSeverity.Verbose => LogLevel.Trace,
                LogSeverity.Warning => LogLevel.Warning,
                _ => LogLevel.None
            };
        }
    }
}