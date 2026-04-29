using SelfServiceHub.Models.Messages;

namespace SelfServiceHub.Services.EmailSender
{
    public interface IEmailQueue
    {
        Task EnqueueAsync(EmailQueueMessage message);
        Task<EmailQueueMessage> DequeueAsync(CancellationToken cancellationToken);
    }
}