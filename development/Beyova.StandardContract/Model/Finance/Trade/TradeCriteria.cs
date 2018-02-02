using System;

namespace Beyova
{
    /// <summary>
    /// Class TradeBase
    /// </summary>
    public class TradeCriteria : TradeCategorizable, IPageable
    {
        /// <summary>
        /// Gets or sets the trade created UTC time from.
        /// </summary>
        /// <value>
        /// The trade created UTC time from.
        /// </value>
        public DateTime? TradeCreatedUtcTimeFrom { get; set; }

        /// <summary>
        /// Gets or sets the trade UTC time to.
        /// </summary>
        /// <value>
        /// The trade UTC time to.
        /// </value>
        public DateTime? TradeCreatedUtcTimeTo { get; set; }

        /// <summary>
        /// Gets or sets the trade last updated UTC time from.
        /// </summary>
        /// <value>
        /// The trade last updated UTC time from.
        /// </value>
        public DateTime? TradeLastUpdatedUtcTimeFrom { get; set; }

        /// <summary>
        /// Gets or sets the trade last updated UTC time to.
        /// </summary>
        /// <value>
        /// The trade last updated UTC time to.
        /// </value>
        public DateTime? TradeLastUpdatedUtcTimeTo { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Gets or sets the state of the trade.
        /// </summary>
        /// <value>
        /// The state of the trade.
        /// </value>
        public TradeState? TradeState { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the start index.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        public int StartIndex { get; set; }
    }
}