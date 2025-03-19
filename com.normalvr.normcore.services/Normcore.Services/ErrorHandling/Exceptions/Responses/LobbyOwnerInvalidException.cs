using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails because the
    /// requested lobby owner was not found in the lobby's members.
    /// </summary>
    /// <seealso cref="ErrorCode.LobbyOwnerInvalid"/>
    public sealed class LobbyOwnerInvalidException : ErrorResponseException
    {
        internal LobbyOwnerInvalidException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}