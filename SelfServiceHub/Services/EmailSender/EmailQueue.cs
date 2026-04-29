using System.Threading.Channels;
using SelfServiceHub.Models.Messages;

namespace SelfServiceHub.Services.EmailSender
{
    public class EmailQueue : IEmailQueue
    {
        private readonly Channel<EmailQueueMessage> _queue;

        public EmailQueue()
        {
            _queue = Channel.CreateUnbounded<EmailQueueMessage>();
        }

        // Enqueue a new email message to be sent
        public async Task EnqueueAsync(EmailQueueMessage message)
        {
            await _queue.Writer.WriteAsync(message);
        }

        // Dequeue the next email message to be sent (this will wait if the queue is empty)
        public async Task<EmailQueueMessage> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}