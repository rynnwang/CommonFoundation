using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public class OAuth2ClientOptions : OAuth2ClientOptions<OAuth2ProviderOptions>
    {
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class OAuth2ClientOptions<TProviderOptions>
        where TProviderOptions : OAuth2ProviderOptions, new()
    {
        /// <summary>
        /// Gets or sets the provider options.
        /// </summary>
        /// <value>
        /// The provider options.
        /// </value>
        [JsonProperty(PropertyName = "providerOptions")]
        public TProviderOptions ProviderOptions { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        [JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        [JsonProperty(PropertyName = "clientSecret")]
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the redirect URI. This redirect URI is to send to OAUTH server, to tell it where to redirect back when user grant access.
        /// </summary>
        /// <value>
        /// The redirect URI.
        /// </value>
        [JsonProperty(PropertyName = "redirectUri")]
        public string RedirectUri { get; set; }
    }
}