using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Normcore.Services
{
    using AnonymousUserSession = NormcoreServicesSession<AnonymousUserAuth>; 
    
    partial class NormcoreServices
    {
        private struct AnonymousUserSessionParams : IEquatable<AnonymousUserSessionParams>
        {
            public string baseUrl;
            public string appId;
            public string apiKey;
            public string userId;

            /// <inheritdoc />
            public bool Equals(AnonymousUserSessionParams other)
            {
                return baseUrl == other.baseUrl
                    && appId == other.appId
                    && apiKey == other.apiKey
                    && userId == other.userId;
            }
        }
        
        private static readonly Dictionary<AnonymousUserSessionParams, AnonymousUserSession> AnonymousUserSessionCache = new();

        private static AnonymousUserSession CreateAnonymousUserSession(
            AnonymousUserSessionParams data,
            AnonymousUserAuth auth
        )
        {
            var session = new AnonymousUserSession(data.baseUrl, data.appId, auth, () =>
            {
                AnonymousUserSessionCache.Remove(data);
            });
            AnonymousUserSessionCache.Add(data, session);
            return session;
        }
        
        /// <summary>
        /// Authenticate as a new anonymous user.
        /// </summary>
        /// <remarks>
        /// The created user may be reused in future sessions by using <see cref="AuthenticateAnonymousUser"/>.
        /// </remarks>
        /// <param name="appId">The Normcore app ID.</param>
        /// <param name="apiKey">The API key to authenticate with.</param>
        /// <param name="baseUrl">The base URL to use when making API requests, or <see langword="null"/>
        /// to use the default given by <see cref="NormcoreServicesSettings.Url"/>.</param>
        /// <returns>A new authenticated session.</returns>
        /// <seealso cref="AuthenticateAnonymousUser"/>
        public static async ValueTask<AnonymousUserSession> CreateAnonymousUser(
            string appId,
            string apiKey,
            string baseUrl = null
        )
        {
            baseUrl ??= NormcoreServicesSettings.Url;
            
            var result = await AuthApi.CreateAnonymousUser(baseUrl, appId, apiKey);
            
            var sessionData = new AnonymousUserSessionParams
            {
                baseUrl = baseUrl,
                appId = appId,
                apiKey = apiKey,
                userId = result.User.Id,
            };
            var auth = new AnonymousUserAuth(result.Token, result.User.Id, result.Auth.Secret, apiKey);
            return CreateAnonymousUserSession(sessionData, auth);
        }
        
        /// <summary>
        /// Authenticate as an existing anonymous user.
        /// </summary>
        /// <remarks>
        /// It can be helpful to store the authentication details of the user when first created. This ensures
        /// the <c>user ID</c> and <c>secret</c> are captured for use later.
        /// <code>
        /// using var session = await NormcoreServices.CreateAnonymousUser(...);
        /// PlayerPrefs.SetString("User", JsonUtility.ToJson(session.Auth));
        /// PlayerPrefs.Save();
        /// 
        /// // later:
        ///
        /// 
        /// var auth = JsonUtility.FromJson&lt;AnonymousUserAuth&gt;(PlayerPrefs.GetString("User"));
        /// using var session = await NormcoreServices.AuthenticateAnonymousUser(..., auth.UserId, auth.Secret);
        /// </code>
        /// </remarks>
        /// <param name="appId">The Normcore app ID.</param>
        /// <param name="apiKey">The API key to authenticate with.</param>
        /// <param name="userId">The ID of an existing user.</param>
        /// <param name="secret">The secret needed to authenticate as the specified user. This can be acquired
        /// via the <see cref="NormcoreServicesSession{T}.Auth"/> property of the session
        /// returned by <see cref="CreateAnonymousUser"/> when the user was first created.</param>
        /// <param name="baseUrl">The base URL to use when making API requests, or <see langword="null"/>
        /// to use the default given by <see cref="NormcoreServicesSettings.Url"/>.</param>
        /// <returns>A new authenticated session, or the existing session for this user if one already exists.</returns>
        /// <seealso cref="CreateAnonymousUser"/>
        public static async ValueTask<AnonymousUserSession> AuthenticateAnonymousUser(
            string appId,
            string apiKey,
            string userId,
            string secret,
            string baseUrl = null
        )
        {
            baseUrl ??= NormcoreServicesSettings.Url;

            var sessionData = new AnonymousUserSessionParams
            {
                baseUrl = baseUrl,
                appId = appId,
                apiKey = apiKey,
                userId = userId,
            };

            // Check if a session with the given parameters has already been created. Each session runs a 
            // heartbeat, so this helps to reduce pressure on the backend caused by redundant sessions.
            if (AnonymousUserSessionCache.TryGetValue(sessionData, out var session))
            {
                return session;
            }
            
            var result = await AuthApi.AuthenticateAnonymousUser(baseUrl, appId, apiKey, userId, secret);
            
            var auth = new AnonymousUserAuth(result.Token, userId, secret, apiKey);
            return CreateAnonymousUserSession(sessionData, auth);
        }
    }
}
