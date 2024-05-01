namespace Normcore.Services
{
    public interface IAuthentication
    {
        /// <summary>
        /// The access token for this authenticated session.
        /// </summary>
        public string AccessToken { get; }
    }
}
