
namespace SelfServiceHub.Services.EmailSender
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly IEmailQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory;

        public EmailBackgroundService(IEmailQueue queue, IServiceScopeFactory scopeFactory)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // get the next email message from the queue (this will wait if the queue is empty)
                var message = await _queue.DequeueAsync(stoppingToken);

                //as IEmailSender is registered as a scoped service, create a new scope to get a new instance for each email
                using var scope = _scopeFactory.CreateScope();
                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                try
                {
                    // send the email using the IEmailSender service
                    await emailSender.SendAsync(
                        message.To,
                        message.Subject,
                        message.HtmlContent,
                        message.Link
                    );
                }
                catch (OperationCanceledException)
                {
                    // The operation was canceled, likely due to application shutdown. Just exit the loop.
                    break;
                }
            }
        }
    }
}