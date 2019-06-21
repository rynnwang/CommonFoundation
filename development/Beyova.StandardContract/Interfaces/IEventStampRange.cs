using System;

namespace Beyova
{
    /// <summary>
    /// Interface IEventStampRange
    /// </summary>
    public interface IEventStampRange
    {
        /// <summary>
        /// Gets or sets the start stamp.
        /// </summary>
        /// <value>
        /// The start stamp.
        /// </value>
        DateTime? StartStamp { get; set; }

        /// <summary>
        /// Gets or sets the end stamp.
        /// </summary>
        /// <value>
        /// The end stamp.
        /// </value>
        DateTime? EndStamp { get; set; }
    }
}