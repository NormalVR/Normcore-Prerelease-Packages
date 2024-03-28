using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Normcore.Services
{
    [JsonConverter(typeof(LobbyDataContainerConverter))]
    public class LobbyDataContainer
    {
        public Dictionary<string, LobbyDataEntry> Entries = new (); // TODO use non allocating container

        /// <summary>
        /// Shorthand for adding a public value to the data container.
        /// </summary>
        public void AddValue(string key, LobbyDataValue value)
        {
            Entries.Add(key, LobbyDataEntry.Public(value));
        }

        public bool TryGetValue(string key, out LobbyDataValue value)
        {
            if (Entries != null && Entries.TryGetValue(key, out var entry))
            {
                value = entry.Value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
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
