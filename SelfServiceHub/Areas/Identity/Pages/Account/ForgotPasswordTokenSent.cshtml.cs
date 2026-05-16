using SelfServiceHub.Services.EmailSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfServiceHub.Models.ViewModels;

namespace SelfServiceHub.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordTokenSent : PageModel
    {

        private readonly IWebHostEnvironment _env;

        public ConfirmationViewModel ViewModel { get; set; }

        public ForgotPasswordTokenSent(IWebHostEnvironment env)
        {
            _env = env;
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
