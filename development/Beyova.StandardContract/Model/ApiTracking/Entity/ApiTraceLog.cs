using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beyova.Diagnostic
{
    /// <summary>
    /// Class ApiTraceLog.
    /// </summary>
    public class ApiTraceLog : ApiTraceStep, ICreatedStamp, IServiceIdentifiable
    {
        /// <summary>
        /// Gets or sets the trace identifier.
        /// </summary>
        /// <value>The trace identifier.</value>
        [JsonProperty(PropertyName = "traceId")]
        public string TraceId { get; set; }

        /// <summary>
        /// Gets or sets the trace sequence.
        /// </summary>
        /// <value>The trace sequence.</value>
        [JsonProperty(PropertyName = "traceSequence")]
        public int? TraceSequence { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        [JsonProperty(PropertyName = "createdStamp")]
        public DateTime? CreatedStamp { get; set; }

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
        /// Gets or sets the steps.
        /// </summary>
        /// <value>
        /// The steps.
        /// </value>
        [JsonProperty(PropertyName = "steps")]
        public List<ApiTraceStep> Steps { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTraceLog" /> class.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="serviceName">Name of the service.</param>
        public ApiTraceLog(string serverName, string serviceName)
            : this()
        {
            ServerIdentifier = serverName.SafeToString(EnvironmentCore.MachineName);
            ServiceIdentifier = serviceName.SafeToString(EnvironmentCore.ProductName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTraceLog" /> class.
        /// </summary>
        public ApiTraceLog()
        {
            CreatedStamp = DateTime.UtcNow;
            Steps = new List<ApiTraceStep>();
        }
    }
}