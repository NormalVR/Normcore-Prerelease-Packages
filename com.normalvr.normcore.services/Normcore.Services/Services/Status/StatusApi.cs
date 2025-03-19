using System.Net;
using System.Threading.Tasks;
using static Normcore.Services.Validation;

namespace Normcore.Services
{
    public class StatusApi
    {
        private readonly INormcoreServicesSessionInternal _session;

        internal StatusApi(INormcoreServicesSessionInternal session)
        {
            _session = session;
        }

        /// <summary>
        /// Get the status of the API server.
        /// </summary>
        /// <returns>True if the API server is accessible.</returns>
        public async ValueTask<bool> Get()
        {
            _session.CheckDisposed();
            
            var endpoint = FormatPath("status");
            var response = await Request.Get(_session.BaseUrl, endpoint).Send();

            return response.Status == HttpStatusCode.OK;
        }
    }
}
