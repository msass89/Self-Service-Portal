using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfServiceHub.Services.Auth;

namespace SelfServiceHub.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly AuthService _authService;

        public LoginModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                // use the sign in manager to sign in the user
                var result = await _authService.LoginAsync(Input.Email, Input.Password, Input.RememberMe);

                Console.WriteLine($"Login attempt for {Input.Email}: {result}");
                if (result == LoginResult.Success)
                {
                    // if the sign in was successful, redirect to the return url
                    return RedirectToPage("/Account/LoginSuccess");
                }
                else if (result == LoginResult.LockedOut)
                {
                    // if the user is locked out, add an error to the model state indicating that the account is locked out and redisplay the form
                    ModelState.AddModelError("", "Your account has been locked out due to multiple failed login attempts. Please try again later.");
                }
                else if (result == LoginResult.NotAllowed)
                {
                    // if the user is not allowed to sign in, add an error to the model state indicating that the account is not allowed to sign in and redisplay the form
                    ModelState.AddModelError("", "Your account is not confirmed. Please check your email for a confirmation link.");
                }
                else
                {
                    // if the user does not exist, add a generic error message to prevent user enumeration and redisplay the form
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
