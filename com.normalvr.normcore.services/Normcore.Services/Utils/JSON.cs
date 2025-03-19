using Newtonsoft.Json;

namespace Normcore.Services
{
    internal static class JSON
    {
        private static JsonSerializerSettings Settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Converters = {
                new ErrorConverter(),
                new ValidationIssueConverter(),
            },
        };

        public static string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data, Formatting.None, Settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
