using System.Threading.Tasks;
using Newtonsoft.Json;
using static Normcore.Services.Validation;

namespace Normcore.Services
{
    [JsonObject]
    internal struct CreateAnonymousUserResult
    {
        [JsonProperty("user")]
        public UserObject User;

        [JsonProperty("auth")]
        public AnonymousAuth Auth;

        [JsonProperty("token")]
        public string Token;
    }

    [JsonObject]
    internal struct AuthenticateAnonymousUserResult
    {
        [JsonProperty("token")]
        public string Token;
    }

    [JsonObject]
    internal struct AnonymousAuth
    {
        [JsonProperty("secret")]
        public string Secret;
    }

    [JsonObject]
    internal struct RefreshAnonymousUserTokenRequestData
    {
        [JsonProperty("id")]
        public string ID;

        [JsonProperty("secret")]
        public string Secret;
    }

    internal static class AuthService
    {
        private const string NormcoreApiKeyHeader = "Normcore-API-Key";

        public static async ValueTask<CreateAnonymousUserResult> CreateAnonymousUser(string appKey, string apiKey)
        {
            var endpoint = FormatPath("apps/{0}/auth/anon/create", appKey);
            var request = NormcoreServicesRequest.Post(endpoint).WithHeader(NormcoreApiKeyHeader, apiKey);
            var response = await request.Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<CreateAnonymousUserResult>();
            }

            // TODO add expected error responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        public static async ValueTask<AuthenticateAnonymousUserResult> AuthenticateAnonymousUser(
            string key,
            string userID,
            string secret
        )
        {
            var body = new RefreshAnonymousUserTokenRequestData { ID = userID, Secret = secret };

            var endpoint = FormatPath("apps/{0}/auth/anon", key);
            var request = NormcoreServicesRequest.Post(endpoint, body);
            var response = await request.Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<AuthenticateAnonymousUserResult>();
            }

            // TODO add expected error responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        // TODO add admin / service worker authentication
    }
}
