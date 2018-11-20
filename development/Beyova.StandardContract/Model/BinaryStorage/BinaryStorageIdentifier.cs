namespace Beyova
{
    /// <summary>
    /// Class BinaryStorageIdentifier.
    /// </summary>
    public class BinaryStorageIdentifier : ContainerableIdentifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageIdentifier"/> class.
        /// </summary>
        public BinaryStorageIdentifier() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageIdentifier"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public BinaryStorageIdentifier(BinaryStorageIdentifier identifier)
        {
            if (identifier != null)
            {
                Container = identifier.Container;
                Identifier = identifier.Identifier;
            }
        }

        /// <summary>
        /// To the cloud resource URI.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>CloudResourceUri.</returns>
        public CloudResourceUri ToCloudResourceUri(string type)
        {
            return new CloudResourceUri
            {
                Type = type.SafeToString("default"),
                Container = this.Container,
                Identifier = this.Identifier
            };
        }
    }
}