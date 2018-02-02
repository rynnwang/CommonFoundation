namespace Beyova.Cache
{
    /// <summary>
    /// Class MemoryCacheContainerSummary.
    /// </summary>
    public class MemoryCacheContainerSummary : IMemoryCacheContainer
    {
        /// <summary>
        /// The capacity
        /// </summary>
        /// <value>The capacity.</value>
        public int? Capacity { get; set; }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }

        /// <summary>
        /// The expiration in second
        /// </summary>
        /// <value>The expiration in second.</value>
        public long? ExpirationInSecond { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the statistic.
        /// </summary>
        /// <value>
        /// The statistic.
        /// </value>
        public MemoryCacheStatistic Statistic { get; set; }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            //Do nothing.
        }
    }
}