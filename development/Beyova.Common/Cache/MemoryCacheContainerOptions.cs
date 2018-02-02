using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;

namespace Beyova.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheContainerOptions
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the expiration in second.
        /// </summary>
        /// <value>
        /// The expiration in second.
        /// </value>
        public long? ExpirationInSecond { get; set; }
    }
}