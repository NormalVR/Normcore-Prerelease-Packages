using Newtonsoft.Json;

namespace Normcore.Services
{
    /// <summary>
    /// An entry in the lobby data map, containing both the value and metadata.
    /// </summary>
    [JsonObject]
    public struct LobbyDataEntry
    {
        [JsonProperty("value")] public LobbyDataValue Value;

        // TODO add read and write permissions

        /// <summary>
        /// Create a new public data entry.
        /// </summary>
        /// <returns></returns>
        public static LobbyDataEntry Public(LobbyDataValue value)
        {
            return new LobbyDataEntry { Value = value };
        }

        // TODO add static constructors for protected / private values
    }
}
