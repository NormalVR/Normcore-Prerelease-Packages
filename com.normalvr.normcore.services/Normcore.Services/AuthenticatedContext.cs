namespace Normcore.Services
{
    public struct AuthenticatedContext
    {
        /// <summary>
        /// The authenticated user ID.
        /// </summary>
        public string UserID;

        /// <summary>
        /// The authorization token.
        /// </summary>
        public string Token;
    }
}
