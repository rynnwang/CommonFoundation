using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class StatisticResource : StatisticResource<long>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StatisticResource<T> : StatisticResource<string, T>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TFactor">The type of the factor.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class StatisticResource<TFactor, TValue>
    {
        /// <summary>
        /// Gets or sets the group factors.
        /// </summary>
        /// <value>
        /// The group factors.
        /// </value>
        [JsonProperty(PropertyName = "groupFactors")]
        public List<TFactor> GroupFactors { get; set; }

        /// <summary>
        /// Gets or sets the value factors.
        /// </summary>
        /// <value>
        /// The value factors.
        /// </value>
        [JsonProperty(PropertyName = "valueFactors")]
        public Dictionary<TFactor, TValue> ValueFactors { get; set; }
    }
}