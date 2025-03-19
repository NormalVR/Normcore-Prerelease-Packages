namespace Normcore.Services
{
    /// <summary>
    /// The set of values that can be passed to <see cref="LobbiesApi.SetLobbyProperties"/>.
    /// </summary>
    /// <remarks>
    /// Any options passed here will replace the existing property of the lobby. To leave a property
    /// as is, leave the value as <see langword="null"/>
    /// </remarks>
    public struct SetLobbyOptions
    {
        /// <summary>
        /// If non-null, the new name of the lobby.
        /// </summary>
        public LobbyName? Name;

        /// <summary>
        /// If non-null, the new size of the lobby.
        /// </summary>
        public int? Capacity;

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
