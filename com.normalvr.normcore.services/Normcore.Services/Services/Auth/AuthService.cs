﻿using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Normcore.Services
{
    [JsonObject]
    internal struct CreateAnonymousUserResult
    {
        [JsonProperty("user")] public UserObject User;

        [JsonProperty("auth")] public AnonymousAuth Auth;

        [JsonProperty("token")] public string Token;
    }

    [JsonObject]
    internal struct AuthenticateAnonymousUserResult
    {
        [JsonProperty("token")] public string Token;
    }

    [JsonObject]
    internal struct AnonymousAuth
    {
        [JsonProperty("secret")] public string Secret;
    }

    [JsonObject]
    internal struct RefreshAnonymousUserTokenRequestData
    {
        [JsonProperty("id")] public string ID;

        [JsonProperty("secret")] public string Secret;
    }

    internal static class AuthService
    {
        private const string NormcoreAppKeyHeader = "Normcore-App-Key";

        public static async ValueTask<CreateAnonymousUserResult> CreateAnonymousUser(string key)
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

        public static async ValueTask<AuthenticateAnonymousUserResult> AuthenticateAnonymousUser(string key, string id, string secret)
        {
            var body = new RefreshAnonymousUserTokenRequestData { ID = id, Secret = secret };

            var response = await NormcoreServicesRequest
                .Post("auth/user/anon", body)
                .WithHeader(NormcoreAppKeyHeader, key)
                .Send();

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
