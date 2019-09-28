using Newtonsoft.Json;

namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    public class ICPOption
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the root domain. e.g.: beyova.com, google.com
        /// </summary>
        /// <value>
        /// The root domain.
        /// </value>
        [JsonProperty("rootDomain")]
        public string RootDomain { get; set; }

        /// <summary>
        /// Gets or sets the gong an code.
        /// </summary>
        /// <value>
        /// The gong an code.
        /// </value>
        [JsonProperty("gongAnCode")]
        public string GongAnCode { get; set; }
    }
}