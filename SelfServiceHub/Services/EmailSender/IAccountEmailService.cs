using SelfServiceHub.Models.Entities;

namespace SelfServiceHub.Services.EmailSender
{
    public interface IAccountEmailService
    {
        Task SendConfirmationEmailAsync(ApplicationUser user);

        Task SendPasswordResetEmailAsync(ApplicationUser user);
    }
}