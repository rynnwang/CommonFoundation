namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Interface IRestApiEventHandlers. This is to save sensitive event handlers for <see cref="ApiHandlerBase"/> or <see cref="RestApiRouter"/>. Such as initialize thread user info by token.
    /// </summary>
    public interface IRestApiEventHandlers
    {
        /// <summary>
        /// Gets the credential by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>ICredential.</returns>
        ICredential GetCredentialByToken(string token);
    }
}