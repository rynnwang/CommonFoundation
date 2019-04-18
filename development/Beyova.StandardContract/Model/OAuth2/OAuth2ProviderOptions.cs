using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public class OAuth2ProviderOptions : IOAuth2ProviderOptions
    {
        /// <summary>
        /// Gets or sets the authentication URI.
        /// </summary>
        /// <value>
        /// The authentication URI.
        /// </value>
        [JsonProperty(PropertyName = "authenticationUri")]
        public string AuthenticationUri { get; set; }

        /// <summary>
        /// Gets or sets the access token URI.
        /// </summary>
        /// <value>
        /// The access token URI.
        /// </value>
        [JsonProperty(PropertyName = "accessTokenUri")]
        public string AccessTokenUri { get; set; }

        /// <summary>
        /// Gets or sets the authentication HTTP method.
        /// </summary>
        /// <value>
        /// The authentication HTTP method.
        /// </value>
        [JsonProperty(PropertyName = "authenticationHttpMethod")]
        public string AuthenticationHttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the user profile URI.
        /// </summary>
        /// <value>
        /// The user profile URI.
        /// </value>
        [JsonProperty(PropertyName = "userProfileUri")]
        public string UserProfileUri { get; set; }

        /// <summary>
        /// Gets or sets the type of the authentication by code response.
        /// </summary>
        /// <value>
        /// The type of the authentication by code response.
        /// </value>
        [JsonProperty(PropertyName = "authenticationByCodeResponseType")]
        public string AuthenticationByCodeResponseType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2ProviderOptions"/> class.
        /// </summary>
        public OAuth2ProviderOptions() : this(null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2ProviderOptions"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public OAuth2ProviderOptions(IOAuth2ProviderOptions options)
        {
            if (options != null)
            {
                AuthenticationUri = options.AuthenticationUri;
                AccessTokenUri = options.AccessTokenUri;
                AuthenticationHttpMethod = options.AuthenticationHttpMethod;
                UserProfileUri = options.UserProfileUri;
                AuthenticationByCodeResponseType = options.AuthenticationByCodeResponseType;
            }
        }
    }
}