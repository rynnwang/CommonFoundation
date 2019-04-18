namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOAuth2ClientOptions : IOAuth2ClientOptions<OAuth2ProviderOptions>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IOAuth2ClientOptions<TProviderOptions>
        where TProviderOptions : IOAuth2ProviderOptions, new()
    {
        /// <summary>
        /// Gets or sets the provider options.
        /// </summary>
        /// <value>
        /// The provider options.
        /// </value>
        OAuth2ProviderOptions ProviderOptions { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the redirect URI.
        /// </summary>
        /// <value>
        /// The redirect URI.
        /// </value>
        string RedirectUri { get; set; }
    }
}