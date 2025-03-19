using Newtonsoft.Json;

namespace Normcore.Services.Auth
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
    internal struct AuthenticateAnonymousUserRequest
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("secret")]
        public string Secret;
    }
}
