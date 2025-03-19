namespace Normcore.Services
{
    /// <summary>
    /// A class containing the error codes returned by the services backend.
    /// </summary>
    public static class ErrorCode
    {
        /// <summary>
        /// Returned when the backend encountered an unexpected error.
        /// </summary>
        /// <seealso cref="InternalServerErrorException"/>
        public const string InternalServerError = "INTERNAL_SERVER_ERROR";
        /// <summary>
        /// Returned when the backend does not implement the operation.
        /// </summary>
        /// <seealso cref="NotImplementedException"/>
        public const string NotImplemented = "NOT_IMPLEMENTED";
        
        /// <summary>
        /// Returned if the request was malformed.
        /// </summary>
        /// <seealso cref="BadRequestException"/>
        public const string BadRequest = "BAD_REQUEST";
        /// <summary>
        /// Returned if the request contained invalid parameters or is missing required parameters. 
        /// </summary>
        /// <seealso cref="InvalidSchemaException"/>
        public const string InvalidSchema = "INVALID_SCHEMA";

        /// <summary>
        /// Returned when authenticating using an API key, and the key is not provided,
        /// has an invalid format, or is not valid for the app. 
        /// </summary>
        /// <seealso cref="InvalidApiKeyException"/>
        public const string InvalidApiKey = "INVALID_API_KEY";
        /// <summary>
        /// Returned when re-authenticating as an anonymous user and the provided secret is invalid.
        /// </summary>
        /// <seealso cref="UnauthenticatedException"/>
        public const string Unauthenticated = "UNAUTHENTICATED";
        /// <summary>
        /// Returned if the authenticated user does not have the required permission level to access the resource.
        /// </summary>
        /// <seealso cref="NotPermittedException"/>
        public const string NotPermitted = "NOT_PERMITTED";

        /// <summary>
        /// Returned if the requested resource does not exist.
        /// </summary>
        /// <seealso cref="NonexistentException"/>
        public const string Nonexistent = "NONEXISTENT";
        
        /// <summary>
        /// Returned when the requested lobby was not found.
        /// </summary>
        /// <seealso cref="LobbyNotFoundException"/>
        public const string LobbyNotFound = "LOBBY_NOT_FOUND";
        /// <summary>
        /// Returned when the requested lobby already exists.
        /// </summary>
        /// <seealso cref="LobbyAlreadyExistsException"/>
        public const string LobbyAlreadyExists = "LOBBY_ALREADY_EXISTS";
        /// <summary>
        /// Returned when the lobby query is invalid.
        /// </summary>
        /// <seealso cref="LobbyInvalidQueryException"/>
        public const string LobbyInvalidQuery = "LOBBY_INVALID_QUERY";
        /// <summary>
        /// Returned when the requested lobby member was not found.
        /// </summary>
        /// <seealso cref="LobbyMemberNotFoundException"/>
        public const string LobbyMemberNotFound = "LOBBY_MEMBER_NOT_FOUND";
        /// <summary>
        /// Returned when the lobby was already full.
        /// </summary>
        /// <seealso cref="LobbyMemberCapacityExceededException"/>
        public const string LobbyMemberCapacityExceeded = "LOBBY_MEMBER_CAPACITY_EXCEEDED";
        /// <summary>
        /// Returned when the requested lobby owner was not found.
        /// </summary>
        /// <seealso cref="LobbyOwnerNotFoundException"/>
        public const string LobbyOwnerNotFound = "LOBBY_OWNER_NOT_FOUND";
        /// <summary>
        /// Returned when the requested lobby owner was not found in the lobby's members.
        /// </summary>
        /// <seealso cref="LobbyOwnerInvalidException"/>
        public const string LobbyOwnerInvalid = "LOBBY_OWNER_INVALID";
        
        /// <summary>
        /// Returned when the requested user was not found.
        /// </summary>
        /// <seealso cref="UserNotFoundException"/>
        public const string UserNotFound = "USER_NOT_FOUND";
    }
}
