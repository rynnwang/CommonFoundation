using System;
using System.Collections.Generic;
using System.Linq;
using Beyova.ExceptionSystem;

namespace Beyova.Cache
{
    /// <summary>
    /// Class MemoryCacheStatistic
    /// </summary>
    public class MemoryCacheStatistic : Dictionary<string, MemoryCacheHourlyStatistic>
    {
        /// <summary>
        /// The locker
        /// </summary>
        protected object locker = new object();

        /// <summary>
        /// Gets the hit percentage.
        /// </summary>
        /// <value>
        /// The hit percentage.
        /// </value>
        public string HitPercentage { get { return ToPercentage((ulong)(this.Values.Sum(x => (long)x.HitAttemptCount)), (ulong)(this.Values.Sum(x => (long)x.TotalAttemptCount))); } }

        /// <summary>
        /// Gets the failure percentage.
        /// </summary>
        /// <value>
        /// The failure percentage.
        /// </value>
        public string FailurePercentage { get { return ToPercentage((ulong)(this.Values.Sum(x => (long)x.FailurRetrievalCount)), (ulong)(this.Values.Sum(x => (long)x.TotalAttemptCount))); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheStatistic"/> class.
        /// </summary>
        internal MemoryCacheStatistic() : base(24, StringComparer.InvariantCultureIgnoreCase)
        {
        }

        /// <summary>
        /// To the percentage.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <returns></returns>
        internal static string ToPercentage(ulong numerator, ulong denominator)
        {
            return denominator > 0 ? string.Format("{0}%", (numerator * 0.01 / denominator).ToString(".00")) : "NA";
        }

        /// <summary>
        /// Gets the hour identifier.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        protected static string GetHourIdentifier(DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHH");
        }

        /// <summary>
        /// Adds the hit.
        /// </summary>
        /// <param name="isFailure">if set to <c>true</c> [is failure].</param>
        /// <param name="isHit">if set to <c>true</c> [is hit].</param>
        public void AddHit(bool isFailure, bool isHit)
        {
            var nowTime = DateTime.UtcNow;
            var hourlyIdentifier = GetHourIdentifier(nowTime);
            MemoryCacheHourlyStatistic statistic = null;

            if (!this.TryGetValue(hourlyIdentifier, out statistic))
            {
                lock (locker)
                {
                    if (!this.TryGetValue(hourlyIdentifier, out statistic))
                    {
                        //clean expired item first
                        this.Remove(x => x.Value.ExpriedStamp < nowTime);
                        statistic = new MemoryCacheHourlyStatistic(hourlyIdentifier, nowTime.ResetMinute().ResetSecond());
                        this.Add(hourlyIdentifier, statistic);
                    }
                }
            }

            statistic.AddHit(isFailure, isHit);
        }
    }
}