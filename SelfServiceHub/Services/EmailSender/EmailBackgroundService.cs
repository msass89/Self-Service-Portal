
namespace SelfServiceHub.Services.EmailSender
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly IEmailQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory;
        //private readonly IEmailSender _emailSender;

        public EmailBackgroundService(IEmailQueue queue, IServiceScopeFactory scopeFactory)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = await _queue.DequeueAsync(stoppingToken);

                //as IEmailSender is registered as a scoped service, create a new scope to get a new instance for each email
                using var scope = _scopeFactory.CreateScope();
                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                try
                {
                    await emailSender.SendAsync(
                        message.To,
                        message.Subject,
                        message.HtmlContent,
                        message.ConfirmationLink
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Email failed: {ex.Message}");
                    // später: Retry-Logik
                }
            }
        }
    }
}