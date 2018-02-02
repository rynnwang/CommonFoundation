using System.Collections.Generic;

namespace Beyova.Scheduling
{
    /// <summary>
    /// class SchedulingExtension
    /// </summary>
    public static class SchedulingExtension
    {
        /// <summary>
        /// Creates the day time block indexes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="timeRange">The time range.</param>
        /// <param name="sample">The sample.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static List<T> CreateDayTimeBlockIndexes<T>(this TimeRange timeRange, IDayTimeBlockAssignable sample, TimeBlockSize size)
            where T : class, IDayTimeBlockAssignable
        {
            var result = new List<T>();

            if (timeRange != null && size != null)
            {
                foreach (var one in size.DetectBlockHit(timeRange, sample))
                {
                    result.AddIfNotNull(one as T);
                }
            }

            return result;
        }

        /// <summary>
        /// To the indexes.
        /// </summary>
        /// <param name="allocation">The allocation.</param>
        /// <param name="timeRange">The time range.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static List<SchedulingResourceDimensionAllocationIndex> ToIndexes(this SchedulingResourceDimensionAllocation allocation, TimeRange timeRange, TimeBlockSize size)
        {
            return (allocation == null || timeRange == null) ?
                new List<SchedulingResourceDimensionAllocationIndex>()
                : CreateDayTimeBlockIndexes<SchedulingResourceDimensionAllocationIndex>(
                    timeRange,
                    new SchedulingResourceDimensionAllocationIndex { },
                    size);
        }
    }
}