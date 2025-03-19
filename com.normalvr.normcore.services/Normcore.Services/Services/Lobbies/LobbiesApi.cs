using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Normcore.Services.Lobbies;
using static Normcore.Services.Validation;
using static Normcore.Services.LobbiesValidation;

namespace Normcore.Services
{
    public class LobbiesApi
    {
        /// <summary>
        /// The maximum number of lobby members allowed in a single lobby (inclusive).
        /// </summary>
        public static readonly int MaxLobbyCapacity = 30;
        
        /// <summary>
        /// The maximum number of tags which can be associated with a single lobby (inclusive).
        /// </summary>
        public static readonly int MaxLobbyTagCount = 10;
        
        /// <summary>
        /// The maximum number of characters allowed in a lobby tag (inclusive).
        /// </summary>
        public static readonly int MaxLobbyTagLength = 64;
        
        /// <summary>
        /// The maximum number of characters allowed in a lobby name (inclusive).
        /// </summary>
        public static readonly int MaxLobbyNameLength = 128;
        
        /// <summary>
        /// The maximum number of characters allowed in a custom data key of a lobby (inclusive).
        /// </summary>
        public static readonly int MaxLobbyDataKeyLength = 64;
        
        /// <summary>
        /// The maximum number of characters allowed in a custom data value of a lobby (inclusive).
        /// </summary>
        public static readonly int MaxLobbyDataValLength = 256;
        
        /// <summary>
        /// The maximum number of tags allowed in a lobby query (inclusive).
        /// </summary>
        public static readonly int MaxLobbyQueryTagCount = 10;
        
        private readonly INormcoreServicesSessionInternal _session;
        private readonly IAuthentication _auth;

        internal LobbiesApi(INormcoreServicesSessionInternal session, IAuthentication auth)
        {
            _session = session;
            _auth = auth;
        }
        
        /// <summary>
        /// Create a lobby.
        /// </summary>
        /// <param name="capacity">
        /// The maximum number of lobby members allowed in the lobby. Must be in the range [1, <see cref="MaxLobbyCapacity"/>].
        /// </param>
        /// <param name="name">
        /// The name of the lobby. If provided, the name must not be an empty string, and can be up
        /// to <see cref="MaxLobbyNameLength"/> characters in length. 
        /// </param>
        /// <param name="tags">
        /// The tags associated with the lobby. They are useful when filtering lobbies with <see cref="GetLobbies"/>,
        /// allowing for improved performance when searching for relevant lobbies. Tags are case insensitive, and
        /// each tag can be up to <see cref="MaxLobbyTagLength"/> characters in length. Each lobby can have a maximum
        /// of <see cref="MaxLobbyTagCount"/> tags.
        /// </param>
        /// <param name="data">
        /// The custom data associated with the lobby.
        /// </param>
        /// <returns>The new lobby.</returns>
        public async ValueTask<LobbyObject> CreateLobby(
            int capacity,
            string name = null,
            string[] tags = null,
            LobbyDataContainer data = null
        )
        {
            _session.CheckDisposed();
            
            var body = new CreateLobbyRequestBody
            {
                Capacity = ValidateCapacity(capacity),
                Name = ValidateName(name),
                Tags = ValidateTags(tags),
                Data = ValidateDataContainer(data),
            };

            var endpoint = FormatPath("apps/{0}/lobbies", _session.AppId);
            var request = Request.Post( _session.BaseUrl, endpoint, body).WithAuth(_auth);
            var response = await request.Send();

            return response.ParseDataResponse<LobbyObject>();
        }

        /// <summary>
        /// Create a lobby if one does not yet exist with the same name.
        /// </summary>
        /// <param name="capacity">
        /// The maximum number of lobby members allowed in the lobby. Must be in the range [1, <see cref="MaxLobbyCapacity"/>].
        /// </param>
        /// <param name="name">
        /// The name of the lobby. If provided, the name must not be an empty string, and can be up
        /// to <see cref="MaxLobbyNameLength"/> characters in length. 
        /// </param>
        /// <param name="tags">
        /// The tags associated with the lobby. They are useful when filtering lobbies with <see cref="GetLobbies"/>,
        /// allowing for improved performance when searching for relevant lobbies. Tags are case insensitive, and
        /// each tag can be up to <see cref="MaxLobbyTagLength"/> characters in length. Each lobby can have a maximum
        /// of <see cref="MaxLobbyTagCount"/> tags.
        /// </param>
        /// <param name="data">
        /// The custom data associated with the lobby.
        /// </param>
        /// <returns>The new lobby, or the existing lobby if one with the same name already exists.</returns>
        public async ValueTask<LobbyObject> CreateOrGetLobby(
            int capacity,
            string name = null,
            string[] tags = null,
            LobbyDataContainer data = null
        )
        {
            _session.CheckDisposed();
            
            var body = new CreateLobbyRequestBody
            {
                Capacity = ValidateCapacity(capacity),
                Name = ValidateName(name),
                Tags = ValidateTags(tags),
                Data = ValidateDataContainer(data),
            };

            var endpoint = FormatPath("apps/{0}/lobbies/create-or-get", _session.AppId);
            var request = Request.Post(_session.BaseUrl, endpoint, body).WithAuth(_auth);
            var response = await request.Send();

            return response.ParseDataResponse<LobbyObject>();
        }

        /// <summary>
        /// Get the properties of a lobby.
        /// </summary>
        /// <param name="lobbyId">The ID of the lobby.</param>
        /// <returns>The lobby object.</returns>
        public async ValueTask<LobbyObject> GetLobby(string lobbyId)
        {
            _session.CheckDisposed();
            
            var endpoint = FormatPath("apps/{0}/lobbies/{1}", _session.AppId, ValidateLobbyId(lobbyId));
            var request = Request.Get(_session.BaseUrl, endpoint).WithAuth(_auth);
            var response = await request.Send();

            return response.ParseDataResponse<LobbyObject>();
        }

        /// <summary>
        /// Get the properties of a lobby by name (if set).
        /// </summary>
        /// <param name="name">The name of the lobby.</param>
        /// <returns>The lobby object.</returns>
        public async ValueTask<LobbyObject> GetLobbyByName(string name)
        {
            _session.CheckDisposed();
            
            var endpoint = FormatPath("apps/{0}/lobbies/name/{1}", _session.AppId, ValidateName(name));
            var request = Request.Get(_session.BaseUrl, endpoint).WithAuth(_auth);
            var response = await request.Send();

            return response.ParseDataResponse<LobbyObject>();
        }
        
        /// <summary>
        /// Get all available lobbies.
        /// </summary>
        /// <remarks>
        /// Use an async foreach loop to iterate over the items:
        /// <code>
        /// await foreach (var lobby in lobbies.GetAllLobbies())
        /// {
        ///     // Do something
        /// }
        /// </code>
        /// </remarks>
        /// <param name="query">An optional query used to filter and sort the lobbies.</param>
        /// <returns>An async enumerable of lobby objects.</returns>
        /// <seealso cref="GetLobbies"/>
        public async IAsyncEnumerable<LobbyObject> GetAllLobbies(LobbyQuery? query = null)
        {
            await foreach (var item in PageInfo.IterateByItem(page => GetLobbies(query, page)))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Get the available lobbies.
        /// </summary>
        /// <param name="query">An optional query used to filter and sort the lobbies.</param>
        /// <param name="page">The page to retrieve, or <see langword="null"/> to retrieve the first page.</param>
        /// <returns>A page of lobby objects.</returns>
        /// <seealso cref="GetAllLobbies"/>
        public async ValueTask<Page<LobbyObject>> GetLobbies(LobbyQuery? query = null, PageInfo? page = null)
        {
            _session.CheckDisposed();

            Request request;

            if (query != null)
            {
                // For queries, pagination parameters are delivered in the body.
                var q = query.Value;
                var p = page ?? PageInfo.Default();

                if (p.Offset.HasValue)
                {
                    q.offset = p.Offset.Value;
                    q.count = p.Count;
                }
                else
                {
                    // TODO: Add cursor pagination support once the backend supports it.
                    throw new System.NotImplementedException();
                }
                
                var endpoint = FormatPath("apps/{0}/lobbies/query", _session.AppId);
                request = Request.Post(_session.BaseUrl, endpoint, q).WithAuth(_auth).WithPage(page, false);
            }
            else
            {
                var endpoint = FormatPath("apps/{0}/lobbies", _session.AppId);
                request = Request.Get(_session.BaseUrl, endpoint).WithAuth(_auth).WithPage(page, true);
            }
            
            var response = await request.Send();

            return response.ParsePageResponse<LobbyObject>();
        }
        
        /// <summary>
        /// Get the lobbies this client is a member of.
        /// </summary>
        /// <returns>A new array of lobby IDs.</returns>
        public async ValueTask<string[]> GetJoinedLobbies()
        {
            _session.CheckDisposed();
            
            var endpoint = FormatPath("apps/{0}/lobbies/joined", _session.AppId);
            var request = Request.Get(_session.BaseUrl, endpoint).WithAuth(_auth);
            var response = await request.Send();

            return response.ParseDataResponse<IdObject[]>().Select(x => x.Id).ToArray();
        }

        /// <summary>
        /// Get the owner of a lobby.
        /// </summary>
        /// <param name="lobbyId">The ID of the lobby.</param>
        /// <returns>The user ID of the lobby owner.</returns>
        public async ValueTask<string> GetLobbyOwner(string lobbyId)
        {
            _session.CheckDisposed();
            
            var endpoint = FormatPath("apps/{0}/lobbies/{1}/owner", _session.AppId, ValidateLobbyId(lobbyId));
            var request = Request.Get(_session.BaseUrl, endpoint).WithAuth(_auth);
            var response = await request.Send();

            return response.ParseDataResponse<UserObject>().Id;
        }

        /// <summary>
        /// Set the owner of a lobby.
        /// </summary>
        /// <remarks>
        /// The user associated with the current session must be the lobby owner to set the owner.
        /// </remarks>
        /// <param name="lobbyId">The ID of the lobby.</param>
        /// <param name="userId">The ID of the new owner.</param>
        public async ValueTask SetLobbyOwner(string lobbyId, string userId)
        {
            _session.CheckDisposed();
            
            var body = new SetLobbyOwnerRequestBody { Id = ValidateUserId(userId) };

            var endpoint = FormatPath("apps/{0}/lobbies/{1}/owner", _session.AppId, ValidateLobbyId(lobbyId));
            var request = Request.Put(_session.BaseUrl, endpoint, body).WithAuth(_auth);
            var response = await request.Send();

            response.ThrowIfError();
        }

        /// <summary>
        /// Changes the name of a lobby.
        /// </summary>
        /// <remarks>
        /// The user associated with the current session must be the lobby owner to set the name.
        /// </remarks>
        /// <param name="lobbyId">The ID of the lobby.</param>
        /// <param name="name">The new name of the lobby.</param>
        public async ValueTask SetLobbyName(string lobbyId, string name)
        {
            await SetLobbyProperties(lobbyId, new SetLobbyOptions { Name = name });
        }

        /// <summary>
        /// Changes the capacity of a lobby.
        /// </summary>
        /// <remarks>
        /// The user associated with the current session must be the lobby owner to set the capacity.
        /// Reducing the capacity of the lobby will not kick any excess members.
        /// </remarks>
        /// <param name="lobbyId">The ID of the lobby.</param>
        /// <param name="capacity">The new capacity of the lobby.</param>
        public async ValueTask SetLobbyCapacity(string lobbyId, int capacity)
        {
            await SetLobbyProperties(lobbyId, new SetLobbyOptions { Capacity = capacity });
        }

        /// <summary>
        /// Changes the tags of a lobby.
        /// </summary>
        /// <remarks>
        /// The user associated with the current session must be the lobby owner to set the tags.
        /// </remarks>
        /// <param name="lobbyId">The ID of the lobby.</param>
        /// <param name="tags">The new tags of the lobby.</param>
        public async ValueTask SetLobbyTags(string lobbyId, string[] tags)
        {
            await SetLobbyProperties(lobbyId, new SetLobbyOptions { Tags = tags });
        }

        /// <summary>
        /// Changes the custom data of a lobby.
        /// </summary>
        /// <remarks>
        /// The user associated with the current session must be a member of the lobby to set the data.
        /// To add, change, or remove individual keys from the custom data, use <see cref="UpdateLobbyData"/>.
        /// </remarks>
        /// <param name="lobbyId">The ID of the lobby.</param>
        /// <param name="data">The new data for the lobby.</param>
        /// <seealso cref="UpdateLobbyData"/>
        public async ValueTask SetLobbyData(string lobbyId, LobbyDataContainer data)
        {
            await SetLobbyProperties(lobbyId, new SetLobbyOptions { Data = data });
        }

        /// <summary>
        /// Changes the properties of a lobby.
        /// </summary>
        /// <remarks>
        /// The user associated with the current session must be the lobby owner to set the name, size, or tags,
        /// or be a member of the lobby to set the data.
        /// </remarks>
        /// <param name="lobbyId">The ID of the lobby.</param>
        /// <param name="options">The new properties of the lobby. Any options passed here will replace the existing
        /// property of the lobby. To leave a property as is, leave the value as <see langword="null"/>.</param>
        public async ValueTask SetLobbyProperties(string lobbyId, SetLobbyOptions options)
        {
            _session.CheckDisposed();
            
            var body = new ModifyLobbyRequestBody
            {
                Capacity = options.Capacity != null ? ValidateCapacity(options.Capacity.Value) : null,
                Name = options.Name != null ? ValidateName(options.Name.Value.Value) : null,
                Tags = options.Tags != null ? ValidateTags(options.Tags) : null,
                Data = options.Data != null ? ValidateDataContainer(options.Data) : null,
            };

            var endpoint = FormatPath("apps/{0}/lobbies/{1}", _session.AppId, lobbyId);
            var request = Request.Put(_session.BaseUrl, endpoint, body).WithAuth(_auth);
            var response = await request.Send();

            response.ThrowIfError();
        }

        /// <summary>
        /// Changes the custom data of a lobby.
        /// </summary>
        /// <remarks>
        /// The user associated with the current session must be a member of the lobby.
        /// </remarks>
        /// <param name="lobbyId">The ID of the lobby.</param>
        /// <param name="update">The changes to make to the lobby data.</param>
        /// <returns>The lobby object.</returns>
        public async ValueTask<LobbyObject> UpdateLobbyData(string lobbyId, LobbyDataUpdate update)
        {
            _session.CheckDisposed();

            var body = new UpdateLobbyDataRequestBody
            {
                Data = ValidateDataUpdate(update),
            };

            var endpoint = FormatPath("apps/{0}/lobbies/{1}/data", _session.AppId, ValidateLobbyId(lobbyId));
            var request = Request.Post(_session.BaseUrl, endpoint, body).WithAuth(_auth);
            var response = await request.Send();

            return response.ParseDataResponse<LobbyObject>();
        }

        /// <summary>
        /// Add the currently authenticated user to a lobby.
        /// </summary>
        /// <param name="lobbyId">The ID of the lobby.</param>
        public async ValueTask<LobbyObject> JoinLobby(string lobbyId)
        {
            _session.CheckDisposed();
            
            var endpoint = FormatPath("apps/{0}/lobbies/{1}/members", _session.AppId, ValidateLobbyId(lobbyId));
            var request = Request.Post(_session.BaseUrl, endpoint).WithAuth(_auth);
            var response = await request.Send();

            return response.ParseDataResponse<LobbyObject>();
        }

        /// <summary>
        /// Remove the currently authenticated user from a lobby.
        /// </summary>
        /// <remarks>
        /// The user associated with the current session must be a member of the lobby.
        /// </remarks>
        /// <param name="lobbyId">The ID of the lobby.</param>
        public async ValueTask LeaveLobby(string lobbyId)
        {
            _session.CheckDisposed();
            
            if (_auth is not IUserAuthentication userAuth)
            {
                throw new System.NotImplementedException();
            }

            var endpoint = FormatPath(
                "apps/{0}/lobbies/{1}/members/{2}",
                _session.AppId,
                ValidateLobbyId(lobbyId),
                userAuth.UserId
            );
            var request = Request.Delete(_session.BaseUrl, endpoint).WithAuth(_auth);
            var response = await request.Send();

            response.ThrowIfError();
        }

        /// <summary>
        /// Close a lobby.
        /// </summary>
        /// <remarks>
        /// The user associated with the current session must be the lobby owner of the lobby.
        /// </remarks>
        /// <param name="lobbyId">The ID of the lobby.</param>
        public async ValueTask CloseLobby(string lobbyId)
        {
            _session.CheckDisposed();
            
            var endpoint = FormatPath("apps/{0}/lobbies/{1}", _session.AppId, ValidateLobbyId(lobbyId));
            var request = Request.Delete(_session.BaseUrl, endpoint).WithAuth(_auth);
            var response = await request.Send();

            response.ThrowIfError();
        }

        /// <summary>
        /// Get the members of a lobby.
        /// </summary>
        /// <param name="lobbyId">The ID of the lobby.</param>
        /// <returns>A new array of lobby members.</returns>
        public async ValueTask<LobbyMemberObject[]> GetLobbyMembers(string lobbyId)
        {
            _session.CheckDisposed();
            
            var endpoint = FormatPath("apps/{0}/lobbies/{1}/members", _session.AppId, ValidateLobbyId(lobbyId));
            var request = Request.Get(_session.BaseUrl, endpoint).WithAuth(_auth);
            var response = await request.Send();

            return response.ParseDataResponse<LobbyMemberObject[]>();
        }

        /// <summary>
        /// Get the properties of a lobby member.
        /// </summary>
        /// <param name="lobbyId">The ID of the lobby.</param>
        /// <param name="userId">The ID of the user.</param>
        public async ValueTask<LobbyMemberObject> GetLobbyMember(string lobbyId, string userId)
        {
            _session.CheckDisposed();
            
            var endpoint = FormatPath(
                "apps/{0}/lobbies/{1}/members/{2}",
                _session.AppId,
                ValidateLobbyId(lobbyId),
                ValidateUserId(userId)
            );
            var request = Request.Get(_session.BaseUrl, endpoint).WithAuth(_auth);
            var response = await request.Send();

            return response.ParseDataResponse<LobbyMemberObject>();
        }

        /// <summary>
        /// Replaces the custom data of the currently authenticated user for a lobby.
        /// </summary>
        /// <remarks>
        /// To add, change, or remove individual keys from the custom data, use <see cref="UpdateLobbyMemberData"/>.
        /// </remarks>
        /// <param name="lobbyId">The ID of the lobby.</param>
        /// <param name="data">The new data for the user.</param>
        /// <seealso cref="UpdateLobbyMemberData"/>
        public async ValueTask SetLobbyMemberData(string lobbyId, LobbyDataContainer data)
        {
            _session.CheckDisposed();
            
            var body = new ModifyLobbyMemberRequestBody
            {
                Data = ValidateDataContainer(data),
            };

            if (_auth is not IUserAuthentication userAuth)
            {
                throw new System.NotImplementedException();
            }

            var endpoint = FormatPath(
                "apps/{0}/lobbies/{1}/members/{2}",
                _session.AppId,
                ValidateLobbyId(lobbyId),
                userAuth.UserId
            );
            var request = Request.Put(_session.BaseUrl, endpoint, body).WithAuth(_auth);
            var response = await request.Send();

            response.ThrowIfError();
        }
        
        /// <summary>
        /// Changes the custom data of the currently authenticated user for a lobby.
        /// </summary>
        /// <param name="lobbyId">The ID of the lobby.</param>
        /// <param name="update">The changes to make to the lobby member data.</param>
        /// <returns>The lobby member object.</returns>
        public async ValueTask<LobbyMemberObject> UpdateLobbyMemberData(string lobbyId, LobbyDataUpdate update)
        {
            _session.CheckDisposed();

            var body = new UpdateLobbyDataRequestBody
            {
                Data = ValidateDataUpdate(update),
            };

            if (_auth is not IUserAuthentication userAuth)
            {
                throw new System.NotImplementedException();
            }

            var endpoint = FormatPath(
                "apps/{0}/lobbies/{1}/members/{2}/data",
                _session.AppId,
                ValidateLobbyId(lobbyId),
                userAuth.UserId
            );
            var request = Request.Post(_session.BaseUrl, endpoint, body).WithAuth(_auth);
            var response = await request.Send();

            return response.ParseDataResponse<LobbyMemberObject>();
        }

        /// <summary>
        /// Remove a user from a lobby.
        /// </summary>
        /// <remarks>
        /// The user associated with the current session must be the lobby owner of the lobby to remove
        /// other lobby members.
        /// </remarks>
        /// <param name="lobbyId">The ID of the lobby.</param>
        /// <param name="userId">The ID of the user.</param>
        public async ValueTask RemoveLobbyMember(string lobbyId, string userId)
        {
            _session.CheckDisposed();
            
            var endpoint = FormatPath(
                "apps/{0}/lobbies/{1}/members/{2}",
                _session.AppId,
                ValidateLobbyId(lobbyId),
                ValidateUserId(userId)
            );
            var request = Request.Delete(_session.BaseUrl, endpoint).WithAuth(_auth);
            var response = await request.Send();

            response.ThrowIfError();
        }
    }
}
