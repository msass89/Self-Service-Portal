
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SelfServiceHub.Services;

namespace SelfServiceHub.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserService _userService;

        public ResetPasswordModel(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [BindProperty]
            public string UserId { get; set; }

            [BindProperty]
            public string? Token { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }


            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        /// is called when the user clicks the password reset link in their email. 
        /// It takes the token and email as parameters, decodes the token and the email, and populates
        /// the Input model with the token and email for use in the password reset form.
        public IActionResult OnGet(string token, string userId)
        {
            if (token == null || userId == null)
            {
                // Don't reveal that the token or user ID is missing
                return BadRequest();
            }
            else
            {
                Input = new InputModel
                {
                    Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token)),
                    UserId = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userId))
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userService.GetUserByIdAsync(Input.UserId);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            // Attempt to reset the user's password using the provided token and new password
            var result = await _userService.ResetPasswordAsync(user, Input.Token, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
