using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Template.Application.DTOs;
using Template.Application.Features.F_Notifications.Commands.Create;

namespace Template.Web.API.Controllers
{
    /// <summary>
    /// Controller for managing notifications.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ApiControllerBase
    {
        private readonly IHubContext<NotificationsHub, INotificationClient> _hubContext;
        private readonly ILogger<NotificationController> _logger;

        /// <summary>
        /// Constructor injecting required dependencies.
        /// </summary>
        /// <param name="hubContext">SignalR hub context for real-time notifications.</param>
        /// <param name="logger">Logger for tracking actions.</param>
        public NotificationController(IHubContext<NotificationsHub, INotificationClient> hubContext, ILogger<NotificationController> logger)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates a new notification and broadcasts it via SignalR.
        /// </summary>
        /// <param name="command">Notification data transfer object.</param>
        /// <returns>Returns the notification creation response.</returns>
        [HttpPost]
        public async Task<ActionResult<NotificationResponse>> Create(NotificationDTO command)
        {
            if (command == null)
            {
                _logger.LogWarning("CreateNotification received a null request.");
                return BadRequest("Invalid notification request.");
            }

            var result = await Mediator.Send(new NotificationCommand { NotificationDTO = command });

            if (result.Success)
            {
                _logger.LogInformation("New notification created successfully. Broadcasting to clients.");

                // Broadcast to all connected SignalR clients
                await _hubContext.Clients.All.ReceiveNotification($"📢 New notification: {command.Message}");

                return Ok(result);
            }

            _logger.LogError("Failed to create notification. Reason: {ErrorMessage}", result.ErrorMessage);
            return StatusCode(500, result);
        }
    }
}
