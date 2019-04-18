using Newtonsoft.Json;

namespace Beyova.Diagnostic
{
    /// <summary>
    ///
    /// </summary>
    public class ApiUniqueIdentifier
    {
        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        [JsonProperty(PropertyName = "httpMethod")]
        public string HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}", HttpMethod, Path);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiUniqueIdentifier"/> class.
        /// </summary>
        public ApiUniqueIdentifier() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiUniqueIdentifier"/> class.
        /// </summary>
        /// <param name="uniqueIdentifier">The unique identifier.</param>
        public ApiUniqueIdentifier(ApiUniqueIdentifier uniqueIdentifier)
        {
            if (uniqueIdentifier != null)
            {
                Path = uniqueIdentifier.Path;
                HttpMethod = uniqueIdentifier.HttpMethod;
            }
        }
    }
}