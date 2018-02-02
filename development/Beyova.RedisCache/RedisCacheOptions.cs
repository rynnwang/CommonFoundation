using System.Collections.Generic;

namespace Beyova.Cache
{
    /// <summary>
    ///
    /// </summary>
    public class RedisCacheOptions : CacheContainerOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether [use entity full name].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use entity full name]; otherwise, <c>false</c>.
        /// </value>
        public bool UseEntityFullName { get; set; }

        /// <summary>
        /// Gets or sets the endpoints.
        /// </summary>
        /// <value>
        /// The endpoints.
        /// </value>
        public List<UriEndpoint> Endpoints { get; set; }

        /// <summary>
        /// Gets or sets the index of the database.
        /// </summary>
        /// <value>
        /// The index of the database.
        /// </value>
        public int DatabaseIndex { get; set; }
    }
}