namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOAuth2ProviderOptions
    {
        /// <summary>
        /// Gets or sets the authentication URI.
        /// </summary>
        /// <value>
        /// The authentication URI.
        /// </value>
        string AuthenticationUri { get; set; }

        /// <summary>
        /// Gets or sets the access token URI.
        /// </summary>
        /// <value>
        /// The access token URI.
        /// </value>
        string AccessTokenUri { get; set; }

        /// <summary>
        /// Gets or sets the authentication HTTP method.
        /// </summary>
        /// <value>
        /// The authentication HTTP method.
        /// </value>
        string AuthenticationHttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the user profile URI.
        /// </summary>
        /// <value>
        /// The user profile URI.
        /// </value>
        string UserProfileUri { get; set; }

        /// <summary>
        /// Gets or sets the type of the authentication by code response.
        /// </summary>
        /// <value>
        /// The type of the authentication by code response.
        /// </value>
        string AuthenticationByCodeResponseType { get; set; }
    }
}