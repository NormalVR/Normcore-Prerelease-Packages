using System;
using Newtonsoft.Json;

namespace Normcore.Services
{
    [JsonObject]
    public struct LobbyQueryRange
    {
        public int min;
        public int max;
    }

    [JsonObject]
    public class LobbyQuery
    {
        [JsonProperty] private string ownerID;

        [JsonProperty] private LobbyQueryRange? capacity;

        [JsonProperty] private LobbyQueryRange? members;

        [JsonProperty] private string[] tagsIncludeAny;
        [JsonProperty] private string[] tagsIncludeAll;
        [JsonProperty] private string[] tagsExcludeAny;
        [JsonProperty] private string[] tagsExcludeAll;

        /// <summary>
        /// Filter for lobbies owned by a user.
        /// </summary>
        /// <param name="id">The user ID of the owner.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery WithOwner(string id)
        {
            if (ownerID != null)
            {
                throw new Exception("This query parameter has already been specified.");
            }

            ownerID = id;
            return this;
        }

        /// <summary>
        /// Filter for lobbies with an exact number of members.
        /// </summary>
        /// <param name="exactly">The exact number of members.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery WithMemberCount(int exactly)
        {
            return WithMemberCount(exactly, exactly);
        }

        /// <summary>
        /// Filter for lobbies with a range of members.
        /// </summary>
        /// <param name="min">The minimum (inclusive) number of members.</param>
        /// <param name="max">The maximum (inclusive) number of members.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery WithMemberCount(int min, int max)
        {
            if (members != null)
            {
                throw new Exception("This query parameter has already been specified.");
            }

            members = new LobbyQueryRange { min = min, max = max };
            return this;
        }

        /// <summary>
        /// Filter for lobbies with an exact number of spaces for additional users.
        /// </summary>
        /// <param name="exactly">The exact number of available spaces for additional members.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery WithAvailableSpace(int exactly)
        {
            return WithAvailableSpace(exactly, exactly);
        }

        /// <summary>
        /// Filter for lobbies with a range of space for additional users.
        /// </summary>
        /// <param name="min">The minimum (inclusive) space available for additional members.</param>
        /// <param name="max">The maximum (inclusive) space available for additional members.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery WithAvailableSpace(int min, int max)
        {
            if (capacity != null)
            {
                throw new Exception("This query parameter has already been specified.");
            }

            capacity = new LobbyQueryRange { min = min, max = max };
            return this;
        }

        /// <summary>
        /// Filter for lobbies with any of the specified tags.
        /// </summary>
        /// <param name="tags">The tags to filter on.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery IncludeAnyTags(string[] tags)
        {
            if (tagsIncludeAny != null)
            {
                throw new Exception("This query parameter has already been specified.");
            }

            tagsIncludeAny = tags;
            return this;
        }

        /// <summary>
        /// Filter for lobbies with all of the specified tags.
        /// </summary>
        /// <param name="tags">The tags to filter on.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery IncludeAllTags(string[] tags)
        {
            if (tagsIncludeAll != null)
            {
                throw new Exception("This query parameter has already been specified.");
            }

            tagsIncludeAll = tags;
            return this;
        }

        /// <summary>
        /// Filter for lobbies without any of the specified tags.
        /// </summary>
        /// <param name="tags">The tags to filter on.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery ExcludeAnyTags(string[] tags)
        {
            if (tagsExcludeAny != null)
            {
                throw new Exception("This query parameter has already been specified.");
            }

            tagsExcludeAny = tags;
            return this;
        }

        /// <summary>
        /// Filter for lobbies without all of the specified tags.
        /// </summary>
        /// <param name="tags">The tags to filter on.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery ExcludeAllTags(string[] tags)
        {
            if (tagsExcludeAll != null)
            {
                throw new Exception("This query parameter has already been specified.");
            }

            tagsExcludeAll = tags;
            return this;
        }
    }
}
