using System;
using Newtonsoft.Json;

namespace Beyova.Diagnostic
{
    /// <summary>
    /// Class ApiEventLog.
    /// </summary>
    public class ApiEventLog : ApiEventLogBase, IApiEventLog
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [JsonProperty(PropertyName = "key")]
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the entry stamp.
        /// </summary>
        /// <value>The entry stamp.</value>
        [JsonProperty(PropertyName = "entryStamp")]
        public DateTime? EntryStamp { get; set; }

        /// <summary>
        /// Gets or sets the exit stamp.
        /// </summary>
        /// <value>The exit stamp.</value>
        [JsonProperty(PropertyName = "exitStamp")]
        public DateTime? ExitStamp { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        [JsonProperty(PropertyName = "createdStamp")]
        public DateTime? CreatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        [JsonProperty(PropertyName = "contentLength")]
        public long? ContentLength { get; set; }

        /// <summary>
        /// Gets or sets the geo information.
        /// </summary>
        /// <value>The geo information.</value>
        [JsonProperty(PropertyName = "geoInfo")]
        public GeoInfoBase GeoInfo { get; set; }

        /// <summary>
        /// Gets the duration. Unit: TotalMilliseconds
        /// </summary>
        /// <value>The duration.</value>
        [JsonProperty(PropertyName = "duration")]
        public double? Duration
        {
            get
            {
                return (ExitStamp != null && EntryStamp != null) ? (ExitStamp.Value - EntryStamp.Value).TotalMilliseconds : null as double?;
            }
            set
            {
                //Do nothing.
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventLog"/> class.
        /// </summary>
        public ApiEventLog(ApiEventLogBase eventLogBase)
            : base(eventLogBase)
        {
            Key = Guid.NewGuid();
            CreatedStamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventLog"/> class.
        /// </summary>
        public ApiEventLog()
            : this(null)
        {
        }
    }
}