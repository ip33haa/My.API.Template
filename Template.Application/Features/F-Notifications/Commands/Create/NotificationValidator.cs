using FluentValidation;

namespace Template.Application.Features.F_Notifications.Commands.Create
{
    public class NotificationValidator : AbstractValidator<NotificationCommand>
    {
        public NotificationValidator()
        {
            RuleFor(x => x.NotificationDTO.Sender).NotEmpty().WithMessage("Sender ID is required");
            RuleFor(x => x.NotificationDTO.Receiver).NotEmpty().WithMessage("Receiver ID is required");
            RuleFor(x => x.NotificationDTO.Message).NotEmpty().WithMessage("Message cannot be empty");
            RuleFor(x => x.NotificationDTO.NotificationType).NotEmpty().WithMessage("Notification Type is required");
        }
    }
}
