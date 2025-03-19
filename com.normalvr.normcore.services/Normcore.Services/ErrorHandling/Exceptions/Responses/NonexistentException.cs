using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails because the
    /// requested resource does not exist.
    /// </summary>
    /// <seealso cref="ErrorCode.Nonexistent"/>
    public sealed class NonexistentException : ErrorResponseException
    {
        internal NonexistentException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}