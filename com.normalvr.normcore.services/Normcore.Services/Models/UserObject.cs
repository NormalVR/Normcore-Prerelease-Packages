using Newtonsoft.Json;

namespace Normcore.Services
{
    /// <summary>
    /// The properties of a user.
    /// </summary>
    [JsonObject]
    public struct UserObject
    {
        /// <summary>
        /// The unique ID of the user.
        /// </summary>
        [JsonProperty("id")] public string Id;

        public override string ToString()
        {
            return $"UserObject(Id: {Id})";
        }
    }
}
