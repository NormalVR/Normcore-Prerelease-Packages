using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails because the
    /// requested lobby owner was not found.
    /// </summary>
    /// <seealso cref="ErrorCode.LobbyOwnerNotFound"/>
    public sealed class LobbyOwnerNotFoundException : ErrorResponseException
    {
        internal LobbyOwnerNotFoundException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}