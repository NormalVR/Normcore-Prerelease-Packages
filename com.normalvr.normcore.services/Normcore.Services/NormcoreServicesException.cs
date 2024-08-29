using System;

using UnityEngine.Networking;

namespace Normcore.Services
{
    public class NormcoreServicesException : Exception
    {
        internal struct RequestInfo
        {
            public string method;
            public string url;
            public string error;

            public RequestInfo(UnityWebRequest request)
            {
                method = request.method;
                url = request.url;
                error = request.error;
            }

            public string Format(string message = "")
            {
                var result = string.Empty;
                
                if (!string.IsNullOrEmpty(message))
                {
                    result = message;
                }
                
                if (!string.IsNullOrEmpty(method) && !string.IsNullOrEmpty(url))
                {
                    result += $"\n\n[{method}: {url}] ";
                }

                if (!string.IsNullOrEmpty(error))
                {
                    result += $"{error} ";
                }

                result += "\n";

                return result;
            }
        }

        private NormcoreServicesException(RequestInfo requestInfo, string message = "") : base(requestInfo.Format(message))
        {
        }

        internal static NormcoreServicesException ConnectionError(RequestInfo request)
        {
            return new NormcoreServicesException(request, "Unable to connect to Normcore Services backend.");
        }

        internal static NormcoreServicesException DataProcessingError(RequestInfo request)
        {
            return new NormcoreServicesException(request, "Unable to process response from Normcore Services backend.");
        }

        internal static NormcoreServicesException UnexpectedResponse(NormcoreServicesResponse resp)
        {
            return new NormcoreServicesException(resp.RequestInfo, $"Unexpected response from the Normcore Services API: {resp.Status}");
        }
    }
}
