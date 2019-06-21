using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class KVMetaDictionary : Dictionary<string, JValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KVMetaDictionary"/> class.
        /// </summary>
        public KVMetaDictionary() : base(System.StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Tries the get value as.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T TryGetValueAs<T>(string key)
        {
            JValue result = null;
            return TryGetValue(key, out result) ? result.ToObject<T>() : default(T);
        }

        /// <summary>
        /// Standardizes the data.
        /// </summary>
        public void StandardizeData()
        {
            HashSet<string> fieldsToDelete = new HashSet<string>();

            foreach (var item in this)
            {
                if (item.Value.Type == JTokenType.Null
                    || item.Value.Type == JTokenType.Undefined
                    || (item.Value.Type == JTokenType.String && string.IsNullOrEmpty(item.Value.ToObject<string>())))
                {
                    fieldsToDelete.Add(item.Key);
                }
            }

            foreach (var item in fieldsToDelete)
            {
                Remove(item);
            }
        }
    }
}