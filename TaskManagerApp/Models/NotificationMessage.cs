namespace TaskManagerApp.Models
{
    public class NotificationMessage
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
        public string Message { get; set; }
    }
}
