using Newtonsoft.Json;

namespace Beyova.Diagnostic
{
    /// <summary>
    /// Class ApiMessageCriteria.
    /// </summary>
    public class ApiMessageCriteria : BaseLogCriteria
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
        /// Initializes a new instance of the <see cref="ApiMessageCriteria"/> class.
        /// </summary>
        public ApiMessageCriteria()
        {
        }
    }
}