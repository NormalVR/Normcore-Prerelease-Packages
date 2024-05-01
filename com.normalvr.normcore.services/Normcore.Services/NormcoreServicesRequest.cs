using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

#if ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Normcore.Services
{
    internal struct NormcoreServicesRequest
    {
        private const string JSONContentType = "application/json";

        public UnityWebRequest UnityWebRequest;

        /// <summary>
        /// Map an API endpoint to a full qualified URL using the Normcore settings.
        /// </summary>
        private static string GetEndpointURL(string endpoint)
        {
            return $"{NormcoreServicesSettings.Host}/{endpoint}";
        }

        /// <summary>
        /// Make an unauthenticated GET request to the service backend.
        /// </summary>
        public static NormcoreServicesRequest Get(string endpoint)
        {
            var req = UnityWebRequest.Get(GetEndpointURL(endpoint));

            return new NormcoreServicesRequest { UnityWebRequest = req };
        }

        /// <summary>
        /// Make an unauthenticated PUT request to the service backend with data.
        /// </summary>
        public static NormcoreServicesRequest Put<T>(string endpoint, T data)
        {
            var req = UnityWebRequest.Put(GetEndpointURL(endpoint), JSON.Serialize(data));

            req.SetRequestHeader("Content-Type", JSONContentType);

            return new NormcoreServicesRequest { UnityWebRequest = req };
        }

        /// <summary>
        /// Make an unauthenticated POST request to the service backend without data.
        /// </summary>
        public static NormcoreServicesRequest Post(string endpoint)
        {
#if UNITY_2022_1_OR_NEWER
            var req = UnityWebRequest.Post(GetEndpointURL(endpoint), string.Empty, JSONContentType);
#else
            // Unity 2021 and earlier automatically applies URL encoding to the request body
            // when using UnityWebRequest.Post, so we have to manually create a POST request
            // with the raw UTF8 bytes to send a JSON body.

            var req = UnityWebRequest.Post(GetEndpointURL(endpoint), string.Empty);

            req.SetRequestHeader("Content-Type", JSONContentType);
#endif
            return new NormcoreServicesRequest { UnityWebRequest = req };
        }

        /// <summary>
        /// Make an unauthenticated POST request to the service backend with data.
        /// </summary>
        public static NormcoreServicesRequest Post<T>(string endpoint, T data)
        {
#if UNITY_2022_1_OR_NEWER
            var req = UnityWebRequest.Post(GetEndpointURL(endpoint), JSON.Serialize(data), JSONContentType);
#else
            // Unity 2021 and earlier automatically applies URL encoding to the request body
            // when using UnityWebRequest.Post, so we have to manually create a POST request
            // with the raw UTF8 bytes to send a JSON body.

            var dlHandler = new DownloadHandlerBuffer();
            var upHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JSON.Serialize(data)));

            var req = new UnityWebRequest(GetEndpointURL(endpoint), "POST", dlHandler, upHandler);

            req.SetRequestHeader("Content-Type", JSONContentType);
#endif
            return new NormcoreServicesRequest { UnityWebRequest = req };
        }

        /// <summary>
        /// Make an unauthenticated DELETE request to the service backend.
        /// </summary>
        public static NormcoreServicesRequest Delete(string endpoint)
        {
            var req = UnityWebRequest.Delete(GetEndpointURL(endpoint));

            return new NormcoreServicesRequest { UnityWebRequest = req };
        }

        /// <summary>
        /// Add a header to the request with a builder-style syntax.
        /// </summary>
        public NormcoreServicesRequest WithHeader(string header, string value)
        {
            UnityWebRequest.SetRequestHeader(header, value);
            return this;
        }

        /// <summary>
        /// Add an authorization header to the request with a builder-style syntax.
        /// </summary>
        public NormcoreServicesRequest WithAuth(IAuthentication auth)
        {
            return WithHeader("Authorization", $"Bearer {auth.AccessToken}");
        }

        /// <summary>
        /// Send the web request and handle the response.
        /// </summary>
        public async ValueTask<NormcoreServicesResponse> Send()
        {
            try
            {
                await SendWebRequestAsync(UnityWebRequest);

                Debug.Assert(UnityWebRequest.result != UnityWebRequest.Result.InProgress);

                if (UnityWebRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    throw NormcoreServicesException.ConnectionError();
                }

                if (UnityWebRequest.result == UnityWebRequest.Result.DataProcessingError)
                {
                    throw NormcoreServicesException.DataProcessingError();
                }

                // Result.Success and Result.ProtocolError both represent complete requests. The
                // individual request methods will handle the error response codes.

                return NormcoreServicesResponse.FromUnityWebRequest(UnityWebRequest);
            }
            finally
            {
                UnityWebRequest.Dispose();
            }
        }


#if ENABLE_UNITASK
        private static async ValueTask SendWebRequestAsync(UnityWebRequest req)
        {
            // Note, UniTask implicitly changes the error handling of UnityWebRequest to throw an
            // exception. See https://github.com/Cysharp/UniTask/issues/304

            try
            {
                await req.SendWebRequest();
            }
            catch (UnityWebRequestException)
            {
                // ignore UniTask exception
            }
        }
#else
        private static async ValueTask SendWebRequestAsync(UnityWebRequest req)
        {
            req.SendWebRequest();

            while (!req.isDone)
            {
                await Task.Yield();
            }
        }
#endif
    }
}
