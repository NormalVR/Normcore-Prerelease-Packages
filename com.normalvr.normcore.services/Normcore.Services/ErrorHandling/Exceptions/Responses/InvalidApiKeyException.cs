using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when authenticating with the Normcore Services API
    /// using an API key, and the key is not provided, has an invalid format, or
    /// is not valid for the app.
    /// </summary>
    /// <seealso cref="ErrorCode.InvalidApiKey"/>
    public sealed class InvalidApiKeyException : ErrorResponseException
    {
        internal InvalidApiKeyException(in RequestInfo requestInfo, HttpStatusCode status, Error error)
            : base(requestInfo, status, error)
        {
        }
    }
}