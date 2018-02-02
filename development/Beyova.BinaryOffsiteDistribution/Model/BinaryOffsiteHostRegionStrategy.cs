using System;

namespace Beyova
{
    /// <summary>
    /// Class BinaryOffsiteHostRegionStrategy.
    /// </summary>
    public class BinaryOffsiteHostRegionStrategy
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the host region.
        /// </summary>
        /// <value>The host region.</value>
        public string HostRegion { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public string CountryCode { get; set; }
    }
}