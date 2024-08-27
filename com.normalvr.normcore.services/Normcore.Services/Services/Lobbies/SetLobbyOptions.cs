namespace Normcore.Services
{
    /// <summary>
    /// The set of values that can be passed to SetLobbyProperties. Null values will leave the property unmodified.
    /// </summary>
    public struct SetLobbyOptions
    {
        /// <summary>
        /// The new name of the lobby.
        /// </summary>
        public LobbyName? Name;

        /// <summary>
        /// If non-null, the new size of the lobby.
        /// </summary>
        public uint? Size;

        /// <summary>
        /// If non-null, the new list of tags of the lobby.
        /// </summary>
        public string[] Tags;

        /// <summary>
        /// If non-null, the new set of custom data of the lobby.
        /// </summary>
        public LobbyDataContainer Data;
    }
}
