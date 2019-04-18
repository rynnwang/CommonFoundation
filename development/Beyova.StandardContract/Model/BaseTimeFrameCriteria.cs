using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TFilters">The type of the filters.</typeparam>
    /// <typeparam name="TGroupBy">The type of the group by.</typeparam>
    public abstract class BaseTimeFrameCriteria<TFilters, TGroupBy> : BaseTimeFrameCriteria
    {
        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        [JsonProperty(PropertyName = "filters")]
        public TFilters Filters { get; set; }

        /// <summary>
        /// Gets or sets the group by.
        /// </summary>
        /// <value>
        /// The group by.
        /// </value>
        [JsonProperty(PropertyName = "groupBy")]
        public TGroupBy GroupBy { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BaseTimeFrameCriteria
    {
        /// <summary>
        /// Gets or sets the time frame.
        /// </summary>
        /// <value>
        /// The time frame.
        /// </value>
        [JsonProperty(PropertyName = "timeFrame")]
        public TimeFrame TimeFrame { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTimeFrameCriteria"/> class.
        /// </summary>
        public BaseTimeFrameCriteria() : base()
        {
        }
    }
}