using System;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public class UnixStampConverter : JsonConverter
    {
        /// <summary>
        ///
        /// </summary>
        public enum UnixStampAccuracy
        {
            /// <summary>
            /// The millisecond
            /// </summary>
            Millisecond = 0,
            /// <summary>
            /// The second
            /// </summary>
            Second = 1,
            /// <summary>
            /// The minute
            /// </summary>
            Minute = 2
        }

        /// <summary>
        /// Gets or sets the accuracy.
        /// </summary>
        /// <value>
        /// The accuracy.
        /// </value>
        public UnixStampAccuracy Accuracy { get; protected set; }

        /// <summary>
        /// Gets the magnification by millisecond.
        /// </summary>
        /// <value>
        /// The magnification by millisecond.
        /// </value>
        private int MagnificationByMillisecond
        {
            get
            {
                switch (Accuracy)
                {
                    case UnixStampAccuracy.Second:
                        return 1000;

                    case UnixStampAccuracy.Minute:
                        return 60000;

                    case UnixStampAccuracy.Millisecond:
                    default:
                        return 1;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnixStampConverter" /> class.
        /// </summary>
        /// <param name="accuracy">The accuracy. It defines how accurate of serialized long value stands for time (seconds? milliseconds? ,etc.) from 1970-1-1. </param>
        public UnixStampConverter(UnixStampAccuracy accuracy = UnixStampConverter.UnixStampAccuracy.Millisecond) : base()
        {
            Accuracy = accuracy;
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
            return objectType == typeof(DateTime?) || objectType == typeof(DateTime);
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
            if (objectType.IsNullable())
            {
                var unixTotalSeconds = reader.Value.SafeToString().ObjectToNullableInt64();
                return unixTotalSeconds.HasValue ? CommonExtension.UnixMillisecondsToDateTime(unixTotalSeconds.Value * MagnificationByMillisecond, DateTimeKind.Utc) as DateTime? : null;
            }
            else
            {
                var unixTotalSeconds = reader.Value.SafeToString().ObjectToInt64();
                return CommonExtension.UnixMillisecondsToDateTime(unixTotalSeconds * MagnificationByMillisecond, DateTimeKind.Utc);
            }
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dateTime = value as DateTime?;
            if (dateTime.HasValue)
            {
                writer.WriteValue((long)(dateTime.Value.ToUnixMillisecondsDateTime() / MagnificationByMillisecond));
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}