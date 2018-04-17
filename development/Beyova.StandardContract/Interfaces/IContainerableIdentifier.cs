namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContainerableIdentifier : IContainerableIdentifier<string, string>
    {
    }

    /// <summary>
    /// Interface IContainerableIdentifier
    /// </summary>
    public interface IContainerableIdentifier<TContainer, TIdentifier>
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        TContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        TIdentifier Identifier { get; set; }
    }
}