using System;
using System.Threading.Tasks;
using Normcore.Services.Users;
using static Normcore.Services.Validation;

namespace Normcore.Services
{
    public class UsersApi
    {
        private readonly INormcoreServicesSessionInternal _session;
        private readonly IAuthentication _auth;

        internal UsersApi(INormcoreServicesSessionInternal session, IAuthentication auth)
        {
            _session = session;
            _auth = auth;
        }

        /// <summary>
        /// Get the currently authenticated user.
        /// </summary>
        public async ValueTask<UserObject> GetSelf()
        {
            _session.CheckDisposed();
            
            var endpoint = FormatPath("apps/{0}/users/self", _session.AppId);
            var request = Request.Get(_session.BaseUrl, endpoint).WithAuth(_auth);
            var response = await request.Send();

            return response.ParseDataResponse<UserObject>();
        }

        /// <summary>
        /// Get a user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        public async ValueTask<UserObject> GetUser(string userId)
        {
            _session.CheckDisposed();
            
            var endpoint = FormatPath("apps/{0}/users/{1}", _session.AppId, ValidateUserId(userId));
            var request = Request.Get(_session.BaseUrl, endpoint).WithAuth(_auth);
            var response = await request.Send();
            
            return response.ParseDataResponse<UserObject>();
        }

        internal async ValueTask<GetHeartbeatBody> Heartbeat()
        {
            _session.CheckDisposed();
            
            var endpoint = FormatPath("apps/{0}/users/heartbeat", _session.AppId);
            var request = Request.Post(_session.BaseUrl, endpoint).WithAuth(_auth);
            var response = await request.Send();

            return response.ParseDataResponse<GetHeartbeatBody>();
        }
        
        private string ValidateUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentOutOfRangeException(nameof(userId), userId, $"Must be non-empty.");
            }

            return userId;
        }
    }
}
