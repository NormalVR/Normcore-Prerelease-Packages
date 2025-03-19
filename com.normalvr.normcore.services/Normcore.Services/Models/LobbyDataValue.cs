using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Normcore.Services
{
    /// <summary>
    /// The types of data allowed in a <see cref="LobbyDataValue"/>.
    /// </summary>
    public enum LobbyDataValueType
    {
        Invalid = 0,
        Boolean = 1,
        Number = 2,
        String = 3,
    }

    /// <summary>
    /// A data value which can be stored in a <see cref="LobbyDataContainer"/>.
    /// </summary>
    [JsonConverter(typeof(LobbyDataValueConverter))]
    public readonly struct LobbyDataValue : IEquatable<LobbyDataValue>
    {
        /// <summary>
        /// The data value type.
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

            BoolValue = value;
            StringValue = null;
            NumberValue = null;
        }

        public LobbyDataValue(long value)
        {
            Type = LobbyDataValueType.Number;

            BoolValue = null;
            NumberValue = value;
            StringValue = null;
        }

        public LobbyDataValue(double value)
        {
            Type = LobbyDataValueType.Number;

            BoolValue = null;
            NumberValue = value;
            StringValue = null;
        }

        public LobbyDataValue(string value)
        {
            Type = LobbyDataValueType.String;

            BoolValue = null;
            NumberValue = null;
            StringValue = value;
        }

        /// <summary>
        /// Returns the value as a bool.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the value is not a boolean type.</exception>
        public bool AsBool()
        {
            if (!BoolValue.HasValue)
            {
                throw new InvalidOperationException("The value is not a boolean type.");
            }

            return BoolValue.Value;
        }

        /// <summary>
        /// Returns the number value as a double.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the value is not a number type.</exception>
        public double AsDouble()
        {
            if (!NumberValue.HasValue)
            {
                throw new InvalidOperationException("The value is not a number type.");
            }

            return NumberValue.Value;
        }

        /// <summary>
        /// Returns the string value.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the value is not a string type.</exception>
        public string AsString()
        {
            if (StringValue == null)
            {
                throw new InvalidOperationException("The value is not a string type.");
            }

            return StringValue;
        }

        private void AssertValidUnion()
        {
            Debug.Assert(Type == LobbyDataValueType.Invalid || BoolValue.HasValue != NumberValue.HasValue != (StringValue != null));
        }

        /// <inheritdoc />
        public bool Equals(LobbyDataValue other)
        {
            return Type switch
            {
                LobbyDataValueType.Invalid when other.Type == LobbyDataValueType.Invalid => true,
                LobbyDataValueType.Boolean when other.Type == LobbyDataValueType.Boolean => BoolValue == other.BoolValue,
                LobbyDataValueType.Number  when other.Type == LobbyDataValueType.Number  => NumberValue == other.NumberValue,
                LobbyDataValueType.String  when other.Type == LobbyDataValueType.String  => StringValue == other.StringValue,
                _                                                              => false,
            };
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is LobbyDataValue other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine((int)Type, BoolValue, NumberValue, StringValue);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            AssertValidUnion();

            if (BoolValue.HasValue)
            {
                return $"{nameof(LobbyDataValue)}({BoolValue.Value})";
            }

            if (NumberValue.HasValue)
            {
                return $"{nameof(LobbyDataValue)}({NumberValue.Value})";
            }

            if (StringValue != null)
            {
                return $"{nameof(LobbyDataValue)}(\"{StringValue}\")";
            }

            throw new Exception($"The {nameof(LobbyDataValue)} is invalid.");
        }

        public static bool operator ==(LobbyDataValue left, LobbyDataValue right) => left.Equals(right);
        public static bool operator !=(LobbyDataValue left, LobbyDataValue right) => !left.Equals(right);
        
        public static implicit operator LobbyDataValue(bool value) => new LobbyDataValue(value);
        public static implicit operator LobbyDataValue(long value) => new LobbyDataValue(value);
        public static implicit operator LobbyDataValue(double value) => new LobbyDataValue(value);
        public static implicit operator LobbyDataValue(string value) => new LobbyDataValue(value);
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
                throw new Exception($"The {nameof(LobbyDataValue)} is invalid");
            }
        }

        public override LobbyDataValue ReadJson(JsonReader reader, Type objectType, LobbyDataValue existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return reader.Value switch
            {
                bool b => new LobbyDataValue(b),
                long n => new LobbyDataValue(n),
                double d => new LobbyDataValue(d),
                string s => new LobbyDataValue(s),
                _ => throw new ArgumentException($"Unexpected token type while reading data value: {reader.TokenType}"),
            };
        }
    }
}
