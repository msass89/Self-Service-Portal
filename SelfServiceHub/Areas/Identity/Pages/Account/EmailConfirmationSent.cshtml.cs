using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfServiceHub.Models.ViewModels;
using SelfServiceHub.Services.EmailSender;

namespace SelfServiceHub.Areas.Identity.Pages.Account
{
    public class EmailConfirmationSent : PageModel
    {
        private readonly IWebHostEnvironment _env;

        public ConfirmationViewModel ViewModel { get; set; }

        public EmailConfirmationSent(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnGet()
        {
            ViewModel = new ConfirmationViewModel
            {
                // indicate whether we're in development mode so the UI can decide whether to show the confirmation link
                IsDevelopment = _env.IsDevelopment(),
                
                // get the confirmation link from the DevEmailSender
                Link = DevEmailSender.SentEmails
                    .LastOrDefault()?.Link
            };
        }
    }
}