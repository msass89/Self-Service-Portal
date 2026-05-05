using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfServiceHub.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace SelfServiceHub.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserService _userService;

        public ConfirmEmailModel(UserService userService)
        {
            _userService = userService;
        }

        private bool isConfirmed = false;
        public bool IsConfirmed
        {
            get => isConfirmed;
            set => isConfirmed = value;
        }

        // is called when the user clicks the confirmation link in their email. 
        // It takes the user ID and token as parameters, validates them, and confirms the user's email
        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest();

            userId = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userId));
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));            
            
            var user = await _userService.GetUserByIdAsync(userId);

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
    }
}