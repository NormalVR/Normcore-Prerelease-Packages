using System;

using UnityEngine;

namespace Normcore.Services
{
    /// <summary>
    /// A struct that holds the authentication details for an anonymous user. 
    /// </summary>
    [Serializable]
    public struct AnonymousUserAuth : IUserAuthentication
    {
        [SerializeField]
        private string _accessToken;
        [SerializeField]
        private string _userId;
        [SerializeField]
        private string _secret;
        [SerializeField]
        private string _apiKey;

        /// <inheritdoc />
        public string AccessToken => _accessToken;

        /// <inheritdoc />
        public string UserId => _userId;

        /// <summary>
        /// The secret needed to reauthenticate as the same anonymous user.
        /// </summary>
        /// <seealso cref="NormcoreServices.AuthenticateAnonymousUser"/>
        public string Secret => _secret;
        
        /// <summary>
        /// The API key used to authenticate with the Normcore Serviecs API when this user was created.
        /// </summary>
        public string ApiKey => _apiKey;
        
        internal AnonymousUserAuth(string accessToken, string userId, string secret, string apiKey)
        {
            _accessToken = accessToken;
            _userId = userId;
            _secret = secret;
            _apiKey = apiKey;
        }
    }
}
