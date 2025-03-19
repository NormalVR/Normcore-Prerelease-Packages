using Newtonsoft.Json;

namespace Normcore.Services
{
    /// <summary>
    /// An entry in a data container, storing a value and its metadata.
    /// </summary>
    [JsonObject]
    public struct LobbyDataEntry
    {
        /// <summary>
        /// The data stored in this entry.
        /// </summary>
        [JsonProperty("value")] public LobbyDataValue Value;

        // TODO add read and write permissions

        /// <summary>
        /// Create a new public data entry.
        /// </summary>
        /// <returns>The new data entry.</returns>
        public static LobbyDataEntry Public(LobbyDataValue value)
        {
            return new LobbyDataEntry { Value = value };
        }

        // TODO add static constructors for protected / private values
    }
}
