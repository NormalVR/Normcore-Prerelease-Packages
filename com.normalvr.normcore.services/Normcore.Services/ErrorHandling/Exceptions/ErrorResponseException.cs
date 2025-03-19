using System.Net;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails and the server responded with an error.
    /// </summary>
    public class ErrorResponseException : RequestException
    {
        /// <summary>
        /// The HTTP code returned by the server.
        /// </summary>
        public HttpStatusCode Code { get; }
        
        /// <summary>
        /// A human-readable message describing the error.
        /// </summary>
        public string Error { get; }
        
        internal ErrorResponseException(
            in RequestInfo request,
            HttpStatusCode status,
            Error error,
            string details = null
        )
            : base(request, Format(error) + $"\n{details ?? string.Empty}")
        {
            Code = status;
            Error = error.Message;
        }

        private static string Format(Error error)
        {
            return $"Received {error.Code} error from the Normcore Services API: {error.Message}";
        }
    }
}
