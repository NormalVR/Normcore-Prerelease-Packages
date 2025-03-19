using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails because the
    /// backend does not implement the operation.
    /// </summary>
    /// <seealso cref="ErrorCode.NotImplemented"/>
    public sealed class NotImplementedException : ErrorResponseException
    {
        internal NotImplementedException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}
