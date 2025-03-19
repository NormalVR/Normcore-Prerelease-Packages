using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails because the
    /// requested user was not found.
    /// </summary>
    /// <seealso cref="ErrorCode.UserNotFound"/>
    public sealed class UserNotFoundException : ErrorResponseException
    {
        internal UserNotFoundException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}