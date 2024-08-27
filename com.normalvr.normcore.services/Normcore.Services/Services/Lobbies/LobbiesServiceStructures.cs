using Newtonsoft.Json;

namespace Normcore.Services
{
    [JsonObject]
    internal struct CreateLobbyRequestBody
    {
        [JsonProperty("name")] public string Name;

        [JsonProperty("tags")] public string[] Tags;

        [JsonProperty("size")] public uint Size;

        [JsonProperty("data")] public LobbyDataContainer Data;
    }

    [JsonObject]
    internal struct ModifyLobbyRequestBody
    {
        [JsonProperty("name")] public LobbyName? Name;

        [JsonProperty("tags")] public string[] Tags;

        [JsonProperty("size")] public uint? Size;

        [JsonProperty("data")] public LobbyDataContainer Data;
    }

    [JsonObject]
    internal struct SetLobbyOwnerRequestBody
    {
        [JsonProperty("id")] public string ID;
    }

    [JsonObject]
    internal struct UpdateLobbyDataRequestBody
    {
        [JsonProperty("data")] public LobbyDataUpdate Data;
    }
}
