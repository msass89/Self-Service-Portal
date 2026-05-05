using SelfServiceHub.Models.Messages;

namespace SelfServiceHub.Services.EmailSender
{
    public class DevEmailSender : IEmailSender
    {
        // This is a simple in-memory email sender for development purposes. It stores sent emails in a static list that can be accessed for testing or debugging.
        public static List<EmailQueueMessage> SentEmails = new();

        // This is a development email sender that simulates sending emails by storing them in memory.
        public Task SendAsync(string to, string subject, string htmlContent, string? link)
        {
            SentEmails.Add(new EmailQueueMessage
            {
                To = to,
                Subject = subject,
                HtmlContent = htmlContent,
                Link = link,
                SentAt = DateTime.UtcNow
            });

            return Task.CompletedTask;
        }
    }
}



