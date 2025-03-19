using System;

namespace Normcore.Services
{
    /// <summary>
    /// The client used to interact with the Normcore Services API.
    /// </summary>
    public partial class ServicesApi : IDisposable
    {
        /// <summary>
        /// The authenticated session used when making requests.
        /// </summary>
        public INormcoreServicesSession Session { get; }
        
        /// <summary>
        /// The Normcore Services status API.
        /// </summary>
        public StatusApi Status { get; }

        /// <summary>
        /// The Normcore Services users API.
        /// </summary>
        public UsersApi Users { get; }

        /// <summary>
        /// The Normcore Services lobbies API.
        /// </summary>
        public LobbiesApi Lobbies { get; }

        internal ServicesApi(INormcoreServicesSessionInternal session, IAuthentication auth)
        {
            Session = session;

            Status = new StatusApi(session);
            Users = new UsersApi(session, auth);
            Lobbies = new LobbiesApi(session, auth);
        }

        /// <summary>
        /// Disposes the underlying client session.
        /// </summary>
        public void Dispose()
        {
            Session.Dispose();
        }
    }
}
