using Newtonsoft.Json;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ApiLogBase.
    /// </summary>
    public class ApiLogBase
    {
        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>The service identifier.</value>
        [JsonProperty(PropertyName = "serviceIdentifier")] public string ServiceIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the server identifier.
        /// <remarks>Commonly, it is server name.</remarks>
        /// </summary>
        /// <value>The server identifier.</value>
        [JsonProperty(PropertyName = "serverIdentifier")] public string ServerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the server host.
        /// </summary>
        /// <value>
        /// The server host.
        /// </value>
        [JsonProperty(PropertyName = "serverHost")] public string ServerHost { get; set; }

        /// <summary>
        /// Gets or sets the raw URL.
        /// </summary>
        /// <value>
        /// The raw URL.
        /// </value>
        [JsonProperty(PropertyName = "rawUrl")] public string RawUrl { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiLogBase" /> class.
        /// </summary>
        /// <param name="logBase">The log base.</param>
        public ApiLogBase(ApiLogBase logBase)
        {
            if (logBase != null)
            {
                this.ServerIdentifier = logBase.ServerIdentifier;
                this.ServiceIdentifier = logBase.ServiceIdentifier;
                this.ServerHost = logBase.ServerHost;
                this.RawUrl = logBase.RawUrl;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiLogBase"/> class.
        /// </summary>
        public ApiLogBase()
        {
        }
    }
}