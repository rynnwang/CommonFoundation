using Newtonsoft.Json;
using System;

namespace Beyova
{
    /// <summary>
    /// Class BinaryCapacityCriteria.
    /// </summary>
    public class BinaryCapacityCriteria
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        [JsonProperty("container")]
        public string Container { get; set; }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        [JsonProperty("ownerKey")]
        public Guid? OwnerKey { get; set; }
    }
}