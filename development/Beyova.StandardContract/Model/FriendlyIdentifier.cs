using Newtonsoft.Json;
using System;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class FriendlyIdentifier : FriendlyIdentifier<Guid?>, IFriendlyIdentifier
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class FriendlyIdentifier<TKey>
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonProperty("key")]
        public TKey Key { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}