namespace Normcore.Services
{
    public class UserAuthentication : IAuthentication
    {
        /// <inheritdoc cref="IAuthentication"/>>
        public string AccessToken { get; set; }

        /// <summary>
        /// The authenticated user ID.
        /// </summary>
        public string UserID;

        internal UserAuthentication(string access, string userID)
        {
            AccessToken = access;
            UserID = userID;
        }
    }
}
