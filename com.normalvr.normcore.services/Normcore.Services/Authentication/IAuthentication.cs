namespace Normcore.Services
{
    /// <summary>
    /// An interface that provides the access token for an authenticated session.
    /// </summary>
    public interface IAuthentication
    {
        /// <summary>
        /// The access token for the authenticated session.
        /// </summary>
        string AccessToken { get; }
    }
}
