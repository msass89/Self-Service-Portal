using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfServiceHub.Models.Entities;
using SelfServiceHub.Services;

namespace SelfServiceHub.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserService _userService;

        public ConfirmEmailModel(UserService userService)
        {
            _userService = userService;
        }

        // is called when the user clicks the confirmation link in their email. 
        // It takes the user ID and token as parameters, validates them, and confirms the user's email
        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest();

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound();

            var result = await _userService.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return Page();

            return BadRequest();
        }
    }
}