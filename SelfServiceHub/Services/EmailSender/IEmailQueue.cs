using SelfServiceHub.Models.DTO;

namespace SelfServiceHub.Services.EmailSender
{
    public interface IEmailQueue
    {
        Task EnqueueAsync(EmailQueueMessage message);
        Task<EmailQueueMessage> DequeueAsync(CancellationToken cancellationToken);
    }
}