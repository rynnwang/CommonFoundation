using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class TradeInfo
    /// </summary>
    public class TradeInfo : TradeInfo<JToken, JToken, JToken>
    {
    }

    /// <summary>
    /// Class TradeInfo
    /// </summary>
    public abstract class TradeInfo<TBusinessDetail, TDiscount, TVoucher> : TradeInfoBase
    {
        /// <summary>
        /// Gets or sets the offline trade detail.
        /// </summary>
        /// <value>
        /// The offline trade detail.
        /// </value>
        public OfflineTradeDetail OfflineTradeDetail { get; set; }

        /// <summary>
        /// Gets or sets the balance currency.
        /// </summary>
        /// <value>
        /// The balance currency.
        /// </value>
        public string BalanceCurrency { get; set; }

        /// <summary>
        /// Gets or sets the currency exchange rate.
        /// </summary>
        /// <value>
        /// The currency exchange rate.
        /// </value>
        public decimal CurrencyExchangeRate { get; set; }

        /// <summary>
        /// Gets or sets the balance amount.
        /// </summary>
        /// <value>
        /// The balance amount.
        /// </value>
        public decimal? BalanceAmount { get; set; }

        /// <summary>
        /// Gets or sets the business detail.
        /// </summary>
        /// <value>
        /// The business detail.
        /// </value>
        public TBusinessDetail BusinessDetail { get; set; }

        /// <summary>
        /// Gets or sets the discount.
        /// </summary>
        /// <value>
        /// The discount.
        /// </value>
        public TDiscount Discount { get; set; }

        /// <summary>
        /// Gets or sets the voucher. (优惠券)
        /// </summary>
        /// <value>
        /// The voucher.
        /// </value>
        public TVoucher Voucher { get; set; }

        /// <summary>
        /// Gets or sets the refund expired stamp.
        /// </summary>
        /// <value>
        /// The refund expired stamp.
        /// </value>
        public DateTime? RefundExpiredStamp { get; set; }

        /// <summary>
        /// Gets or sets the trade expired stamp.
        /// </summary>
        /// <value>
        /// The trade expired stamp.
        /// </value>
        public DateTime? TradeExpiredStamp { get; set; }
    }
}