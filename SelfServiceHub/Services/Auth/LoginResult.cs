namespace SelfServiceHub.Services.Auth
{
    // This enum represents the possible outcomes of a login attempt
    public enum LoginResult
    {
        Success,
        Failed,
        LockedOut,
        NotAllowed
    }
}