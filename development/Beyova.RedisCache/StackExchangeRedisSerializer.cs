using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Cache
{
    /// <summary>
    /// 
    /// </summary>
    internal class StackExchangeRedisSerializer : StackExchange.Redis.Extensions.Core.ISerializer
    {
        /// <summary>
        /// Deserializes the specified serialized object.
        /// </summary>
        /// <param name="serializedObject">The serialized object.</param>
        /// <returns></returns>
        public object Deserialize(byte[] serializedObject)
        {
            return serializedObject.ToString(Encoding.UTF8).TryConvertJsonToObject<JToken>();
        }

        /// <summary>
        /// Deserializes the specified serialized object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject">The serialized object.</param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] serializedObject)
        {
            return serializedObject.ToString(Encoding.UTF8).TryConvertJsonToObject<T>();
        }

        /// <summary>
        /// Deserializes the asynchronous.
        /// </summary>
        /// <param name="serializedObject">The serialized object.</param>
        /// <returns></returns>
        public async Task<object> DeserializeAsync(byte[] serializedObject)
        {
            return await Task.Factory.StartNew(() => Deserialize(serializedObject));
        }

        /// <summary>
        /// Deserializes the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject">The serialized object.</param>
        /// <returns></returns>
        public async Task<T> DeserializeAsync<T>(byte[] serializedObject)
        {
            return await Task.Factory.StartNew(() => Deserialize<T>(serializedObject));
        }

        /// <summary>
        /// Serializes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public byte[] Serialize(object item)
        {
            return item.ToJson(false).ToByteArray(Encoding.UTF8);
        }

        /// <summary>
        /// Serializes the asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<byte[]> SerializeAsync(object item)
        {
            return await Task.Factory.StartNew(() => Serialize(item));
        }
    }
}
