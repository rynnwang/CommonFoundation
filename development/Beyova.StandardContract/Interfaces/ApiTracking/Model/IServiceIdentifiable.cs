namespace Beyova.Diagnostic
{
    /// <summary>
    /// Interface IServiceIdentifiable.
    /// </summary>
    public interface IServiceIdentifiable
    {
        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>The service identifier.</value>
        string ServiceIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the server identifier.
        /// <remarks>Commonly, it is server name or ip address.</remarks>
        /// </summary>
        /// <value>The server identifier.</value>
        string ServerIdentifier { get; set; }
    }
}