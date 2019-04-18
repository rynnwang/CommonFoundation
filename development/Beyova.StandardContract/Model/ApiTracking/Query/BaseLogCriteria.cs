using System;
using Newtonsoft.Json;

namespace Beyova.Diagnostic
{
    /// <summary>
    ///
    /// </summary>
    public class BaseLogCriteria : IStampCriteria, ICriteria, IServiceIdentifiable
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
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonProperty(PropertyName = "key")]
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>
        /// The service identifier.
        /// </value>
        [JsonProperty(PropertyName = "serviceIdentifier")]
        public string ServiceIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the server identifier.
        /// </summary>
        /// <value>
        /// The server identifier.
        /// </value>
        [JsonProperty(PropertyName = "serverIdentifier")]
        public string ServerIdentifier { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventCriteria" /> class.
        /// </summary>
        public BaseLogCriteria(BaseLogCriteria criteria)
        {
            if (criteria != null)
            {
                FromStamp = criteria.FromStamp;
                ToStamp = criteria.ToStamp;
                Count = criteria.Count;
                Key = criteria.Key;
                ServiceIdentifier = criteria.ServiceIdentifier;
                ServerIdentifier = criteria.ServerIdentifier;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventCriteria"/> class.
        /// </summary>
        public BaseLogCriteria()
            : this(null)
        {
        }
    }
}