namespace Beyova
{
    /// <summary>
    /// Interface IConfigurationLoader
    /// </summary>
    public interface IConfigurationLoader
    {
        /// <summary>
        /// Gets the reader.
        /// </summary>
        /// <param name="sourceAssembly">The source assembly.</param>
        /// <param name="coreComponentVersion">The core component version.</param>
        /// <returns></returns>
        IConfigurationReader GetReader(string sourceAssembly, string coreComponentVersion);
    }
}