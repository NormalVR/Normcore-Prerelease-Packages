using Newtonsoft.Json;

namespace Normcore.Services
{
    [JsonObject]
    public struct UserObject
    {
        [JsonProperty("id")] public string ID;

        public override string ToString()
        {
            return $"UserObject(ID: {ID})";
        }
    }
}
