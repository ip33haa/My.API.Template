namespace Template.Application.DTOs
{
    public class NotificationDTO
    {
        public string Message { get; set; }
        public Guid Sender { get; set; }
        public Guid Receiver { get; set; }
        public string NotificationType { get; set; }
        public bool IsUrgent { get; set; }
    }
}
