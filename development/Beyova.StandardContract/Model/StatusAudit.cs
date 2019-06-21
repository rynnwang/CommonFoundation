using Newtonsoft.Json;
using System;

namespace Beyova
{
    /// <summary>
    /// Class StatusAudit.
    /// </summary>
    public class StatusAudit : IStatusAudit
    {
        /// <summary>
        /// Gets or sets the operated by.
        /// </summary>
        /// <value>
        /// The operated by.
        /// </value>
        [JsonProperty("operatedBy")]
        public Guid? OperatedBy { get; set; }

        /// <summary>
        /// Gets or sets the stamp.
        /// </summary>
        /// <value>
        /// The stamp.
        /// </value>
        [JsonProperty("stamp")]
        public DateTime? Stamp { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        [JsonProperty("comments")]
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty("status")]
        public int Status { get; set; }
    }
}