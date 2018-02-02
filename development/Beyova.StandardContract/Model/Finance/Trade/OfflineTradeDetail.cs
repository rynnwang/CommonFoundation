using System;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class OfflineTradeDetail
    {
        /// <summary>
        /// Gets or sets the terminal device identifier.
        /// </summary>
        /// <value>
        /// The terminal device identifier.
        /// </value>
        public string TerminalDeviceId { get; set; }

        /// <summary>
        /// Gets or sets the store identifier.
        /// </summary>
        /// <value>
        /// The store identifier.
        /// </value>
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public string OperatorId { get; set; }
    }
}