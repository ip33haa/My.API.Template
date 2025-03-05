using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Template.Application.DTOs;
using Template.Application.Features.F_Notifications.Commands.Create;

namespace Template.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ApiControllerBase
    {
        private readonly IHubContext<NotificationsHub> _hubContext;

        // Inject the IHubContext<NotificationHub> to interact with SignalR
        public NotificationController(IHubContext<NotificationsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<ActionResult<NotificationResponse>> Create(NotificationDTO command)
        {
            var result = await Mediator.Send(new NotificationCommand() { NotificationDTO = command });

            // After creating the notification, send a SignalR message to all clients
            if (result.Success)
            {
                // You can modify the message or broadcast more details here
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"New notification: {command.Message}");
            }


            return result;
        }
    }
}
