using Microsoft.AspNetCore.SignalR;

namespace Template.Web.API
{
    /// <summary>
    /// Background service that periodically notifies clients of the server time.
    /// </summary>
    public class ServerTimeNotifier : BackgroundService
    {
        private static readonly TimeSpan Period = TimeSpan.FromSeconds(5);
        private readonly ILogger<ServerTimeNotifier> _logger;
        private readonly IHubContext<NotificationsHub, INotificationClient> _context;

        public ServerTimeNotifier(ILogger<ServerTimeNotifier> logger, IHubContext<NotificationsHub, INotificationClient> context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Periodically sends server time notifications to connected clients.
        /// </summary>
        /// <param name="stoppingToken">Token to monitor for service shutdown.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{Service} started at {Time}", nameof(ServerTimeNotifier), DateTime.UtcNow);

            using var timer = new PeriodicTimer(Period);

            try
            {
                while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
                {
                    var dateTime = DateTime.UtcNow;
                    var message = $"📅 Server Time (UTC): {dateTime:yyyy-MM-dd HH:mm:ss}";

                    _logger.LogInformation("{Service} sending notification at {Time}", nameof(ServerTimeNotifier), dateTime);

                    // Broadcast to all connected clients
                    await _context.Clients.All.ReceiveNotification(message);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("{Service} is stopping due to cancellation", nameof(ServerTimeNotifier));
            }
            finally
            {
                _logger.LogInformation("{Service} stopped at {Time}", nameof(ServerTimeNotifier), DateTime.UtcNow);
            }
        }
    }
}
