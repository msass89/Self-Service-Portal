namespace SelfServiceHub.Models.Messages
{
    public class EmailQueueMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public string? ConfirmationLink { get; set; }
        public DateTime SentAt { get; set; }
    }
}

