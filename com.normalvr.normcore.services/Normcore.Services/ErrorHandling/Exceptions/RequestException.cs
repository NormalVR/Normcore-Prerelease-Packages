using System;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails.
    /// </summary>
    public class RequestException : Exception
    {
        internal RequestException(in RequestInfo request, string message = "") : base(Format(request, message))
        {
        }

        private static string Format(in RequestInfo request, string message)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(message))
            {
                result = message;
            }

            if (!string.IsNullOrEmpty(request.method) && !string.IsNullOrEmpty(request.url))
            {
                result += $"\n\n[{request.method}: {request.url}] ";
            }

            if (!string.IsNullOrEmpty(request.error))
            {
                result += $"{request.error} ";
            }

            result += "\n";

            return result;
        }
    }
}
