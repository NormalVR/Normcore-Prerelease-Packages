using System.Collections;
using UnityEngine;

namespace Normcore.Services
{
    public static class NormcoreServicesHelpers
    {
        /// <summary>
        /// Create a coroutine that heartbeats a user session on a regular basis.
        /// </summary>
        /// <param name="client">The client instance to heartbeat.</param>
        public static IEnumerator HeartbeatRoutine(NormcoreServices client)
        {
            while (true)
            {
                yield return new WaitForSeconds(5f);
                yield return client.Users.Heartbeat(); // TODO error handling
            }
        }
    }
}
