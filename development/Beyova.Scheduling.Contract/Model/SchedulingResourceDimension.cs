using System;

namespace Beyova.Scheduling
{
    /// <summary>
    /// class SchedulingResourceDimension. It defines scheduling resource dimension, which supports to create entity instances. Typical resource demension is such as room, bus, people, etc.
    /// </summary>
    public class SchedulingResourceDimension : IIdentifier, ICodeIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}