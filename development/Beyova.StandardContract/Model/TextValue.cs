using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class TextValue.
    /// </summary>
    public class TextValue
    {
        /// <summary>
        /// Gets or sets the raw text.
        /// </summary>
        /// <value>The raw text.</value>
        [JsonProperty("rawText")]
        public string RawText { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        [JsonProperty("contentType")]
        public string ContentType { get; set; }
    }
}