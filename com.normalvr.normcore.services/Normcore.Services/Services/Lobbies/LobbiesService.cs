using System;
using System.Threading.Tasks;
using static Normcore.Services.Validation;

namespace Normcore.Services
{
    public class LobbiesService
    {
        private IAuthentication auth;
        private string appKey;

        internal LobbiesService(IAuthentication auth, string appKey)
        {
            this.auth = auth;
            this.appKey = appKey;
        }

        /// <summary>
        /// Create a lobby.
        /// </summary>
        /// <returns>The lobby properties.</returns>
        public async ValueTask<LobbyObject> CreateLobby(uint size, string[] tags, LobbyDataContainer data, string name)
        {
            var body = new CreateLobbyRequestBody();

            body.Name = name;
            body.Size = size;
            body.Tags = tags;
            body.Data = data;

            var endpoint = FormatPath("apps/{0}/lobbies", appKey);
            var request = NormcoreServicesRequest.Post(endpoint, body).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<LobbyObject>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        /// <summary>
        /// Get the properties of a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        public async ValueTask<LobbyObject> GetLobby(string lobbyID)
        {
            var endpoint = FormatPath("apps/{0}/lobbies/{1}", appKey, lobbyID);
            var request = NormcoreServicesRequest.Get(endpoint).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<LobbyObject>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        /// <summary>
        /// Get the properties of a lobby by name (if set).
        /// </summary>
        /// <param name="name">The name of the lobby.</param>
        public async ValueTask<LobbyObject> GetLobbyByName(string name)
        {
            var endpoint = FormatPath("lobbies/name/{0}", name);
            var request = NormcoreServicesRequest.Get(endpoint).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 200)
            {
                var lobby = response.ParseDataResponse<LobbyObject>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        /// <summary>
        /// Get the most recently created lobbies with no additional filters.
        /// </summary>
        /// <returns>A list of lobby objects.</returns>
        public async ValueTask<LobbyObject[]> GetLobbies()
        {
            var endpoint = FormatPath("apps/{0}/lobbies", appKey);
            var request = NormcoreServicesRequest.Get(endpoint).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<LobbyObject[]>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        /// <summary>
        /// Get the members of a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        /// <returns></returns>
        public async ValueTask<UserObject[]> GetLobbyMembers(string lobbyID)
        {
            var endpoint = FormatPath("apps/{0}/lobbies/{1}/members", appKey, lobbyID);
            var request = NormcoreServicesRequest.Get(endpoint).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<UserObject[]>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        /// <summary>
        /// Get the owner of a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        /// <returns>The user ID of the lobby owner.</returns>
        public async ValueTask<string> GetOwner(string lobbyID)
        {
            var endpoint = FormatPath("apps/{0}/lobbies/{1}/owner", appKey, lobbyID);
            var request = NormcoreServicesRequest.Get(endpoint).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<UserObject>().ID;
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        /// <summary>
        /// Set the owner of a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        /// <param name="userID">The ID of the new owner.</param>
        public async ValueTask SetOwner(string lobbyID, string userID)
        {
            var body = new SetLobbyOwnerRequestBody { ID = userID };

            var endpoint = FormatPath("apps/{0}/lobbies/{1}/owner", appKey, lobbyID);
            var request = NormcoreServicesRequest.Put(endpoint, body).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 204)
            {
                return;
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        /// <summary>
        /// Replace the name of a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        /// <param name="name">The new name of the lobby.</param>
        public async ValueTask SetLobbyName(string lobbyID, string name)
        {
            await SetLobbyProperties(lobbyID, new SetLobbyOptions { Name = name });
        }

        /// <summary>
        /// Replace the size of a lobby.
        /// </summary>
        /// <remarks>
        /// Reducing the size of the lobby will not kick any excess members.
        /// </remarks>
        /// <param name="lobbyID">The ID of the lobby.</param>
        /// <param name="size">The new size of the lobby.</param>
        public async ValueTask SetLobbySize(string lobbyID, uint size)
        {
            await SetLobbyProperties(lobbyID, new SetLobbyOptions { Size = size });
        }

        /// <summary>
        /// Replace the tags of a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        /// <param name="tags">The new list of lobby tags.</param>
        public async ValueTask SetLobbyTags(string lobbyID, string[] tags)
        {
            await SetLobbyProperties(lobbyID, new SetLobbyOptions { Tags = tags });
        }

        /// <summary>
        /// Replace the custom data of a lobby.
        /// </summary>
        /// <remarks>
        /// To add or remove individual keys from the custom data, use UpdateLobbyData.
        /// </remarks>
        /// <param name="lobbyID">The ID of the lobby.</param>
        /// <param name="data">The new custom data of the lobby.</param>
        public async ValueTask SetLobbyData(string lobbyID, LobbyDataContainer data)
        {
            await SetLobbyProperties(lobbyID, new SetLobbyOptions { Data = data });
        }

        /// <summary>
        /// Replace multiple properties of a lobby.
        /// </summary>
        /// <remarks>
        /// Any options passed here will replace the existing property of the lobby. To leave a property as is, leave the value as null.
        /// </remarks>
        /// <param name="lobbyID">The ID of the lobby.</param>
        public async ValueTask SetLobbyProperties(string lobbyID, SetLobbyOptions options)
        {
            var body = new ModifyLobbyRequestBody
            {
                Name = options.Name,
                Size = options.Size,
                Tags = options.Tags,
                Data = options.Data
            };

            var endpoint = FormatPath("apps/{0}/lobbies/{1}", appKey, lobbyID);
            var request = NormcoreServicesRequest.Put(endpoint, body).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 204)
            {
                return;
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        /// <summary>
        /// Add the currently authenticated user to a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        public async ValueTask<LobbyObject> JoinLobby(string lobbyID)
        {
            var endpoint = FormatPath("apps/{0}/lobbies/{1}/members", appKey, lobbyID);
            var request = NormcoreServicesRequest.Post(endpoint).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<LobbyObject>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        public async ValueTask<LobbyObject[]> QueryLobbies(LobbyQuery query)
        {
            var endpoint = FormatPath("apps/{0}/lobbies/query", appKey);
            var request = NormcoreServicesRequest.Post(endpoint, query).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<LobbyObject[]>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        /// <summary>
        /// Remove the currently authenticated user from a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        public async ValueTask LeaveLobby(string lobbyID)
        {
            if (auth is not UserAuthentication uauth)
            {
                throw new NotImplementedException();
            }

            var endpoint = FormatPath(
                "apps/{0}/lobbies/{1}/members/{2}",
                appKey,
                lobbyID,
                uauth.UserID
            );
            var request = NormcoreServicesRequest.Delete(endpoint).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 204)
            {
                return;
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        /// <summary>
        /// Close a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        public async ValueTask CloseLobby(string lobbyID)
        {
            var endpoint = FormatPath("apps/{0}/lobbies/{1}", appKey, lobbyID);
            var request = NormcoreServicesRequest.Delete(endpoint).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 204)
            {
                return;
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        /// <summary>
        /// Remove another member from the lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        /// <param name="userID">The ID of the user to remove.</param>
        public async ValueTask RemoveMember(string lobbyID, string userID)
        {
            var endpoint = FormatPath("apps/{0}/lobbies/{1}/members/{2}", appKey, lobbyID, userID);
            var request = NormcoreServicesRequest.Delete(endpoint).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 204)
            {
                return;
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        /// <summary>
        /// Get the list of lobbies where the client is a member.
        /// </summary>
        /// <returns></returns>
        public async ValueTask<LobbyObject[]> GetJoinedLobbies()
        {
            var endpoint = FormatPath("apps/{0}/lobbies/joined", appKey);
            var request = NormcoreServicesRequest.Get(endpoint).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<LobbyObject[]>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }

        public async ValueTask<LobbyObject> UpdateLobbyData(string lobbyID, LobbyDataUpdate update)
        {
            var body = new UpdateLobbyDataRequestBody();

            body.Data = update;

            var endpoint = FormatPath("apps/{0}/lobbies/{1}/data", appKey, lobbyID);
            var request = NormcoreServicesRequest.Post(endpoint, body).WithAuth(auth);
            var response = await request.Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<LobbyObject>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(request, response);
        }
    }
}
