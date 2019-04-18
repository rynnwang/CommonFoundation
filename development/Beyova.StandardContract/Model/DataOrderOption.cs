using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class DataOrderOption.
    /// </summary>
    public sealed class DataOrderOption
    {
        /// <summary>
        ///
        /// </summary>
        public enum OrderMethod
        {
            /// <summary>
            /// The ascending
            /// </summary>
            Ascending = 0,
            /// <summary>
            /// The descending
            /// </summary>
            Descending = 1
        }

        /// <summary>
        /// Gets or sets the by.
        /// </summary>
        /// <value>
        /// The by.
        /// </value>
        [JsonProperty(PropertyName = "by")]
        public string By { get; set; }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        [JsonProperty(PropertyName = "method")]
        public OrderMethod Method { get; set; }
    }
}