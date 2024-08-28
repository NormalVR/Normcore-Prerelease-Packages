using System;

namespace Normcore.Services
{
    public class NormcoreServicesException : Exception
    {
        internal struct RequestInfo
        {
            public string method;
            public string error;
            public string url;

            public string Format(string additionalMessage = "")
            {
                var result = $"[{method}: {url}]";

                if (!string.IsNullOrEmpty(error))
                {
                    result += $" {error}";
                }

                if (!string.IsNullOrEmpty(additionalMessage))
                {
                    result += $" - {additionalMessage}";
                }

                return result;
            }
        }

        private NormcoreServicesException(RequestInfo requestInfo, string message = "") : base(requestInfo.Format(message))
        {

        }

        internal static NormcoreServicesException ConnectionError(NormcoreServicesRequest request)
        {
            return new NormcoreServicesException(request.GetExceptionInfo(), "Unable to connect to Normcore Services backend.");
        }

        internal static NormcoreServicesException DataProcessingError(NormcoreServicesRequest request)
        {
            return new NormcoreServicesException(request.GetExceptionInfo(), "Unable to process response from Normcore Services backend.");
        }

        internal static NormcoreServicesException UnexpectedResponse(NormcoreServicesRequest request, NormcoreServicesResponse resp)
        {
            return new NormcoreServicesException(request.GetExceptionInfo(), $"Unexpected response from the Normcore Services API: {resp.Status}");
        }
    }
}
