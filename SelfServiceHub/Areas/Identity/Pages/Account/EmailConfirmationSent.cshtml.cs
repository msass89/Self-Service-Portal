using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfServiceHub.Models.ViewModels;
using SelfServiceHub.Services.EmailSender;

namespace SelfServiceHub.Areas.Identity.Pages.Account
{
    public class EmailConfirmationSent : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly IEmailSender _emailSender;

        public RegisterConfirmationViewModel ViewModel { get; set; }

        public EmailConfirmationSent(IWebHostEnvironment env, IEmailSender emailSender)
        {
            _env = env;
            _emailSender = emailSender;
        }

        public void OnGet()
        {
            ViewModel = new RegisterConfirmationViewModel
            {
                // indicate whether we're in development mode so the UI can decide whether to show the confirmation link
                IsDevelopment = _env.IsDevelopment(),
                
                // get the confirmation link from the DevEmailSender
                ConfirmationLink = DevEmailSender.SentEmails
                    .LastOrDefault()?.ConfirmationLink
            };
        }
    }
}