public class DevEmailSender : IEmailSender
{
    // This is a simple in-memory email sender for development purposes. It stores sent emails in a static list that can be accessed for testing or debugging.
    public static List<DevEmail> SentEmails = new();

    // This is a development email sender that simulates sending emails by storing them in memory.
    public Task SendAsync(string to, string subject, string htmlContent, string confirmationLink)
    {
        SentEmails.Add(new DevEmail
        {
            To = to,
            Subject = subject,
            HtmlContent = htmlContent,
            ConfirmationLink = confirmationLink,
            SentAt = DateTime.UtcNow
        });

        return Task.CompletedTask;
    }
}

// This is a simple model to represent an email that was "sent" by the DevEmailSender. In a real application, you would use a proper email sending service like SendGrid, SMTP, etc.
public class DevEmail
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string HtmlContent { get; set; }
    public string ConfirmationLink { get; set; }
    public DateTime SentAt { get; set; }
}