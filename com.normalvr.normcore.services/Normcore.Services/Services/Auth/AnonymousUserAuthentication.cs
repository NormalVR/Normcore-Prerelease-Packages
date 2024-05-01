namespace Normcore.Services
{
    public struct AnonymousUserAuthentication
    {
        /// <summary>
        /// The authenticated user ID.
        /// </summary>
        public string UserID;

        /// <summary>
        /// The anonymous user secret to reauthenticate as the same anonymous user.
        /// </summary>
        public string Secret;

        /// <summary>
        /// The authentication object to provide to NormcoreServicesClient.
        /// </summary>
        public IAuthentication Authentication;
    }
}