using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Normcore.Services
{
    /// <summary>
    /// A container that stores changes to custom data.
    /// </summary>
    [JsonConverter(typeof(LobbyDataUpdateConverter))]
    public class LobbyDataUpdate
    {
        /// <summary>
        /// The custom data changes.
        /// </summary>
        /// <remarks>
        /// A <see langword="null"/> value indicates the entry will be deleted, otherwise it
        /// represents the updated value.
        /// </remarks>
        public Dictionary<string, LobbyDataEntry?> Changes = new();

        /// <summary>
        /// Create or update a value in the custom data.
        /// </summary>
        /// <param name="key">The key to add or replace.</param>
        /// <param name="value">The updated value.</param>
        public void UpdateValue(string key, LobbyDataValue value)
        {
            Changes.Add(key, LobbyDataEntry.Public(value));
        }

        /// <summary>
        /// Delete a value from the custom data.
        /// </summary>
        /// <param name="key">The key to delete.</param>
        public void DeleteValue(string key)
        {
            Changes.Add(key, null);
        }
    }

    internal class LobbyDataUpdateConverter : JsonConverter<LobbyDataUpdate>
    {
        public override void WriteJson(JsonWriter writer, LobbyDataUpdate value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.Changes);
        }

        public override LobbyDataUpdate ReadJson(JsonReader reader, Type objectType, LobbyDataUpdate existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new System.NotImplementedException(); // there should be no reason to read this value
        }
    }
}
