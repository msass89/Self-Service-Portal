namespace SelfServiceHub.Services.EmailSender
{
    public interface ILinkGenerator
    {
        string GenerateLink(string page, string userId, string token);
    }
}