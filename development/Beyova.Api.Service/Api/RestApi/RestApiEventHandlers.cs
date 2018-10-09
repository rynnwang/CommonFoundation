namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class RestApiEventHandlers. This class is to save sensitive event handlers for <see cref="ApiHandlerBase"/> or <see cref="RestApiRouter"/>. Such as initialize thread user info by token.
    /// </summary>
    [System.Obsolete("Use IRestApiEventHandlers")]
    public abstract class RestApiEventHandlers : IRestApiEventHandlers
    {
        /// <summary>
        /// Gets the credential by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>ICredential.</returns>
        public abstract ICredential GetCredentialByToken(string token);
    }
}