using System;

namespace Normcore.Services
{
    internal static class LobbiesValidation
    {
        public static string ValidateLobbyId(string lobbyId)
        {
            if (string.IsNullOrWhiteSpace(lobbyId))
            {
                throw new ArgumentOutOfRangeException(nameof(lobbyId), lobbyId, $"Must be non-empty.");
            }

            return lobbyId;
        }
        
        public static string ValidateUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentOutOfRangeException(nameof(userId), userId, $"Must be non-empty.");
            }

            return userId;
        }

        public static int ValidateCapacity(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), capacity, $"Must be greater than zero.");
            }
            if (capacity > LobbiesApi.MaxLobbyCapacity)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), capacity, $"Must not exceed {LobbiesApi.MaxLobbyCapacity}.");
            }

            return capacity;
        }

        public static string ValidateName(string name)
        {
            if (name != null)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentOutOfRangeException(nameof(name), name, $"Must be non-empty.");
                }
                if (name.Length > LobbiesApi.MaxLobbyNameLength)
                {
                    throw new ArgumentOutOfRangeException(nameof(name), name, $"Must not exceed {LobbiesApi.MaxLobbyNameLength} characters in length.");
                }

                name = name.Trim();
            }

            return name;
        }

        public static string[] ValidateTags(string[] tags, bool isQuery = false)
        {
            tags ??= Array.Empty<string>();

            if (isQuery)
            {
                if (tags.Length > LobbiesApi.MaxLobbyQueryTagCount)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(tags),
                        tags,
                        $"Must not exceed {LobbiesApi.MaxLobbyQueryTagCount} tags."
                    );
                }
            }
            else
            {
                if (tags.Length > LobbiesApi.MaxLobbyTagCount)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(tags),
                        tags,
                        $"Must not exceed {LobbiesApi.MaxLobbyNameLength} tags."
                    );
                }
            }

            for (var i = 0; i < tags.Length; i++)
            {
                var tag = tags[i];

                if (string.IsNullOrWhiteSpace(tag))
                {
                    throw new ArgumentOutOfRangeException(nameof(tags), tags, $"Tags must be non-empty.");
                }
                if (tag.Length > LobbiesApi.MaxLobbyTagLength)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(tags),
                        tags,
                        $"Tags must not exceed {LobbiesApi.MaxLobbyTagLength} characters in length."
                    );
                }

                tags[i] = tag.Trim();
            }

            return tags;
        }

        public static LobbyDataContainer ValidateDataContainer(LobbyDataContainer data)
        {
            data ??= new LobbyDataContainer();

            foreach (var entry in data.Entries)
            {
                if (string.IsNullOrWhiteSpace(entry.Key))
                {
                    throw new ArgumentOutOfRangeException(nameof(data), data, $"Key must be non-empty.");
                }
                if (entry.Key.Length > LobbiesApi.MaxLobbyDataKeyLength)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(data),
                        data,
                        $"Keys must not exceed {LobbiesApi.MaxLobbyDataKeyLength} characters in length."
                    );
                }

                var value = entry.Value.Value;

                if (value.Type == LobbyDataValueType.Invalid)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(data),
                        data,
                        $"Values must have a valid type."
                    );
                }

                if (value.Type == LobbyDataValueType.String)
                {
                    var strValue = value.AsString();

                    if (strValue != null && strValue.Length > LobbiesApi.MaxLobbyDataValLength)
                    {
                        throw new ArgumentOutOfRangeException(
                            nameof(data),
                            data,
                            $"String values must not exceed {LobbiesApi.MaxLobbyDataValLength} characters in length."
                        );
                    }
                }
            }
            
            return data;
        }

        public static LobbyDataUpdate ValidateDataUpdate(LobbyDataUpdate update)
        {
            if (update == null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            foreach (var change in update.Changes)
            {
                if (string.IsNullOrWhiteSpace(change.Key))
                {
                    throw new ArgumentOutOfRangeException(nameof(update), update, $"Key must be non-empty.");
                }
                if (change.Key.Length > LobbiesApi.MaxLobbyDataKeyLength)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(update),
                        update,
                        $"Keys must not exceed {LobbiesApi.MaxLobbyDataKeyLength} characters in length."
                    );
                }

                if (!change.Value.HasValue)
                {
                    continue;
                }

                var value = change.Value.Value.Value;

                if (value.Type == LobbyDataValueType.Invalid)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(update),
                        update,
                        $"Values must have a valid type."
                    );
                }

                if (value.Type == LobbyDataValueType.String)
                {
                    var strValue = value.AsString();

                    if (strValue != null && strValue.Length > LobbiesApi.MaxLobbyDataValLength)
                    {
                        throw new ArgumentOutOfRangeException(
                            nameof(update),
                            update,
                            $"String values must not exceed {LobbiesApi.MaxLobbyDataValLength} characters in length."
                        );
                    }
                }
            }
            
            return update;
        }
    }
}
