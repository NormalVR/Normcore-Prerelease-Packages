using UnityEngine.Networking;

namespace Normcore.Services
{
    internal readonly struct RequestInfo
    {
        public readonly string method;
        public readonly string url;
        public readonly string error;
        public readonly PageInfo? page;

        public RequestInfo(UnityWebRequest request, PageInfo? page)
        {
            method = request.method;
            url = request.url;
            error = request.error;
            this.page = page;
        }
    }
}
