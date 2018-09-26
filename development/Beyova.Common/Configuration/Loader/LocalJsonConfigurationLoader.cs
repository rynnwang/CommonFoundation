using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// It would load configuration by {bin folder/base folder}/Configurations/{configurationName}
    /// </summary>
    public class LocalJsonConfigurationLoader : IConfigurationLoader
    {
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public BeyovaLocalConfigurationOptions Options { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalJsonConfigurationLoader"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public LocalJsonConfigurationLoader(BeyovaLocalConfigurationOptions options)
        {
            Options = options;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalJsonConfigurationLoader" /> class.
        /// </summary>
        /// <param name="configurationName">Name of the configuration.</param>
        public LocalJsonConfigurationLoader(string configurationName)
        {
            Options = string.IsNullOrWhiteSpace(configurationName) ? null : new BeyovaLocalConfigurationOptions(configurationName);
        }

        /// <summary>
        /// Gets the reader.
        /// </summary>
        /// <param name="sourceAssembly">The source assembly.</param>
        /// <param name="coreComponentVersion">The core component version.</param>
        /// <returns></returns>
        public IConfigurationReader GetReader(string sourceAssembly, string coreComponentVersion)
        {
            return this.Options == null ? null : new JsonConfigurationReader(sourceAssembly, coreComponentVersion, nameof(JsonConfigurationReader), this.Options, Framework.DataSecurityProvider);
        }
    }
}
