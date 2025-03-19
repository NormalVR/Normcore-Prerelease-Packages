using Newtonsoft.Json;

namespace Normcore.Services.Lobbies
{
    [JsonObject]
    internal struct CreateLobbyRequestBody
    {
        [JsonProperty("name")] public string Name;
        [JsonProperty("tags")] public string[] Tags;
        [JsonProperty("data")] public LobbyDataContainer Data;
        [JsonProperty("totalCapacity")] public int Capacity;
    }

    [JsonObject]
    internal struct ModifyLobbyRequestBody
    {
        [JsonProperty("name")] public LobbyName? Name;
        [JsonProperty("tags")] public string[] Tags;
        [JsonProperty("data")] public LobbyDataContainer Data;
        [JsonProperty("totalCapacity")] public int? Capacity;
    }

    [JsonObject]
    internal struct SetLobbyOwnerRequestBody
    {
        [JsonProperty("id")] public string Id;
    }

    [JsonObject]
    internal struct UpdateLobbyDataRequestBody
    {
        [JsonProperty("data")] public LobbyDataUpdate Data;
    }

    [JsonObject]
    internal struct ModifyLobbyMemberRequestBody
    {
        [JsonProperty("data")] public LobbyDataContainer Data;
    }
    
    [JsonObject]
    internal class IdObject
    {
        [JsonProperty("id")] public string Id;
    }
}
