using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Normcore.Services
{
    /// <summary>
    /// A container that stores custom data.
    /// </summary>
    [JsonConverter(typeof(LobbyDataContainerConverter))]
    public class LobbyDataContainer
    {
        /// <summary>
        /// The custom data entries.
        /// </summary>
        public Dictionary<string, LobbyDataEntry> Entries = new();

        /// <summary>
        /// Adds a value to the data container.
        /// </summary>
        /// <param name="key">The key to set the data for.</param>
        /// <param name="value">The data value.</param>
        public void AddValue(string key, LobbyDataValue value)
        {
            Entries.Add(key, LobbyDataEntry.Public(value));
        }

        /// <summary>
        /// Gets the data value for the specified key, if it exists.
        /// </summary>
        /// <param name="key">The key to get the data for.</param>
        /// <param name="value">The data value, or <see langword="default"/> if the key does not exist.</param>
        /// <returns><see langword="true"/> if the key exists; otherwise, <see langword="false"/>.</returns>
        public bool TryGetValue(string key, out LobbyDataValue value)
        {
            if (Entries != null && Entries.TryGetValue(key, out var entry))
            {
                value = entry.Value;
                return true;
            }
            
            value = default;
            return false;
        }
    }

    internal class LobbyDataContainerConverter : JsonConverter<LobbyDataContainer>
    {
        public override void WriteJson(JsonWriter writer, LobbyDataContainer value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.Entries);
        }

        public override LobbyDataContainer ReadJson(JsonReader reader, Type objectType, LobbyDataContainer existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var entries = serializer.Deserialize<Dictionary<string, LobbyDataEntry>>(reader);

            return new LobbyDataContainer { Entries = entries };
        }
    }
}
