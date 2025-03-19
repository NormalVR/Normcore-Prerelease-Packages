namespace Normcore.Services
{
    /// <summary>
    /// An interface that provides the user ID for an authenticated session.
    /// </summary>
    public interface IUserAuthentication : IAuthentication
    {
        /// <summary>
        /// The authenticated user ID.
        /// </summary>
        string UserId { get; }
    }
}
