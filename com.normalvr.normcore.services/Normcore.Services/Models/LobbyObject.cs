using Newtonsoft.Json;

namespace Normcore.Services
{
    /// <summary>
    /// The properties of a lobby.
    /// </summary>
    [JsonObject]
    public struct LobbyObject
    {
        /// <summary>
        /// The ID of the lobby.
        /// </summary>
        [JsonProperty("id")] public string Id;

        /// <summary>
        /// The name of the lobby.
        /// </summary>
        [JsonProperty("name")] public string Name;

        /// <summary>
        /// The tags associated with the lobby.
        /// </summary>
        [JsonProperty("tags")] public string[] Tags;

        /// <summary>
        /// The custom data associated with the lobby.
        /// </summary>
        [JsonProperty("data")] public LobbyDataContainer Data;
        
        /// <summary>
        /// The maximum number of members allowed in the lobby.
        /// </summary>
        [JsonProperty("totalCapacity")] public int Capacity;

        /// <summary>
        /// The number of members currently in the lobby.
        /// </summary>
        [JsonProperty("memberCount")] public int OccupiedSlots;

        /// <summary>
        /// The number of members who can join this lobby until it is full.
        /// </summary>
        [JsonProperty("remainingCapacity")] public int AvailableSlots;
    }
}
