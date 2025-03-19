using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails because the
    /// requested lobby was not found.
    /// </summary>
    /// <seealso cref="ErrorCode.LobbyNotFound"/>
    public sealed class LobbyNotFoundException : ErrorResponseException
    {
        internal LobbyNotFoundException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}