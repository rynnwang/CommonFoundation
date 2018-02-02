using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class MappingTableConverter. Supports enum and nullable enum.
    /// </summary>
    public class MappingTableConverter : JsonConverter
    {
        /// <summary>
        /// The property name value unique
        /// </summary>
        protected const string propertyName_ValueUnique = "valueUnique";

        /// <summary>
        /// The property name case sensitive
        /// </summary>
        protected const string propertyName_CaseSensitive = "caseSensitive";

        /// <summary>
        /// The property name items
        /// </summary>
        protected const string propertyName_Items = "items";

        /// <summary>
        /// The property name source
        /// </summary>
        protected const string propertyName_Source = "s";

        /// <summary>
        /// The property name mapping
        /// </summary>
        protected const string propertyName_Mapping = "m";

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingTableConverter"/> class.
        /// </summary>
        public MappingTableConverter()
            : base()
        {
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise,
        /// <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(MappingTable<>).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IMappingTable table = null;
            bool? caseSensitive = false, valueUnique = false;
            JArray items = null;
            Type valueType = null;

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    switch (reader.Value)
                    {
                        case propertyName_CaseSensitive:
                            caseSensitive = reader.ReadAsBoolean();
                            break;
                        case propertyName_ValueUnique:
                            valueUnique = reader.ReadAsBoolean();
                            break;
                        case propertyName_Items:
                            reader.Read();
                            items = reader.ReadAsArray();
                            break;
                        default:
                            break;
                    }
                }
            }

            if (caseSensitive.HasValue && items != null)
            {
                if (objectType == typeof(MappingTable))
                {
                    table = Activator.CreateInstance(objectType, valueUnique, caseSensitive) as IMappingTable;
                    valueType = typeof(string);
                }
                else if (objectType.EqualsByUnderplyingGenericType(typeof(MappingTable<>)))
                {
                    table = Activator.CreateInstance(objectType, valueUnique, caseSensitive, null) as IMappingTable;
                    valueType = objectType.GetGenericArguments().SafeFirstOrDefault();
                }

                if (table != null && valueType != null)
                {
                    foreach (var item in items)
                    {
                        var key = item.Value<string>(propertyName_Source);
                        var value = item.SelectToken(propertyName_Mapping).ToObject(valueType);
                        table.Add(key, value);
                    }
                }
            }

            return table;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var instance = value as IMappingTable;
            if (instance != null)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(propertyName_ValueUnique);
                writer.WriteValue(instance.ValueUnique);

                writer.WritePropertyName(propertyName_CaseSensitive);
                writer.WriteValue(instance.CaseSensitive);

                writer.WritePropertyName(propertyName_Items);
                writer.WriteStartArray();

                var enumerator = instance.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName(propertyName_Source);
                    writer.WriteValue(enumerator.Key);

                    writer.WritePropertyName(propertyName_Mapping);
                    writer.WriteValue(enumerator.Value);
                    writer.WriteEndObject();
                }

                writer.WriteEndArray();
                writer.WriteEndObject();
            }
        }
    }
}