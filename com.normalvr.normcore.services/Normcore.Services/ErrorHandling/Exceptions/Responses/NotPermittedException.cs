using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails because the
    /// authenticated user does not have the required permission level to access the resource.
    /// </summary>
    /// <seealso cref="ErrorCode.NotPermitted"/>
    public sealed class NotPermittedException : ErrorResponseException
    {
        internal NotPermittedException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}