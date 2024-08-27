using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Normcore.Services
{
    [JsonObject]
    public struct LobbyObject
    {
        /// <summary>
        /// The ID of the lobby.
        /// </summary>
        [JsonProperty("id")] public string ID;

        /// <summary>
        /// The name of the lobby.
        /// </summary>
        [JsonProperty("name")] public string Name;

        /// <summary>
        /// The maximum capacity of the lobby.
        /// </summary>
        [JsonProperty("size")] public uint Size;

        /// <summary>
        /// The tags associated with the lobby.
        /// </summary>
        [JsonProperty("tags")] public string[] Tags;

        /// <summary>
        /// The custom data associated with the lobby.
        /// </summary>
        [JsonProperty("data")] public LobbyDataContainer Data;
    }
}
