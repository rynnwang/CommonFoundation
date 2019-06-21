using System;

namespace Beyova
{
    /// <summary>
    /// Interface IEventDateRange
    /// </summary>
    public interface IEventDateRange
    {

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        Date? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        Date? EndDate { get; set; }
    }
}