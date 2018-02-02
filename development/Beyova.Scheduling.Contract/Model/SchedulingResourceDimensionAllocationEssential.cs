using System;

namespace Beyova.Scheduling
{
    /// <summary>
    /// Class SchedulingResourceDimensionAllocationEssential
    /// </summary>
    public abstract class SchedulingResourceDimensionAllocationEssential : IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the dimension key.
        /// </summary>
        /// <value>
        /// The dimension key.
        /// </value>
        public Guid? DimensionKey { get; set; }

        /// <summary>
        /// Gets or sets the dimension code.
        /// </summary>
        /// <value>
        /// The dimension code.
        /// </value>
        public string DimensionCode { get; set; }

        /// <summary>
        /// Gets or sets the dimension entity identifier.
        /// </summary>
        /// <value>
        /// The dimension entity identifier.
        /// </value>
        public string DimensionEntityIdentifier { get; set; }
    }
}