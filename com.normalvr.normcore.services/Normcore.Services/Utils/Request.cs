using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using UnityEngine;
using UnityEngine.Networking;

#if ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Normcore.Services
{
    internal struct Request
    {
        private const string JSONContentType = "application/json";

        private UnityWebRequest _unityWebRequest;
        private PageInfo? _page;

        private static string GetEndpointUrl(string baseUrl, ValidatedPath endpoint)
        {
            // Uri class or Path.Combine can behave un-intuitively in certain cases, so
            // better to just do the simple thing.
            return $"{baseUrl.TrimEnd('/')}/{endpoint.Value.TrimStart('/')}";
        }

        /// <summary>
        /// Make an unauthenticated GET request to the service backend.
        /// </summary>
        public static Request Get(string baseUrl, ValidatedPath endpoint)
        {
            var req = UnityWebRequest.Get(GetEndpointUrl(baseUrl, endpoint));

            return new Request { _unityWebRequest = req };
        }

        /// <summary>
        /// Make an unauthenticated PUT request to the service backend with data.
        /// </summary>
        public static Request Put<T>(string baseUrl, ValidatedPath endpoint, T data)
        {
            var req = UnityWebRequest.Put(GetEndpointUrl(baseUrl, endpoint), JSON.Serialize(data));

            req.SetRequestHeader("Content-Type", JSONContentType);

            return new Request { _unityWebRequest = req };
        }

        /// <summary>
        /// Make an unauthenticated POST request to the service backend without data.
        /// </summary>
        public static Request Post(string baseUrl, ValidatedPath endpoint)
        {
#if UNITY_2022_1_OR_NEWER
            var req = UnityWebRequest.Post(GetEndpointUrl(baseUrl, endpoint), string.Empty, JSONContentType);
#else
            // Unity 2021 and earlier automatically applies URL encoding to the request body
            // when using UnityWebRequest.Post, so we have to manually create a POST request
            // with the raw UTF8 bytes to send a JSON body.

            var req = UnityWebRequest.Post(GetEndpointUrl(baseUrl, endpoint), string.Empty);

            req.SetRequestHeader("Content-Type", JSONContentType);
#endif
            return new Request { _unityWebRequest = req };
        }

        /// <summary>
        /// Make an unauthenticated POST request to the service backend with data.
        /// </summary>
        public static Request Post<T>(string baseUrl, ValidatedPath endpoint, T data)
        {
#if UNITY_2022_1_OR_NEWER
            var req = UnityWebRequest.Post(GetEndpointUrl(baseUrl, endpoint), JSON.Serialize(data), JSONContentType);
#else
            // Unity 2021 and earlier automatically applies URL encoding to the request body
            // when using UnityWebRequest.Post, so we have to manually create a POST request
            // with the raw UTF8 bytes to send a JSON body.

            var dlHandler = new DownloadHandlerBuffer();
            var upHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JSON.Serialize(data)));

            var req = new UnityWebRequest(GetEndpointUrl(baseUrl, endpoint), "POST", dlHandler, upHandler);

            req.SetRequestHeader("Content-Type", JSONContentType);
#endif
            return new Request { _unityWebRequest = req };
        }

        /// <summary>
        /// Make an unauthenticated DELETE request to the service backend.
        /// </summary>
        public static Request Delete(string baseUrl, ValidatedPath endpoint)
        {
            var req = UnityWebRequest.Delete(GetEndpointUrl(baseUrl, endpoint));

            return new Request { _unityWebRequest = req };
        }

        /// <summary>
        /// Add a header to the request with a builder-style syntax.
        /// </summary>
        public Request WithHeader(string header, string value)
        {
            _unityWebRequest.SetRequestHeader(header, value);
            return this;
        }

        /// <summary>
        /// Add an authorization header to the request with a builder-style syntax.
        /// </summary>
        public Request WithAuth(IAuthentication auth)
        {
            return WithHeader("Authorization", $"Bearer {auth.AccessToken}");
        }
        
        /// <summary>
        /// Add pagination parameters to the request with a builder-style syntax.
        /// </summary>
        public Request WithPage(PageInfo? page, bool useQueryParams)
        {
            var p = page ?? PageInfo.Default();

            if (useQueryParams)
            {
                if (p.Offset.HasValue)
                {
                    var queryString = HttpUtility.ParseQueryString(string.Empty);
                    queryString.Add("offset", p.Offset.Value.ToString());
                    queryString.Add("limit", p.Count.ToString());

                    var url = _unityWebRequest.url;
                    var separator = url.Contains('?') ? '&' : '?';
                    _unityWebRequest.url = url + separator + queryString;
                }
                else
                {
                    // TODO: Add cursor pagination support once the backend supports it.
                    throw new System.NotImplementedException();
                }
            }

            _page = p;
            return this;
        }

        /// <summary>
        /// Send the web request and handle the response.
        /// </summary>
        public async ValueTask<Response> Send()
        {
            try
            {
                var requestInfo = new RequestInfo(_unityWebRequest, _page);
                
                await SendWebRequestAsync(_unityWebRequest);

                Debug.Assert(_unityWebRequest.result != UnityWebRequest.Result.InProgress);

                if (_unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    throw new ConnectionException(requestInfo);
                }
                if (_unityWebRequest.result == UnityWebRequest.Result.DataProcessingError)
                {
                    throw new RequestException(requestInfo, "Failed to read response.");
                }

                // Result.Success and Result.ProtocolError both represent complete requests. The
                // individual request methods will handle the error response codes.

                var status = (HttpStatusCode)_unityWebRequest.responseCode;
                var body = _unityWebRequest.downloadHandler?.text;
                
                return new Response(requestInfo, status, body);
            }
            finally
            {
                _unityWebRequest.Dispose();
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
