using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Normcore.Services
{
    [JsonConverter(typeof(LobbyDataUpdateConverter))]
    public class LobbyDataUpdate
    {
        public Dictionary<string, LobbyDataEntry?> Changes = new(); // TODO use non allocating collection

        /// <summary>
        /// Record a change to create or update a value in the lobby data.
        /// </summary>
        /// <param name="key">The key to add or replace.</param>
        /// <param name="value">The updated value.</param>
        public void UpdateValue(string key, LobbyDataValue value)
        {
            Changes.Add(key, LobbyDataEntry.Public(value));
        }

        /// <summary>
        /// Record a change to delete a value from the lobby data.
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
            throw new NotImplementedException(); // there should be no reason to read this value
        }
    }
}
