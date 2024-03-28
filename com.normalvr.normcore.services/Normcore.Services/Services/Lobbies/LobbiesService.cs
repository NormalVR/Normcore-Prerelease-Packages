using System.Threading.Tasks;

namespace Normcore.Services
{
    public struct LobbiesService
    {
        private AuthenticatedContext auth;

        internal LobbiesService(AuthenticatedContext auth)
        {
            this.auth = auth;
        }

        /// <summary>
        /// Create a lobby.
        /// </summary>
        /// <returns>The lobby properties.</returns>
        public async ValueTask<LobbyObject> CreateLobby(uint size, string[] tags, LobbyDataContainer data)
        {
            var body = new CreateLobbyRequestBody();

            body.Size = size;
            body.Tags = tags;
            body.Data = data;

            var response = await NormcoreServicesRequest.Post("lobbies", body).WithAuth(auth).Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<LobbyObject>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        /// <summary>
        /// Get the properties of a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        public async ValueTask<LobbyObject> GetLobby(string lobbyID)
        {
            var response = await NormcoreServicesRequest.Get($"lobbies/{lobbyID}").WithAuth(auth).Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<LobbyObject>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        /// <summary>
        /// Get the most recently created lobbies with no additional filters.
        /// </summary>
        /// <returns>A list of lobby objects.</returns>
        public async ValueTask<LobbyObject[]> GetLobbies()
        {

            var response = await NormcoreServicesRequest.Get($"lobbies").WithAuth(auth).Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<LobbyObject[]>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        /// <summary>
        /// Get the members of a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        /// <returns></returns>
        public async ValueTask<UserObject[]> GetLobbyMembers(string lobbyID)
        {
            var response = await NormcoreServicesRequest.Get($"lobbies/{lobbyID}/members").WithAuth(auth).Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<UserObject[]>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        /// <summary>
        /// Get the owner of a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        /// <returns>The user ID of the lobby owner.</returns>
        public async ValueTask<string> GetOwner(string lobbyID)
        {
            var response = await NormcoreServicesRequest.Get($"lobbies/{lobbyID}/owner").WithAuth(auth).Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<UserObject>().ID;
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        /// <summary>
        /// Set the owner of a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        /// <param name="userID">The ID of the new owner.</param>
        public async ValueTask SetOwner(string lobbyID, string userID)
        {
            var body = new SetLobbyOwnerRequestBody { ID = userID };

            var response = await NormcoreServicesRequest.Put($"lobbies/{lobbyID}/owner", body).WithAuth(auth).Send();

            if (response.Status == 204)
            {
                return;
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        /// <summary>
        /// Set the properties of a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        public async ValueTask ModifyLobby(string lobbyID)
        {
            var body = new ModifyLobbyRequestBody();

            var response = await NormcoreServicesRequest.Put($"lobbies/{lobbyID}", body).WithAuth(auth).Send();

            if (response.Status == 204)
            {
                return;
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        /// <summary>
        /// Add the currently authenticated user to a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        public async ValueTask<LobbyObject> JoinLobby(string lobbyID)
        {
            var response = await NormcoreServicesRequest.Post($"lobbies/{lobbyID}/members").WithAuth(auth).Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<LobbyObject>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        public async ValueTask<LobbyObject[]> QueryLobbies(LobbyQuery query)
        {
            var response = await NormcoreServicesRequest.Post("lobbies/query", query).WithAuth(auth).Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<LobbyObject[]>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        /// <summary>
        /// Remove the currently authenticated user from a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        public async ValueTask LeaveLobby(string lobbyID)
        {
            var response = await NormcoreServicesRequest.Delete($"lobbies/{lobbyID}/members/{auth.UserID}").WithAuth(auth).Send();

            if (response.Status == 204)
            {
                return;
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        /// <summary>
        /// Close a lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        public async ValueTask CloseLobby(string lobbyID)
        {
            var response = await NormcoreServicesRequest.Delete($"lobbies/{lobbyID}").WithAuth(auth).Send();

            if (response.Status == 204)
            {
                return;
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        /// <summary>
        /// Remove another member from the lobby.
        /// </summary>
        /// <param name="lobbyID">The ID of the lobby.</param>
        /// <param name="userID">The ID of the user to remove.</param>
        public async ValueTask RemoveMember(string lobbyID, string userID)
        {
            var response = await NormcoreServicesRequest.Delete($"lobbies/{lobbyID}/members/{userID}").WithAuth(auth).Send();

            if (response.Status == 204)
            {
                return;
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        /// <summary>
        /// Get the list of lobbies where the client is a member.
        /// </summary>
        /// <returns></returns>
        public async ValueTask<LobbyObject[]> GetJoinedLobbies()
        {
            var response = await NormcoreServicesRequest.Get($"lobbies/joined").WithAuth(auth).Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<LobbyObject[]>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }

        public async ValueTask<LobbyObject> UpdateLobbyData(string lobbyID, LobbyDataUpdate update)
        {
            var body = new UpdateLobbyDataRequestBody();

            body.Data = update;

            var response = await NormcoreServicesRequest.Post($"lobbies/{lobbyID}/data", body).WithAuth(auth).Send();

            if (response.Status == 200)
            {
                return response.ParseDataResponse<LobbyObject>();
            }

            // TODO handle expected failure responses

            throw NormcoreServicesException.UnexpectedResponse(response);
        }
    }
}
