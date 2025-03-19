using Newtonsoft.Json;

namespace Normcore.Services.Users
{
    [JsonObject]
    internal struct GetHeartbeatBody
    {
        [JsonProperty("interval")] public int Interval;
    }
}
