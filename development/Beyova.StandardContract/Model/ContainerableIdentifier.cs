namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class ContainerableIdentifier : ContainerableIdentifier<string, string>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TIndentifier">The type of the indentifier.</typeparam>
    public abstract class ContainerableIdentifier<TContainer, TIndentifier> : IContainerableIdentifier<TContainer, TIndentifier>
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        public TContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public TIndentifier Identifier { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}/{1}", Container, Identifier);
        }
    }
}