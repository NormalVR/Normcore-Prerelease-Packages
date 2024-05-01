using System;
using System.Threading.Tasks;

namespace Normcore.Services
{
    public class NormcoreServices
    {
        #region Static Authentication

        /// <summary>
        /// Authenticate as a new anonymous user.
        /// </summary>
        /// <param name="key">A valid Normcore app key.</param>
        /// <returns></returns>
        public static async ValueTask<AnonymousUserAuthentication> CreateAnonymousUser(string key)
        {
            var result = await AuthService.CreateAnonymousUser(key);

            return new AnonymousUserAuthentication
            {
                UserID = result.User.ID,
                Secret = result.Auth.Secret,
                Authentication = new UserAuthentication(result.Token, result.User.ID),
            };
        }

        /// <summary>
        /// Authenticate as an existing anonymous user.
        /// </summary>
        /// <param name="key">A valid Normcore app key.</param>
        /// <param name="userID">The user ID.</param>
        /// <param name="secret">The user secret.</param>
        /// <returns></returns>
        public static async ValueTask<AnonymousUserAuthentication> AuthenticateAnonymousUser(string key, string userID, string secret)
        {
            var result = await AuthService.AuthenticateAnonymousUser(key, userID, secret);

            return new AnonymousUserAuthentication
            {
                UserID = userID,
                Secret = secret, // TODO use new secret from backend
                Authentication = new UserAuthentication(result.Token, userID),
            };
        }

        #endregion

        private IAuthentication auth;

        /// <summary>
        /// Construct a new NormcoreServicesClient with the provided authentication.
        /// </summary>
        public NormcoreServices(IAuthentication auth)
        {
            this.auth = auth;

            Lobbies = new LobbiesService(auth);
            Users = new UsersService(auth);
            Status = new StatusService();
        }

        /// <summary>
        /// The user ID of the authenticated client.
        /// </summary>
        public string UserID
        {
            get
            {
                if (auth is UserAuthentication uauth)
                {
                    return uauth.UserID;
                }

                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The Normcore Services lobbies API.
        /// </summary>
        public LobbiesService Lobbies { get; private set; }

        /// <summary>
        /// The Normcore Services users API.
        /// </summary>
        public UsersService Users { get; private set; }

        /// <summary>
        /// The Normcore Services status API.
        /// </summary>
        public StatusService Status { get; private set; }
    }
}
