using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Normcore.Services
{
    [JsonObject]
    [JsonConverter(typeof(ErrorConverter))]
    internal class Error
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
    
    [JsonObject]
    internal class ValidationError : Error
    {
        [JsonProperty("data")]
        public ValidationIssues Data { get; set; }
    }

    internal class ErrorConverter : JsonConverter<Error>
    {
        public override Error ReadJson(JsonReader reader, Type objectType, Error existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var code = obj["code"]?.ToString().ToUpperInvariant();
            
            var error = code switch
            {
                ErrorCode.InvalidSchema     => new ValidationError(),
                _                           => new Error(),
            };

            serializer.Populate(obj.CreateReader(), error);
            return error;
        }

        public override void WriteJson(JsonWriter writer, Error value, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }
    }
}
