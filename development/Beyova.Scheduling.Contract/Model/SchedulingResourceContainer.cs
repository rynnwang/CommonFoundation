using System;
using System.Collections.Generic;

namespace Beyova.Scheduling
{
    /// <summary>
    /// class SchedulingResourceContainer. All resource conflicts would be calculated based on container.
    /// </summary>
    public class SchedulingResourceContainer : SimpleBaseObject
    {
        /// <summary>
        /// Gets or sets the weight capacity.
        /// </summary>
        /// <value>
        /// The weight capacity.
        /// </value>
        public Dictionary<Guid, double?> WeightCapacity { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the time range.
        /// </summary>
        /// <value>
        /// The time range.
        /// </value>
        public TimeRange TimeRange { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public SchedulingOptions Options { get; set; }
    }
}