namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class RestApiAuthenticationHandler. This class is to save sensitive event handlers for <see cref="ApiHandlerBase"/> or <see cref="RestApiRouter"/>. Such as initialize thread user info by token.
    /// </summary>
    public abstract class RestApiAuthenticationHandler
    {
        /// <summary>
        /// Gets the credential by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="realm">The realm.</param>
        /// <returns>
        /// ICredential.
        /// </returns>
        public abstract ICredential GetCredentialByToken(string token, string realm);
    }
}