using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when re-authenticating with the Normcore Services API
    /// as an anonymous user and the provided secret is invalid.
    /// </summary>
    /// <seealso cref="ErrorCode.Unauthenticated"/>
    public sealed class UnauthenticatedException : ErrorResponseException
    {
        internal UnauthenticatedException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}