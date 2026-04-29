
namespace SelfServiceHub.Services.EmailSender
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string htmlContent, string? confirmationLink = null);
    }
}