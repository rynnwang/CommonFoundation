using System;

namespace Beyova.Scheduling
{
    /// <summary>
    /// Class SchedulingResourceDimensionAllocation, it maps container with all kinds of resource dimension.
    /// </summary>
    public class SchedulingResourceDimensionAllocation : SchedulingResourceDimensionAllocationEssential
    {
        /// <summary>
        /// Gets or sets the container key.
        /// </summary>
        /// <value>
        /// The container key.
        /// </value>
        public Guid? ContainerKey { get; set; }

        /// <summary>
        /// Gets or sets the container code.
        /// </summary>
        /// <value>
        /// The container code.
        /// </value>
        public string ContainerCode { get; set; }
    }
}