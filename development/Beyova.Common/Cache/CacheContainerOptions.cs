using System.Collections.Generic;

namespace Beyova.Cache
{
    /// <summary>
    ///
    /// </summary>
    public class MemoryCacheContainerOptions<TKey> : CacheContainerOptions
    {
        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        /// <value>
        /// The capacity.
        /// </value>
        public int? Capacity { get; set; }

        /// <summary>
        /// Gets or sets the equality comparer.
        /// </summary>
        /// <value>
        /// The equality comparer.
        /// </value>
        public IEqualityComparer<TKey> EqualityComparer { get; set; }
    }
}