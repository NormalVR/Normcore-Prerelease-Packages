using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Normcore.Services
{
    [JsonObject]
    public struct CreateAnonymousUserResult
    {
        [JsonProperty("user")] public UserObject User;

        [JsonProperty("auth")] public AnonymousAuth Auth;

        [JsonProperty("token")] public string Token;
    }

    [JsonObject]
    public struct AnonymousAuth
    {
        [JsonProperty("secret")] public string Secret;
    }

    [JsonObject]
    internal struct RefreshAnonymousUserTokenRequestData
    {
        [JsonProperty("id")] public string ID;

        [JsonProperty("secret")] public string Secret;
    }

    public struct AuthService
    {
        private string key;

        private const string NormcoreAppKeyHeader = "Normcore-App-Key";

        /// <summary>
        /// Create a new AuthService context.
        /// </summary>
        /// <param name="key">The API key to use during authentication.</param>
        public AuthService(string key)
        {
            this.key = key;
        }

        public async ValueTask<AnonymousAuth> AuthenticateAnonymousUser(string id, string secret)
        {
            var body = new RefreshAnonymousUserTokenRequestData { ID = id, Secret = secret };

            var response = await NormcoreServicesRequest
                .Post("auth/user/anon", body)
                .WithHeader(NormcoreAppKeyHeader, key)
                .Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<AnonymousAuth>();
            }

            // TODO add expected error responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        public async ValueTask<CreateAnonymousUserResult> CreateAnonymousUser()
        {
            var response = await NormcoreServicesRequest
                .Post("auth/user/anon/create")
                .WithHeader(NormcoreAppKeyHeader, key)
                .Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<CreateAnonymousUserResult>();
            }

            // TODO add expected error responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        // TODO add admin / service worker authentication
    }
}
