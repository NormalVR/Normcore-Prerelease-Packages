using Newtonsoft.Json;

namespace Normcore.Services
{
    /// <summary>
    /// The properties of a lobby member.
    /// </summary>
    [JsonObject]
    public struct LobbyMemberObject
    {
        /// <summary>
        /// The ID of the user.
        /// </summary>
        [JsonProperty("id")] public string Id;

        /// <summary>
        /// The custom data associated with the lobby member.
        /// </summary>
        [JsonProperty("data")] public LobbyDataContainer Data;
    }
}
