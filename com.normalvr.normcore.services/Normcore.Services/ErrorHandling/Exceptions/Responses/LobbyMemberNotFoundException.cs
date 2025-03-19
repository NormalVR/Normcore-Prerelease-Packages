using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails because the
    /// requested lobby member was not found.
    /// </summary>
    /// <seealso cref="ErrorCode.LobbyMemberNotFound"/>
    public sealed class LobbyMemberNotFoundException : ErrorResponseException
    {
        internal LobbyMemberNotFoundException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}