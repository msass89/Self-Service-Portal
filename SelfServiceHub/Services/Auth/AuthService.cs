using Microsoft.AspNetCore.Identity;
using SelfServiceHub.Models.Entities;

namespace SelfServiceHub.Services.Auth
{
    public class AuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<LoginResult> LoginAsync(string email, string password, bool rememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(
                email,
                password,
                rememberMe,
                lockoutOnFailure: true //set lockoutOnFailure to true to enable account lockout
            );

            if (result.Succeeded)
                return LoginResult.Success;

            if (result.IsLockedOut)
                return LoginResult.LockedOut;

            return LoginResult.Failed;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}