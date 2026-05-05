
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfServiceHub.Services;
using SelfServiceHub.Services.EmailSender;

namespace SelfServiceHub.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserService _userService;
        private readonly IAccountEmailService _accountEmailService;

        public ForgotPasswordModel(UserService userService, IAccountEmailService accountEmailService)
        {
            _userService = userService;
            _accountEmailService = accountEmailService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByEmailAsync(Input.Email);
                if (user == null || !await _userService.IsEmailConfirmedAsync(user))
                {
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                await _accountEmailService.SendPasswordResetEmailAsync(user);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
