using Beyova.Diagnostic;
using Newtonsoft.Json;
using System;

namespace Beyova
{
    /// <summary>
    /// Class DateConverter
    /// </summary>
    public class DateConverter : JsonConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateConverter"/> class.
        /// </summary>
        public DateConverter() : base()
        {
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Date) || objectType == typeof(Date?);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var stringValue = reader.Value.SafeToString();

            var date = string.IsNullOrWhiteSpace(stringValue) ? default(Date?) : Date.FromDateString(stringValue);
            if (objectType == typeof(Date))
            {
                if (date.HasValue)
                {
                    return date.Value;
                }
                else
                {
                    throw new InvalidObjectException(nameof(stringValue));
                }
            }

            return date;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}