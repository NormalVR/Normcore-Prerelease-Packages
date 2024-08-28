using System.Threading.Tasks;

using static Normcore.Services.Validation;

namespace Normcore.Services
{
    public class UsersService
    {
        private IAuthentication auth;
        private string appKey;

        public UsersService(IAuthentication auth, string appKey)
        {
            this.auth = auth;
            this.appKey = appKey;
        }

        /// <summary>
        /// Get the currently authenticated user.
        /// </summary>
        public async ValueTask<UserObject> GetSelf()
        {
            var endpoint = FormatPath("apps/{0}/users/self", appKey);
            var request = NormcoreServicesRequest.Get(endpoint).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<UserObject>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        /// <summary>
        /// Get a user by ID.
        /// </summary>
        public async ValueTask<UserObject> GetUser(string userID)
        {
            var endpoint = FormatPath("apps/{0}/users/{1}", appKey, userID);
            var request = NormcoreServicesRequest.Get(endpoint).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<UserObject>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        /// <summary>
        /// Heartbeat the currently authenticated user.
        /// </summary>
        public async ValueTask Heartbeat()
        {
            var endpoint = FormatPath("apps/{0}/users/heartbeat", appKey);
            var request = NormcoreServicesRequest.Post(endpoint).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 204)
            {
                return;
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }
    }
}
