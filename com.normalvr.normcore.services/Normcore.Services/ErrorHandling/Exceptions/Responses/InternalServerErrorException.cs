using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails because the
    /// backend encountered an unexpected error.
    /// </summary>
    /// <seealso cref="ErrorCode.InternalServerError"/>
    public sealed class InternalServerErrorException : ErrorResponseException
    {
        internal InternalServerErrorException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}
