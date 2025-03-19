using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Normcore.Services
{
    /// <summary>
    /// A dictionary that iterates over items the order in which items were added.
    /// </summary>
    internal class OrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        readonly List<KeyValuePair<TKey, TValue>> _items = new();

        public bool IsReadOnly => false;
        public int Count => _items.Count;
        public ICollection<TKey> Keys => _items.Select(x => x.Key).ToArray();
        public ICollection<TValue> Values => _items.Select(x => x. Value).ToArray();
        
        public TValue this[TKey key]
        {
            get => _items.First(x => x.Key.Equals(key)).Value;
            set => Add(key, value);
        }

        public void Clear() => _items.Clear();
        public bool ContainsKey(TKey key) => _items.Any(x => x.Key.Equals(key));
        public bool Contains(KeyValuePair<TKey, TValue> item) => _items.Contains(item);
        public bool TryGetValue(TKey key, out TValue value)
        {
            foreach (var item in _items)
            {
                if (item.Key.Equals(key))
                {
                    value = item.Value;
                    return true;
                }
            }
            
            value = default;
            return false;
        }
        
        public void Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value));
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (!ContainsKey(item.Key))
            {
                _items.Add(item);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) => _items.Remove(item);
        public bool Remove(TKey key)
        {
            for (var i = 0; i < _items.Count; i++)
            {
                if (_items[i].Key.Equals(key))
                {
                    _items.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
        
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    // The sorting parameters is defined like this:
    // [["sortKey1", "asc"], ["sortKey2", "desc"]]
    // Newtonsoft JSON does not support format natively (dictionaries are a JSON object, not array).
    internal class OrderedDictionaryConverter<TKey, TValue> : JsonConverter<OrderedDictionary<TKey, TValue>>
    {
        public override void WriteJson(JsonWriter writer, OrderedDictionary<TKey, TValue> value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            foreach (var kvp in value)
            {
                writer.WriteStartArray();
                serializer.Serialize(writer, kvp.Key);
                serializer.Serialize(writer, kvp.Value);
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }

        /// <inheritdoc />
        public override OrderedDictionary<TKey, TValue> ReadJson(
            JsonReader reader,
            Type objectType,
            OrderedDictionary<TKey, TValue> existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            throw new System.NotImplementedException();
        }
    }
}
