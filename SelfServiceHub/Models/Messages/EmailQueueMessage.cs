namespace SelfServiceHub.Models.Messages
{
    public class EmailQueueMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public string? Link { get; set; }
        public DateTime SentAt { get; set; }
    }
}

