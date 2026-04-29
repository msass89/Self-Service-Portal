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

            var confirmationLink = _linkGenerator.GetUriByPage(
                _httpContextAccessor.HttpContext,
                page: "/Account/ConfirmEmail",
                values: new
                {
                    area = "Identity",
                    userId = user.Id,
                    token = token
                }
            );

            await _emailQueue.EnqueueAsync(new EmailQueueMessage
            {
                To = user.Email,
                Subject = "Confirm your account",
                HtmlContent = $"Please confirm your account: <a href='{confirmationLink}'>Confirm</a>",
                ConfirmationLink = confirmationLink
            });
        }
    }
}