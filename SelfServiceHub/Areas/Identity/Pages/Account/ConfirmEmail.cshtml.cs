using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfServiceHub.Services;
using SelfServiceHub.Services.EmailSender;

namespace SelfServiceHub.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserService _userService;
        private readonly IAccountEmailService _accountEmailService;

        public ConfirmEmailModel(UserService userService, IAccountEmailService accountEmailService)
        {
            _userService = userService;
            _accountEmailService = accountEmailService;
        }

        private bool isConfirmed = false;
        public bool IsConfirmed
        {
            get => isConfirmed;
            set => isConfirmed = value;
        }

        [BindProperty]
        public string UserId { get; set; }

        // is called when the user clicks the confirmation link in their email. 
        // It takes the user ID and token as parameters, validates them, and confirms the user's email
        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            UserId = userId;

            if (UserId == null || token == null)
                return BadRequest();

            var user = await _userService.GetUserByIdAsync(UserId);

            if (user == null)
                return NotFound();

            var result = await _userService.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                IsConfirmed = true;
            }
            else            
            {
                IsConfirmed = false;
            }

            return Page();
        }


        public async Task<IActionResult> OnPostResendAsync()
        {
            var user = await _userService.GetUserByIdAsync(UserId);

            Console.WriteLine($"Resend confirmation email for user: {user?.Email}");

            if (user != null && !user.EmailConfirmed)
            {
                await _accountEmailService.SendConfirmationEmailAsync(user);
            }

            return RedirectToPage("/Account/EmailConfirmationSent");
        }
    }
}