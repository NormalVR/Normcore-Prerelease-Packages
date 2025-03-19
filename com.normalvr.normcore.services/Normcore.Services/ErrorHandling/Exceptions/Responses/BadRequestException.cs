using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails because the
    /// request was malformed.
    /// </summary>
    /// <seealso cref="ErrorCode.BadRequest"/>
    public sealed class BadRequestException : ErrorResponseException
    {
        internal BadRequestException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}