using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails because the
    /// lobby was already full.
    /// </summary>
    /// <seealso cref="ErrorCode.LobbyMemberCapacityExceeded"/>
    public sealed class LobbyMemberCapacityExceededException : ErrorResponseException
    {
        internal LobbyMemberCapacityExceededException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}