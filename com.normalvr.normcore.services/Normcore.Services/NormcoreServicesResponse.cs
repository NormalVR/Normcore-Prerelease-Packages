using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Normcore.Services
{
    internal struct NormcoreServicesResponse
    {
        /// <summary>
        /// The response HTTP status code.
        /// </summary>
        public uint Status;

        /// <summary>
        /// The raw response JSON, or null if the response body was empty.
        /// </summary>
        public string Body;
        
        internal NormcoreServicesException.RequestInfo RequestInfo;
        
        public static NormcoreServicesResponse FromUnityWebRequest(UnityWebRequest req)
        {
            Debug.Assert(req.isDone);

            return new NormcoreServicesResponse
            {
                Status = (uint)req.responseCode,
                Body = req.downloadHandler?.text,
                RequestInfo = new NormcoreServicesException.RequestInfo(req),
            };
        }

        [Serializable]
        private struct DataResponse<T>
        {
            public T data;
        }

        [Serializable]
        private struct ErrorResponse
        {
            public string error;
        }

        /// <summary>
        /// Parse the response JSON into a data response.
        /// </summary>
        /// <typeparam name="T">The type of the "data" value in the JSON. Must be serializable.</typeparam>
        /// <returns>The "data" value of the JSON, parsed into type T.</returns>
        public T ParseDataResponse<T>()
        {
            // The status codes are technically overly restrictive but true for our responses.

            Debug.Assert(Status == 200 || Status == 204, "The response status must be a success status.");
            Debug.Assert(!string.IsNullOrEmpty(Body), "The response body must not be empty.");

            return JSON.Deserialize<DataResponse<T>>(Body).data;
        }

        /// <summary>
        /// Parse the response JSON into an error response.
        /// </summary>
        /// <returns>The "error" value of the JSON.</returns>
        public string ParseErrorResponse()
        {
            // The status codes are technically overly restrictive but true for our responses.

            Debug.Assert(Status != 200 && Status != 204, "The response status must not be a success status.");
            Debug.Assert(!string.IsNullOrEmpty(Body), "The response body must not be empty.");

            return JSON.Deserialize<ErrorResponse>(Body).error;
        }
    }
}
