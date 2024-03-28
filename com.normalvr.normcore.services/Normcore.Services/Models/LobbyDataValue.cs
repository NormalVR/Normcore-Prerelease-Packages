using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Normcore.Services
{
    public enum LobbyDataValueType
    {
        Invalid = 0,
        Boolean = 1,
        Number = 2,
        String = 3,
    }

    [JsonConverter(typeof(LobbyDataValueConverter))]
    public readonly struct LobbyDataValue
    {
        /// <summary>
        /// The lobby data value type.
        /// </summary>
        public readonly LobbyDataValueType Type;

        /// <summary>
        /// The bool value, if any.
        /// </summary>
        public readonly bool? BoolValue;

        /// <summary>
        /// The number value, if any.
        /// </summary>
        public readonly double? NumberValue;

        /// <summary>
        /// The string value, if any.
        /// </summary>
        public readonly string StringValue;

        public LobbyDataValue(bool value)
        {
            Type = LobbyDataValueType.Boolean;

            BoolValue   = value;
            StringValue = null;
            NumberValue = null;
        }

        public LobbyDataValue(double value)
        {
            Type = LobbyDataValueType.Number;

            BoolValue   = null;
            NumberValue = value;
            StringValue = null;
        }

        public LobbyDataValue(long value)
        {
            Type = LobbyDataValueType.Number;

            BoolValue   = null;
            NumberValue = value;
            StringValue = null;
        }

        public LobbyDataValue(string value)
        {
            Type = LobbyDataValueType.String;

            BoolValue   = null;
            NumberValue = null;
            StringValue = value;
        }

        /// <summary>
        /// Returns the value as a bool. Throws an exception if the value is not a boolean type.
        /// </summary>
        public bool AsBool()
        {
            if (!BoolValue.HasValue)
            {
                throw new Exception("The value is not a boolean type.");
            }

            return BoolValue.Value;
        }

        /// <summary>
        /// Returns the number value as a double. Throws an exception if the value is not a number type.
        /// </summary>
        public double AsDouble()
        {
            if (!NumberValue.HasValue)
            {
                throw new Exception("The value is not a number type.");
            }

            return NumberValue.Value;
        }

        /// <summary>
        /// Returns the string value. Throws an exception if the value is not a string type.
        /// </summary>
        public string AsString()
        {
            if (StringValue == null)
            {
                throw new Exception("The value is not a string type.");
            }

            return StringValue;
        }

        private void AssertValidUnion()
        {
            Debug.Assert(Type == LobbyDataValueType.Invalid || BoolValue.HasValue != NumberValue.HasValue != (StringValue != null));
        }

        public override string ToString()
        {
            AssertValidUnion();

            if (BoolValue.HasValue)
            {
                return $"LobbyDataValue({BoolValue.Value})";
            }

            if (NumberValue.HasValue)
            {
                return $"LobbyDataValue({NumberValue.Value})";
            }

            if (StringValue != null)
            {
                return $"LobbyDataValue(\"{StringValue}\")";
            }

            throw new Exception("The LobbyDataValue is invalid.");
        }
    }

    internal class LobbyDataValueConverter : JsonConverter<LobbyDataValue>
    {
        public override void WriteJson(JsonWriter writer, LobbyDataValue value, JsonSerializer serializer)
        {
            if (value.BoolValue.HasValue)
            {
                writer.WriteValue(value.BoolValue);
            }
            else if (value.NumberValue.HasValue)
            {
                writer.WriteValue(value.NumberValue);
            }
            else if (value.StringValue != null)
            {
                writer.WriteValue(value.StringValue);
            }
            else
            {
                throw new Exception("The LobbyDataValue is invalid");
            }
        }

        public override LobbyDataValue ReadJson(JsonReader reader, Type objectType, LobbyDataValue existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is bool b)
            {
                return new LobbyDataValue(b);
            }

            if (reader.Value is long n)
            {
                return new LobbyDataValue(n);
            }

            if (reader.Value is double d)
            {
                return new LobbyDataValue(d);
            }

            if (reader.Value is string s)
            {
                return new LobbyDataValue(s);
            }

            throw new ArgumentException($"Unexpected token type while reading lobby data value: {reader.TokenType}");
        }
    }
}
