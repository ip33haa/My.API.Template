using MediatR;
using Template.Application.DTOs;

namespace Template.Application.Features.F_Notifications.Commands.Create
{
    public class NotificationCommand : IRequest<NotificationResponse>
    {
        public NotificationDTO NotificationDTO { get; set; }
    }
}
