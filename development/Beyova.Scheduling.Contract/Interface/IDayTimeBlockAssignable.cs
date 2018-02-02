using System;

namespace Beyova.Scheduling
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDayTimeBlockAssignable : ICloneable
    {
        /// <summary>
        /// Gets or sets the UTC date.
        /// </summary>
        /// <value>
        /// The UTC date.
        /// </value>
        Date UtcDate { get; set; }

        /// <summary>
        /// Gets or sets the index of the day time block. Index of time block in <see cref="UtcDate"/>.
        /// </summary>
        /// <value>
        /// The index of the day time page.
        /// </value>
        int DayTimeBlockIndex { get; set; }
    }
}
