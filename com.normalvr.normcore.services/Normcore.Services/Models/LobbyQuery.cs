using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Normcore.Services.LobbiesValidation;

namespace Normcore.Services
{
    /// <summary>
    /// The keys available to sort lobbies by.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LobbySortingKey
    {
        /// <summary>
        /// The ID of the lobby.
        /// </summary>
        [EnumMember(Value = "id")] 
        Id,
            
        /// <summary>
        /// The name of the lobby.
        /// </summary>
        [EnumMember(Value = "name")] 
        Name,
            
        /// <summary>
        /// The maximum number of members allowed in the lobby
        /// </summary>
        [EnumMember(Value = "totalCapacity")] 
        Capacity,
            
        /// <summary>
        /// The number of members currently in the lobby.
        /// </summary>
        [EnumMember(Value = "memberCount")] 
        OccupiedSlots,
            
        /// <summary>
        /// The number of members who can join this lobby until it is full.
        /// </summary>
        [EnumMember(Value = "remainingCapacity")] 
        AvailableSlots,
        
        /// <summary>
        /// The time at which the lobby was created.
        /// </summary>
        [EnumMember(Value = "createdAt")] 
        CreationTime,
            
        /// <summary>
        /// The time at which the lobby was most recently modified.
        /// </summary>
        [EnumMember(Value = "updatedAt")] 
        LastUpdateTime,
    }
    
    /// <summary>
    /// The query used by <see cref="LobbiesApi.GetLobbies"/> to filter and sort lobbies.
    /// </summary>
    [JsonObject]
    public struct LobbyQuery
    {
        [JsonObject]
        struct QueryRange
        {
            [JsonProperty("min")]
            public int min;
            [JsonProperty("max")]
            public int max;
        }
        
        [JsonProperty("ownerID")]
        private string ownerId;

        [JsonProperty("totalCapacity")]
        private QueryRange? capacity;
        
        [JsonProperty("memberCount")]
        private QueryRange? occupiedSlots;
        [JsonProperty("remainingCapacity")]
        private QueryRange? availableSlots;
        
        [JsonProperty("tagsIncludeAny")]
        private string[] tagsIncludeAny;
        [JsonProperty("tagsIncludeAll")]
        private string[] tagsIncludeAll;
        [JsonProperty("tagsExcludeAny")]
        private string[] tagsExcludeAny;
        [JsonProperty("tagsExcludeAll")]
        private string[] tagsExcludeAll;

        [JsonProperty("order"), JsonConverter(typeof(OrderedDictionaryConverter<LobbySortingKey, SortOrder>))]
        private OrderedDictionary<LobbySortingKey, SortOrder> sorting;
        
        [JsonProperty("offset")]
        internal int offset;
        [JsonProperty("limit")]
        internal int count;
        
        /// <summary>
        /// Sorts the lobbies by the specified paramter.
        /// </summary>
        /// <remarks>
        /// When multiple sorting keys are added to the same query, the first key is the primary sort
        /// key, with subsequent keys breaking ties in the order they were added.
        /// </remarks>
        /// <param name="key">The lobby paramter to sort by.</param>
        /// <param name="order">Whether results should be in ascending or descending order.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery WithSorting(LobbySortingKey key, SortOrder order = SortOrder.Ascending)
        {
            sorting ??= new OrderedDictionary<LobbySortingKey, SortOrder>();
            sorting.Add(key, order);
            return this;
        }
        
        /// <summary>
        /// Filter for lobbies owned by a user.
        /// </summary>
        /// <param name="userId">The user ID of the owner.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery WithOwner(string userId)
        {
            if (ownerId != null)
            {
                throw new Exception("This query parameter has already been specified.");
            }

            ownerId = ValidateUserId(userId);
            return this;
        }

        /// <summary>
        /// Filter for lobbies with an exact number of slots.
        /// </summary>
        /// <param name="exactly">The number of slots.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery WithCapacity(int exactly)
        {
            if (exactly < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(exactly), exactly, "Must not be negative.");
            }
            
            return WithCapacity(exactly, exactly);
        }

        /// <summary>
        /// Filter for lobbies with a number of slots in the specified range.
        /// </summary>
        /// <param name="min">The minimum (inclusive) slots.</param>
        /// <param name="max">The maximum (inclusive) slots.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery WithCapacity(int min, int max)
        {
            if (capacity != null)
            {
                throw new Exception("This query parameter has already been specified.");
            }
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, "Must not be negative.");
            }
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Must not be negative.");
            }
            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, $"Must not exceed max ({max}).");
            }

            capacity = new QueryRange { min = min, max = max };
            return this;
        }

        /// <summary>
        /// Filter for lobbies with an exact number of joined members.
        /// </summary>
        /// <param name="exactly">The number of joined members.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery WithOccupiedSlots(int exactly)
        {
            if (exactly < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(exactly), exactly, "Must not be negative.");
            }
            
            return WithOccupiedSlots(exactly, exactly);
        }

        /// <summary>
        /// Filter for lobbies with a number of joined members in the specified range.
        /// </summary>
        /// <param name="min">The minimum (inclusive) number of joined members.</param>
        /// <param name="max">The maximum (inclusive) number of joined members.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery WithOccupiedSlots(int min, int max)
        {
            if (occupiedSlots != null)
            {
                throw new Exception("This query parameter has already been specified.");
            }
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, "Must not be negative.");
            }
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Must not be negative.");
            }
            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, $"Must not exceed max ({max}).");
            }

            occupiedSlots = new QueryRange { min = min, max = max };
            return this;
        }

        /// <summary>
        /// Filter for lobbies with an exact number of available slots for new members.
        /// </summary>
        /// <param name="exactly">The number of available slots for new members.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery WithAvailableSlots(int exactly)
        {
            if (exactly < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(exactly), exactly, "Must not be negative.");
            }
            
            return WithAvailableSlots(exactly, exactly);
        }

        /// <summary>
        /// Filter for lobbies with a number of available slots for new members in the specified range.
        /// </summary>
        /// <param name="min">The minimum (inclusive) available slots for new members.</param>
        /// <param name="max">The maximum (inclusive) available slots for new members.</param>
        /// <returns>This lobby query builder.</returns>
        public LobbyQuery WithAvailableSlots(int min, int max)
        {
            if (availableSlots != null)
            {
                throw new Exception("This query parameter has already been specified.");
            }
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, "Must not be negative.");
            }
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Must not be negative.");
            }
            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, $"Must not exceed max ({max}).");
            }

            availableSlots = new QueryRange { min = min, max = max };
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

            tagsIncludeAny = ValidateTags(tags, isQuery: true);
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

            tagsIncludeAll = ValidateTags(tags, isQuery: true);
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

            tagsExcludeAny = ValidateTags(tags, isQuery: true);
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

            tagsExcludeAll = ValidateTags(tags, isQuery: true);
            return this;
        }
    }
}
