using System;

namespace Normcore.Services
{
    public class NormcoreServicesException : Exception
    {
        private NormcoreServicesException(string message) : base(message)
        {

        }

        internal static NormcoreServicesException ConnectionError()
        {
            return new NormcoreServicesException("Unable to connect to Normcore Services backend.");
        }

        internal static NormcoreServicesException DataProcessingError()
        {
            return new NormcoreServicesException("Unable to process response from Normcore Services backend.");
        }

        internal static NormcoreServicesException UnexpectedResponse(NormcoreServicesResponse resp)
        {
            return new NormcoreServicesException($"Unexpected response from the Normcore Services API: {resp.Status}");
        }
    }
}
