using SelfServiceHub.Services.EmailSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfServiceHub.Models.ViewModels;

namespace SelfServiceHub.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModel
    {

        private readonly IWebHostEnvironment _env;
        private readonly IEmailSender _emailSender;

        public ConfirmationViewModel ViewModel { get; set; }

        public ForgotPasswordConfirmation(IWebHostEnvironment env, IEmailSender emailSender)
        {
            _env = env;
            _emailSender = emailSender;
        }
        public void OnGet()
        {
            ViewModel = new ConfirmationViewModel
            {
                // indicate whether we're in development mode so the UI can decide whether to show the confirmation link
                IsDevelopment = _env.IsDevelopment(),

                // get the reset password link from the DevEmailSender
                Link = DevEmailSender.SentEmails
                    .LastOrDefault()?.Link
            };
        }
    }
}
