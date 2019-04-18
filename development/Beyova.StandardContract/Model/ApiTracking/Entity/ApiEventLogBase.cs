using System;
using Newtonsoft.Json;

namespace Beyova.Diagnostic
{
    /// <summary>
    /// Class ApiEventLogBase.
    /// </summary>
    public class ApiEventLogBase : GlobalApiUniqueIdentifier, IApiEventLogBase
    {
        /// <summary>
        /// Gets or sets the exception key.
        /// </summary>
        /// <value>The exception key.</value>
        [JsonProperty(PropertyName = "exceptionKey")]
        public Guid? ExceptionKey { get; set; }

        /// <summary>
        /// Gets or sets the culture code.
        /// </summary>
        /// <value>The culture code.</value>
        [JsonProperty(PropertyName = "cultureCode")]
        public string CultureCode { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// Commonly, it can be device id, PC name, etc.
        /// </summary>
        /// <value>The client identifier.</value>
        [JsonProperty(PropertyName = "clientIdentifier")]
        public string ClientIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        [JsonProperty(PropertyName = "ipAddress")]
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the trace identifier.
        /// </summary>
        /// <value>The trace identifier.</value>
        [JsonProperty(PropertyName = "traceId")]
        public string TraceId { get; set; }

        /// <summary>
        /// Gets or sets the operator credential.
        /// </summary>
        /// <value>The operator credential.</value>
        [JsonProperty(PropertyName = "operatorCredential")]
        public BaseCredential OperatorCredential { get; set; }

        /// <summary>
        /// Gets or sets the raw URL.
        /// </summary>
        /// <value>
        /// The raw URL.
        /// </value>
        [JsonProperty(PropertyName = "rawUrl")]
        public string RawUrl { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventLogBase"/> class.
        /// </summary>
        public ApiEventLogBase()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventLogBase"/> class.
        /// </summary>
        /// <param name="eventLogBase">The event log base.</param>
        public ApiEventLogBase(ApiEventLogBase eventLogBase)
            : base(eventLogBase)
        {
            if (eventLogBase != null)
            {
                ExceptionKey = eventLogBase.ExceptionKey;
                CultureCode = eventLogBase.CultureCode;
                ClientIdentifier = eventLogBase.ClientIdentifier;
                IpAddress = eventLogBase.IpAddress;
                ExceptionKey = eventLogBase.ExceptionKey;
                OperatorCredential = eventLogBase.OperatorCredential;
                 RawUrl = eventLogBase.RawUrl;
            }
        }
    }
}