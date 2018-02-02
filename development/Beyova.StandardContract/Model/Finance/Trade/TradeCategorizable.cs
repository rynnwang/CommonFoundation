using System;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class TradeCategorizable. It defines very key information for grouping trade records. It is used for both Entity and Criteria
    /// </summary>
    public abstract class TradeCategorizable
    {
        /// <summary>
        /// Gets or sets the buyer identifier.
        /// </summary>
        /// <value>
        /// The buyer identifier.
        /// </value>
        public OrganizationUserIdentifier BuyerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the seller identifier.
        /// </summary>
        /// <value>
        /// The seller identifier.
        /// </value>
        public OrganizationUserIdentifier SellerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the payment party identifier.
        /// </summary>
        /// <value>
        /// The payment party identifier.
        /// </value>
        public string PaymentPartyIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the payment party trade identifier.
        /// </summary>
        /// <value>
        /// The payment party trade identifier.
        /// </value>
        public string PaymentPartyTradeIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string Currency { get; set; }
    }
}