namespace Beyova
{
    /// <summary>
    /// Class BinaryStorageIdentifier.
    /// </summary>
    public class BinaryStorageIdentifier : ContainerableIdentifier
    {
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