using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Normcore.Services
{
    // The server uses Zod for validation.
    // https://zod.dev/ERROR_HANDLING
    
    /// <summary>
    /// A list of validation issues.
    /// </summary>
    [JsonObject]
    public class ValidationIssues
    {
        /// <summary>
        /// The validation issues.
        /// </summary>
        [JsonProperty("issues")]
        public ValidationIssue[] Issues { get; set; }
    }
    
    /// <summary>
    /// A problem that occured while validating the request arguments.
    /// </summary>
    [JsonObject]
    public class ValidationIssue
    {
        /// <summary>
        /// The code describing the type of the validation issue.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// The location of the argument that failed validation. e.g. <c>["addresses", 0, "line1"]</c>
        /// </summary>
        [JsonProperty("path")]
        public object[] Path { get; set; }

        /// <summary>
        /// A human-readable message describing the issue. e.g. <c>Invalid type. Expected string, received number.</c>
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ParsedType
    {
        [EnumMember(Value = "string")] 
        String,
        [EnumMember(Value = "nan")] 
        Nan,
        [EnumMember(Value = "number")] 
        Number,
        [EnumMember(Value = "integer")] 
        Integer,
        [EnumMember(Value = "float")] 
        Float,
        [EnumMember(Value = "boolean")] 
        Boolean,
        [EnumMember(Value = "date")] 
        Date,
        [EnumMember(Value = "bigint")] 
        BigInt,
        [EnumMember(Value = "symbol")] 
        Symbol,
        [EnumMember(Value = "function")] 
        Function,
        [EnumMember(Value = "undefined")] 
        Undefined,
        [EnumMember(Value = "null")] 
        Null,
        [EnumMember(Value = "array")] 
        Array,
        [EnumMember(Value = "object")] 
        Object,
        [EnumMember(Value = "unknown")] 
        Unknown,
        [EnumMember(Value = "promise")] 
        Promise,
        [EnumMember(Value = "void")] 
        Void,
        [EnumMember(Value = "never")] 
        Never,
        [EnumMember(Value = "map")] 
        Map,
        [EnumMember(Value = "set")] 
        Set,
    }

    [JsonObject]
    public class ValidationInvalidType : ValidationIssue
    {
        /// <summary>
        /// The argument type expected.
        /// </summary>
        [JsonProperty("expected")]
        public ParsedType Expected { get; set; }

        /// <summary>
        /// The argument type received.
        /// </summary>
        [JsonProperty("received")]
        public ParsedType Received { get; set; }
    }

    [JsonObject]
    public class ValidationUnrecognizedKeys : ValidationIssue
    {
        /// <summary>
        /// The list of unrecognised keys.
        /// </summary>
        [JsonProperty("keys")]
        public string[] Keys { get; set; }
    }

    [JsonObject]
    public class ValidationInvalidUnion : ValidationIssue
    {
        /// <summary>
        /// The list of unrecognised keys.
        /// </summary>
        [JsonProperty("unionErrors")]
        public ValidationIssues[] UnionErrors { get; set; }
    }

    [JsonObject]
    public class ValidationInvalidEnumValue : ValidationIssue
    {
        /// <summary>
        /// The set of acceptable string values for this enum.
        /// </summary>
        [JsonProperty("options")]
        public string[] Options { get; set; }
    }

    [JsonObject]
    public class ValidationInvalidDate : ValidationIssue
    {
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StringValidation
    {
        [EnumMember(Value = "url")] 
        Url,
        [EnumMember(Value = "email")] 
        Email,
        [EnumMember(Value = "uuid")] 
        Uuid,
    }

    [JsonObject]
    public class ValidationInvalidString : ValidationIssue
    {
        /// <summary>
        /// Which built-in string validator failed.
        /// </summary>
        [JsonProperty("validation")]
        public StringValidation Validation { get; set; }
    }

    [JsonObject]
    public class ValidationTooSmall : ValidationIssue
    {
        /// <summary>
        /// The type of the data failing validation.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The expected length/value.
        /// </summary>
        [JsonProperty("minimum")]
        public int Minimum { get; set; }

        /// <summary>
        /// Whether the minimum is included in the range of acceptable values.
        /// </summary>
        [JsonProperty("inclusive")]
        public bool Inclusive { get; set; }
        
        /// <summary>
        /// Whether the size/length is constrained to be an exact value.
        /// </summary>
        [JsonProperty("exact")]
        public bool Exact { get; set; }
    }

    [JsonObject]
    public class ValidationTooBig : ValidationIssue
    {
        /// <summary>
        /// The type of the data failing validation.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The expected length/value.
        /// </summary>
        [JsonProperty("maximum")]
        public int Maximum { get; set; }

        /// <summary>
        /// Whether the minimum is included in the range of acceptable values.
        /// </summary>
        [JsonProperty("inclusive")]
        public bool Inclusive { get; set; }
        
        /// <summary>
        /// Whether the size/length is constrained to be an exact value.
        /// </summary>
        [JsonProperty("exact")]
        public bool Exact { get; set; }
    }

    [JsonObject]
    public class ValidationNotMultipleOf : ValidationIssue
    {
        /// <summary>
        /// The value the number should be a multiple of.
        /// </summary>
        [JsonProperty("multipleOf")]
        public int MultipleOf { get; set; }
    }

    internal class ValidationIssueConverter : JsonConverter<ValidationIssue>
    {
        public override ValidationIssue ReadJson(JsonReader reader, Type objectType, ValidationIssue existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var code = obj["code"]?.ToString();

            var issue = code switch
            {
                "invalid_type"       => new ValidationInvalidType(),
                "unrecognized_keys"  => new ValidationUnrecognizedKeys(),
                "invalid_union"      => new ValidationInvalidUnion(),
                "invalid_enum_value" => new ValidationInvalidEnumValue(),
                "invalid_date"       => new ValidationInvalidDate(),
                "invalid_string"     => new ValidationInvalidString(),
                "too_small"          => new ValidationTooSmall(),
                "too_big"            => new ValidationTooBig(),
                "not_multiple_of"    => new ValidationNotMultipleOf(),
                _                    => new ValidationIssue(),
            };

            serializer.Populate(obj.CreateReader(), issue);
            return issue;
        }

        public override void WriteJson(JsonWriter writer, ValidationIssue value, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }
    }
}
