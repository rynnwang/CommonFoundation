namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public class SqlConfigurationLoader : IConfigurationLoader
    {
        /// <summary>
        /// Gets or sets the configuration key.
        /// </summary>
        /// <value>
        /// The configuration key.
        /// </value>
        public string ConfigurationKey { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConfigurationLoader" /> class.
        /// </summary>
        /// <param name="configurationKey">The configuration key.</param>
        public SqlConfigurationLoader(string configurationKey)
        {
            // Have to set configuration key here, to move get value by configuration key in GetReader.
            // Because when in constructor, none of configuration readers is registered yet.
            this.ConfigurationKey = configurationKey;
        }

        /// <summary>
        /// Gets the reader.
        /// </summary>
        /// <param name="sourceAssembly">The source assembly.</param>
        /// <param name="coreComponentVersion">The core component version.</param>
        /// <returns></returns>
        public IConfigurationReader GetReader(string sourceAssembly, string coreComponentVersion)
        {
            // Have to use ConfigurationHub.GetConfiguration, not Framework.GetConfiguration.
            // Because when go through this code part, Framework is still in constructor scope.
            var sqlConnection = string.IsNullOrWhiteSpace(this.ConfigurationKey) ? null : ConfigurationHub.GetConfiguration<string>(this.ConfigurationKey);
            return string.IsNullOrWhiteSpace(sqlConnection) ? null : new SqlConfigurationReader(sourceAssembly, coreComponentVersion, sqlConnection);
        }
    }
}