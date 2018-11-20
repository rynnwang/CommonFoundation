using System.Text;
using Newtonsoft.Json;

namespace Beyova.VirtualSecuredTransferProtocol
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ClassicVirtualSecuredRequestMessagePackage<T>
    {
        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        public T Object { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        /// <summary>
        /// To the bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes(this.ToJson(false));
        }

        /// <summary>
        /// Froms the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public ClassicVirtualSecuredRequestMessagePackage<T> FromBytes(byte[] bytes)
        {
            return (bytes == null ? null : Encoding.UTF8.GetString(bytes)).TryConvertJsonToObject<ClassicVirtualSecuredRequestMessagePackage<T>>();
        }
    }
}