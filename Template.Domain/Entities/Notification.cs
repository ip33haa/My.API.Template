namespace Template.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid Sender { get; set; } = Guid.Empty;
        public Guid Receiver { get; set; } = Guid.Empty;
        public bool IsRead { get; set; } = false;
        public bool IsSent { get; set; } = false;
        public string? NotificationType { get; set; }
        public bool IsUrgent { get; set; } = false;
        public DateTime DateSent { get; set; }
        public DateTime? DateAcknowledge { get; set; }
    }
}
