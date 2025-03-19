using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails because the
    /// lobby query is invalid.
    /// </summary>
    /// <seealso cref="ErrorCode.LobbyInvalidQuery"/>
    public sealed class LobbyInvalidQueryException : ErrorResponseException
    {
        internal LobbyInvalidQueryException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}