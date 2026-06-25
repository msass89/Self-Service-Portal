using SelfServiceHub.Models.Entities;
using SelfServiceHub.Models.DTO;

namespace SelfServiceHub.Services.EmailSender
{
    public class AccountEmailService
    {
        private readonly UserService _userService;
        private readonly IEmailQueue _emailQueue;
        private readonly ILinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountEmailService(
            UserService userService,
            IEmailQueue emailQueue,
            ILinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _emailQueue = emailQueue;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendConfirmationEmailAsync(ApplicationUser user)
        {
            // Generate confirmation token and encode it for URL safety
            var token = await _userService.GenerateAccountConfirmationTokenAsync(user);
            var confirmationLink = _linkGenerator.GenerateLink("/Account/ConfirmEmail", user.Id, token);

            // Encode user input and link for HTML safety to prevent XSS attacks
            var safeEmail = System.Net.WebUtility.HtmlEncode(user.Email);
            var safeLink = System.Net.WebUtility.HtmlEncode(confirmationLink);

            await _emailQueue.EnqueueAsync(new EmailQueueMessage
            {
                To = safeEmail,
                Subject = "Confirm your account",
                HtmlContent = $"Please confirm your account: <a href='{safeLink}'>Confirm</a>",
                Link = confirmationLink
            });
        }

        public async Task SendPasswordResetEmailAsync(ApplicationUser user)
        {
            // Generate password reset token and encode it for URL safety
            var token = await _userService.GeneratePasswordResetTokenAsync(user);
            var resetLink = _linkGenerator.GenerateLink("/Account/ResetPassword", user.Id, token);

            // Encode user input and link for HTML safety to prevent XSS attacks
            var safeEmail = System.Net.WebUtility.HtmlEncode(user.Email);
            var safeLink = System.Net.WebUtility.HtmlEncode(resetLink);

            await _emailQueue.EnqueueAsync(new EmailQueueMessage
            {
                To = safeEmail,
                Subject = "Reset your password",
                HtmlContent = $"You can reset your password here: <a href='{safeLink}'>Reset Password</a>",
                Link = resetLink
            });
        }

        // Helper method to generate links with dynamic scheme based on environment
        /*public string GenerateLink(string page, string userId, string token)
        {
            // Encode the token and user ID for URL safety
            var encodedToken = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(token));
            var encodedUserId = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(userId));

            // Dynamically set scheme based on environment
            var httpContext = _httpContextAccessor.HttpContext;
            var scheme = httpContext?.Request?.IsHttps == true ? "https" :
                (httpContext?.Request?.Host.Host == "localhost" ? "http" : "https");


           return _linkGenerator.GetUriByPage(
                httpContext,
                page: page,
                values: new
                {
                    area = "Identity",
                    userId = encodedUserId,
                    token = encodedToken
                },
                scheme: scheme
            );
        }*/
    }
}