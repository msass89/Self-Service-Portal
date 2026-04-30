using SelfServiceHub.Models.Entities;
using Microsoft.AspNetCore.Identity;
using SelfServiceHub.Models.Messages;

namespace SelfServiceHub.Services.EmailSender
{
    public class AccountEmailService : IAccountEmailService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailQueue _emailQueue;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountEmailService(
            UserManager<ApplicationUser> userManager,
            IEmailQueue emailQueue,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _emailQueue = emailQueue;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendConfirmationEmailAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // Dynamically set scheme based on environment
                var httpContext = _httpContextAccessor.HttpContext;
                var scheme = httpContext?.Request?.IsHttps == true ? "https" :
                    (httpContext?.Request?.Host.Host == "localhost" ? "http" : "https");

                var confirmationLink = _linkGenerator.GetUriByPage(
                    httpContext,
                    page: "/Account/ConfirmEmail",
                    values: new
                    {
                        area = "Identity",
                        userId = user.Id,
                        token = token
                    },
                    scheme: scheme
                );

            // Encode user input and link for HTML safety to prevent XSS attacks
            var safeEmail = System.Net.WebUtility.HtmlEncode(user.Email);
            var safeLink = System.Net.WebUtility.HtmlEncode(confirmationLink);

            await _emailQueue.EnqueueAsync(new EmailQueueMessage
            {
                To = safeEmail,
                Subject = "Confirm your account",
                HtmlContent = $"Please confirm your account: <a href='{safeLink}'>Confirm</a>",
                ConfirmationLink = confirmationLink
            });
        }
    }
}