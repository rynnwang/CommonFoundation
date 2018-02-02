using System;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class TradeInfoBase. It defines basic required information for completing a trade. Order/Business info is not included. For full info, use <see cref="TradeInfo"/>
    /// </summary>
    public abstract class TradeInfoBase : TradeCategorizable, IIdentifier
    {
        /// <summary>
        /// Gets or sets the trade created local stamp.
        /// </summary>
        /// <value>
        /// The trade created local stamp.
        /// </value>
        public DateTime? TradeCreatedLocalStamp { get; set; }

        /// <summary>
        /// Gets or sets the trade UTC stamp.
        /// </summary>
        /// <value>
        /// The trade UTC stamp.
        /// </value>
        public DateTime? TradeCreatedUtcStamp
        {
            get
            {
                return TradeCreatedLocalStamp?.ToUniversalTime();
            }
            set
            {
                //do nothing. Keep set is to avoid serialization issue.
            }
        }

        /// <summary>
        /// Gets or sets the trade last updated local stamp.
        /// </summary>
        /// <value>
        /// The trade last updated local stamp.
        /// </value>
        public DateTime? TradeLastUpdatedLocalStamp { get; set; }

        /// <summary>
        /// Gets or sets the trade last updated UTC stamp.
        /// </summary>
        /// <value>
        /// The trade last updated UTC stamp.
        /// </value>
        public DateTime? TradeLastUpdatedUtcStamp
        {
            get
            {
                return TradeLastUpdatedLocalStamp?.ToUniversalTime();
            }
            set
            {
                //do nothing. Keep set is to avoid serialization issue.
            }
        }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        /// <value>
        /// The serial number.
        /// </value>
        public string SerialNumber
        {
            get
            {
                return string.Format("{0}");
            }
            set
            {
                //do nothing. Keep set is to avoid serialization issue.
            }
        }

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
        public TradeState TradeState { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid? Key { get; set; }
    }
}