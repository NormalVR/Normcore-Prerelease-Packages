using System.Threading.Tasks;

namespace Normcore.Services
{
    public class StatusService
    {
        /// <summary>
        /// Get the status of the API server.
        /// </summary>
        /// <returns>True if the API server is accessible.</returns>
        public async ValueTask<bool> Get()
        {
            return (await NormcoreServicesRequest.Get("status").Send()).Status == 200;
        }
    }
}
