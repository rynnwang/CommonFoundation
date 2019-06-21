using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public class HttpCredential : ICommonCredential
    {
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        [JsonProperty("domain")]
        public string Domain { get; set; }
    }
}