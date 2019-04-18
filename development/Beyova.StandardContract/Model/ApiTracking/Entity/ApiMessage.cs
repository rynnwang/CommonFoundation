using System;
using Newtonsoft.Json;

namespace Beyova.Diagnostic
{
    /// <summary>
    /// Class ApiMessage.
    /// </summary>
    public class ApiMessage : IServiceIdentifiable, IApiMessage
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [JsonProperty(PropertyName = "key")]
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>
        /// The created stamp.
        /// </value>
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
        /// Initializes a new instance of the <see cref="ApiMessage"/> class.
        /// </summary>
        public ApiMessage()
        {
        }
    }
}