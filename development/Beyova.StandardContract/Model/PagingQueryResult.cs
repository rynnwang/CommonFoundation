using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public abstract class PagingQueryResult<TObject> : PagingQueryResult<int, TObject>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TStartIndex">The type of the start index.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public abstract class PagingQueryResult<TStartIndex, TObject>
    {
        /// <summary>
        /// Gets or sets the start index.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        [JsonProperty(PropertyName = "startIndex")]
        public TStartIndex StartIndex { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        [JsonProperty(PropertyName = "totalCount")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [JsonProperty(PropertyName = "items")]
        public List<TObject> Items { get; set; }
    }
}