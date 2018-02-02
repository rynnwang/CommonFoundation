using System;

namespace Beyova
{
    /// <summary>
    /// Class TimeRange
    /// </summary>
    public class TimeRange : Range<DateTime>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRange"/> class.
        /// </summary>
        public TimeRange() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRange"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public TimeRange(DateTime? from, DateTime? to) : base(from, to, true, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRange"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="duration">The duration.</param>
        public TimeRange(DateTime? from, TimeSpan? duration) : base(from, from + duration, true, false) { }
    }
}