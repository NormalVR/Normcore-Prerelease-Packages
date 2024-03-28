using System.Threading.Tasks;

namespace Normcore.Services
{
    public class NormcoreServicesClient
    {
        private AuthenticatedContext auth;

        /// <summary>
        /// Create a new instance of the NormcoreServices client by authenticating as a new anonymous user.
        /// </summary>
        /// <param name="key">A valid API key.</param>
        public static async ValueTask<NormcoreServicesClient> AsAnonymousUser(string key)
        {
            var result = await new AuthService(key).CreateAnonymousUser();

            return new NormcoreServicesClient { auth = new AuthenticatedContext { UserID = result.User.ID, Token = result.Token} };
        }

        // TODO add anonymous reauthentication

        /// <summary>
        /// The user ID of the authenticated client.
        /// </summary>
        public string UserID => auth.UserID;

        /// <summary>
        /// The Normcore Services status API.
        /// </summary>
        public StatusService Status => new ();

        /// <summary>
        /// The Normcore Services lobbies API.
        /// </summary>
        public LobbiesService Lobbies => new (auth);

        /// <summary>
        /// The Normcore Services users API.
        /// </summary>
        public UsersService Users => new (auth);
    }
}
