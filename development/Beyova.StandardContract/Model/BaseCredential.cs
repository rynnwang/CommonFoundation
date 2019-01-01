using System;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class BaseCredential.
    /// </summary>
    public class BaseCredential : ICredential
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [JsonProperty("key")]
        public Guid? Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCredential"/> class.
        /// </summary>
        public BaseCredential() { }
    }
}