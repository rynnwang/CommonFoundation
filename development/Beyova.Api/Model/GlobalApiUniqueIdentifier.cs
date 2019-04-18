using Newtonsoft.Json;

namespace Beyova.Diagnostic
{
    /// <summary>
    ///
    /// </summary>
    public class GlobalApiUniqueIdentifier : ApiUniqueIdentifier, IServiceIdentifiable
    {
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
        /// Initializes a new instance of the <see cref="GlobalApiUniqueIdentifier" /> class.
        /// </summary>
        /// <param name="uniqueIdentifier">The log base.</param>
        public GlobalApiUniqueIdentifier(GlobalApiUniqueIdentifier uniqueIdentifier) : base(uniqueIdentifier)
        {
            if (uniqueIdentifier != null)
            {
                ServerIdentifier = uniqueIdentifier.ServerIdentifier;
                ServiceIdentifier = uniqueIdentifier.ServiceIdentifier;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalApiUniqueIdentifier"/> class.
        /// </summary>
        public GlobalApiUniqueIdentifier() : this(null)
        {
        }
    }
}