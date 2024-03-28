using System.Threading.Tasks;

namespace Normcore.Services
{
    public struct UsersService
    {
        private AuthenticatedContext auth;

        public UsersService(AuthenticatedContext auth)
        {
            this.auth = auth;
        }

        /// <summary>
        /// Get the currently authenticated user.
        /// </summary>
        public async ValueTask<UserObject> GetSelf()
        {
            var response = await NormcoreServicesRequest.Get("users/self").WithAuth(auth).Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<UserObject>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        /// <summary>
        /// Get a user by ID.
        /// </summary>
        public async ValueTask<UserObject> GetUser(string id)
        {
            var response = await NormcoreServicesRequest.Get($"users/{id}").WithAuth(auth).Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<UserObject>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        /// <summary>
        /// Heartbeat the currently authenticated user.
        /// </summary>
        public async ValueTask Heartbeat()
        {
            var response = await NormcoreServicesRequest.Post("users/heartbeat").WithAuth(auth).Send();

            if (response.Status == 204)
            {
                return;
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }
    }
}
