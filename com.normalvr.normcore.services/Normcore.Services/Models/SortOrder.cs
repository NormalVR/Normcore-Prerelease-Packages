using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Normcore.Services
{
    /// <summary>
    /// The order in which to sort values.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortOrder
    {
        /// <summary>
        /// Sort in ascending order.
        /// </summary>
        [EnumMember(Value = "asc")] 
        Ascending,
        
        /// <summary>
        /// Sort in descending order.
        /// </summary>
        [EnumMember(Value = "desc")] 
        Descending,
    }
}
