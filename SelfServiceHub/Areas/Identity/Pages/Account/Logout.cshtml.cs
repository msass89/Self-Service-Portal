using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfServiceHub.Services.Auth;

namespace SelfServiceHub.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly AuthService _authService;

        public LogoutModel(AuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> OnPost()
        {
            await _authService.LogoutAsync();

            return RedirectToPage("/Account/LogoutSuccess");
        }
    }
}
