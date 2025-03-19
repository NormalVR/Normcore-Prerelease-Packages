using Newtonsoft.Json;

namespace Normcore.Services
{
    /// <summary>
    /// A wrapper around a string that makes it a value type instead of a reference type
    /// for compatibility with the argument pattern used in <see cref="SetLobbyOptions"/>.
    /// </summary>
    [JsonConverter(typeof(LobbyDataContainerConverter))]
    public struct LobbyName
    {
        /// <summary>
        /// The name of the lobby.
        /// </summary>
        public string Value { get; private set; }

        public LobbyName(string value)
        {
            Value = value;
        }

        public static implicit operator LobbyName(string value) => new LobbyName(value);
    }
}
