using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails because the
    /// requested lobby already exists.
    /// </summary>
    /// <seealso cref="ErrorCode.LobbyAlreadyExists"/>
    public sealed class LobbyAlreadyExistsException : ErrorResponseException
    {
        internal LobbyAlreadyExistsException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}