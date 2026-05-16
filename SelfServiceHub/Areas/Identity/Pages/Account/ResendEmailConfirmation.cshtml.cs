
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfServiceHub.Services;
using SelfServiceHub.Services.EmailSender;

namespace SelfServiceHub.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserService _userService;
        private readonly AccountEmailService _accountEmailService;

        public ResendEmailConfirmationModel(UserService userService, AccountEmailService accountEmailService)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var user = await _userService.GetUserByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToPage("/Account/EmailConfirmationSent");
            }

            await _accountEmailService.SendConfirmationEmailAsync(user);

            return RedirectToPage("/Account/EmailConfirmationSent");
        }
    }
}
