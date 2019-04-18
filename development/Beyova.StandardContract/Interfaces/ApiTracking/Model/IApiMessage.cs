namespace Beyova.Diagnostic
{
    /// <summary>
    /// Interface IApiMessage.
    /// </summary>
    public interface IApiMessage : IServiceIdentifiable, IIdentifier, ICreatedStamp
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        string Message { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        string Category { get; set; }
    }
}