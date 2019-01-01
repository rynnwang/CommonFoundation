using System;

namespace Beyova
{
    /// <summary>
    /// Class BeyovaConfigurationAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    [Obsolete("Use BeyovaConfigurationLoaderAttribute instead. [assembly: BeyovaConfigurationLoader({configurationName})]")]
    public class BeyovaConfigurationAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the underlying object.
        /// </summary>
        /// <value>
        /// The underlying object.
        /// </value>
        public BeyovaLocalConfigurationOptions Options { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaConfigurationAttribute" /> class.
        /// If configurationDirectory is not specified, then use <c>{BaseDirectory}/Configurations/{configurationName}</c>
        /// </summary>
        /// <param name="configurationName">Name of the configuration. e.g.: <c>XXX.YYY.*.json</c></param>
        /// <param name="configurationDirectory">The configuration directory.</param>
        public BeyovaConfigurationAttribute(string configurationName, string configurationDirectory = null) : this(new BeyovaLocalConfigurationOptions(configurationName, configurationDirectory))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaConfigurationAttribute"/> class.
        /// </summary>
        /// <param name="configurationOption">The configuration option.</param>
        protected BeyovaConfigurationAttribute(BeyovaLocalConfigurationOptions configurationOption)
        {
            Options = configurationOption;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Options?.GetConfigurationFullPath();
        }
    }
}