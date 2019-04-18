namespace Beyova.Scheduling
{
    /// <summary>
    /// Class SchedulingResourceDimensionAllocationIndex, it is used to categorize allocation into time blocks, which would improve confliect detection speed.
    /// </summary>
    public class SchedulingResourceDimensionAllocationIndex : SchedulingResourceDimensionAllocationEssential, IDayTimeBlockAssignable
    {
        /// <summary>
        /// Gets or sets the UTC date.
        /// </summary>
        /// <value>
        /// The UTC date.
        /// </value>
        public Date UtcDate { get; set; }

        /// <summary>
        /// Gets or sets the index of the day time block. Index of time block in <see cref="UtcDate"/>.
        /// </summary>
        /// <value>
        /// The index of the day time page.
        /// </value>
        public int DayTimeBlockIndex { get; set; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}