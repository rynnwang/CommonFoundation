using System;

namespace Beyova.Cache
{
    /// <summary>
    /// Class MemoryCacheHourlyStatistic
    /// </summary>
    public class MemoryCacheHourlyStatistic
    {
        /// <summary>
        /// Gets or sets the hour identifier.
        /// </summary>
        /// <value>
        /// The hour identifier.
        /// </value>
        public string HourIdentifier { get; protected set; }

        /// <summary>
        /// Gets or sets the hit attempt count.
        /// </summary>
        /// <value>
        /// The hit attempt count.
        /// </value>
        public ulong HitAttemptCount { get; set; }

        /// <summary>
        /// Gets or sets the total attempt count.
        /// </summary>
        /// <value>
        /// The total attempt count.
        /// </value>
        public ulong TotalAttemptCount { get; set; }

        /// <summary>
        /// Gets or sets the failur retrieval count.
        /// </summary>
        /// <value>
        /// The failur retrieval count.
        /// </value>
        public ulong FailurRetrievalCount { get; set; }

        /// <summary>
        /// Gets the expried stamp.
        /// </summary>
        /// <value>
        /// The expried stamp.
        /// </value>
        internal DateTime ExpriedStamp { get; private set; }

        /// <summary>
        /// Gets the hit percentage.
        /// </summary>
        /// <value>
        /// The hit percentage.
        /// </value>
        public string HitPercentage { get { return MemoryCacheStatistic.ToPercentage(HitAttemptCount, TotalAttemptCount); } }

        /// <summary>
        /// Gets the failure percentage.
        /// </summary>
        /// <value>
        /// The failure percentage.
        /// </value>
        public string FailurePercentage { get { return MemoryCacheStatistic.ToPercentage(FailurRetrievalCount, TotalAttemptCount); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheHourlyStatistic"/> class.
        /// </summary>
        /// <param name="hourIdentifier">The hour identifier.</param>
        /// <param name="expriedStamp">The expried stamp.</param>
        internal MemoryCacheHourlyStatistic(string hourIdentifier, DateTime expriedStamp)
        {
            this.HourIdentifier = hourIdentifier;
            this.ExpriedStamp = expriedStamp;
        }

        /// <summary>
        /// Adds the hit.
        /// </summary>
        /// <param name="isFailure">if set to <c>true</c> [is failure].</param>
        /// <param name="isHit">if set to <c>true</c> [is hit].</param>
        public void AddHit(bool isFailure, bool isHit)
        {
            this.TotalAttemptCount++;
            if (isFailure)
            {
                this.FailurRetrievalCount++;
            }
            if (isHit)
            {
                this.HitAttemptCount++;
            }
        }
    }
}