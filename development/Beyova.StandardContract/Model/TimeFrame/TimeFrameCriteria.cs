using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class TimeFrameCriteria : BaseTimeFrameCriteria, IStampCriteria
    {
        /// <summary>
        /// Gets or sets from stamp.
        /// </summary>
        /// <value>
        /// From stamp.
        /// </value>
        [JsonProperty(PropertyName = "fromStamp")]
        public DateTime? FromStamp { get; set; }

        /// <summary>
        /// Converts to stamp.
        /// </summary>
        /// <value>
        /// To stamp.
        /// </value>
        [JsonProperty(PropertyName = "toStamp")]
        public DateTime? ToStamp { get; set; }

        /// <summary>
        /// Gets or sets the group by.
        /// </summary>
        /// <value>
        /// The group by.
        /// </value>
        [JsonProperty(PropertyName = "groupBy")]
        public string[] GroupBy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeFrameCriteria"/> class.
        /// </summary>
        public TimeFrameCriteria() : base()
        {
        }
    }
}