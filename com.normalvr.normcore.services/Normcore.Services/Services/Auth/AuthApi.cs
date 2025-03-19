using System;
using System.Threading.Tasks;
using Normcore.Services.Auth;
using static Normcore.Services.Validation;

namespace Normcore.Services
{
    internal static class AuthApi
    {
        private const string ApiKeyHeader = "Normcore-API-Key";

        public static async ValueTask<CreateAnonymousUserResult> CreateAnonymousUser(string baseUrl, string appId, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentOutOfRangeException(nameof(baseUrl), baseUrl, "Must be non-empty.");
            }
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentOutOfRangeException(nameof(appId), appId, "Must be non-empty.");
            }
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentOutOfRangeException(nameof(apiKey), apiKey, "Must be non-empty.");
            }

            var endpoint = FormatPath("apps/{0}/auth/anon/create", appId);
            var request = Request.Post(baseUrl, endpoint).WithHeader(ApiKeyHeader, apiKey);
            var response = await request.Send();

            return response.ParseDataResponse<CreateAnonymousUserResult>();
        }

        public static async ValueTask<AuthenticateAnonymousUserResult> AuthenticateAnonymousUser(
            string baseUrl,
            string appId,
            string apiKey,
            string userId,
            string secret
        )
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentOutOfRangeException(nameof(baseUrl), baseUrl, "Must be non-empty.");
            }
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentOutOfRangeException(nameof(appId), appId, "Must be non-empty.");
            }
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentOutOfRangeException(nameof(apiKey), apiKey, "Must be non-empty.");
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "Must be non-empty.");
            }
            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new ArgumentOutOfRangeException(nameof(secret), secret, "Must be non-empty.");
            }

            var body = new AuthenticateAnonymousUserRequest { Id = userId, Secret = secret };

            var endpoint = FormatPath("apps/{0}/auth/anon", appId);
            var request = Request.Post(baseUrl, endpoint, body).WithHeader(ApiKeyHeader, apiKey);
            var response = await request.Send();

            return response.ParseDataResponse<AuthenticateAnonymousUserResult>();
        }

        // TODO add admin / service worker authentication
    }
}
