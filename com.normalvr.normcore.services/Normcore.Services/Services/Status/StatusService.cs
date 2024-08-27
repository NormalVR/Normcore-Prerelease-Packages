using System.Threading.Tasks;
using static Normcore.Services.Validation;

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
            var endpoint = FormatPath("status");
            var response = await NormcoreServicesRequest.Get(endpoint).Send();

            return response.Status == 200;
        }
    }
}
