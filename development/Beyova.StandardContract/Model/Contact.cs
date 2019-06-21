using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class Contact : IContact
    {
        /// <summary>
        /// Gets or sets the name of the contact.
        /// </summary>
        /// <value>
        /// The name of the contact.
        /// </value>
        [JsonProperty("contactName")]
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the call number.
        /// </summary>
        /// <value>
        /// The call number.
        /// </value>
        [JsonProperty("callNumber")]
        public string CallNumber { get; set; }
    }
}