using System;

namespace Beyova
{
    /// <summary>
    /// Class BeyovaComponentAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public class BeyovaConfigurationLoaderAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the loader.
        /// </summary>
        /// <value>
        /// The loader.
        /// </value>
        public IConfigurationLoader Loader { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaConfigurationLoaderAttribute" /> class.
        /// </summary>
        /// <param name="loaderType">Type of the loader. Type need to has non-parameter constructor and inherited from <see cref="IConfigurationLoader"/></param>
        /// <param name="loaderParameter">The loader parameter.</param>
        public BeyovaConfigurationLoaderAttribute(Type loaderType, string loaderParameter)
        {
            if (loaderType != null && typeof(IConfigurationLoader).IsAssignableFrom(loaderType))
            {
                this.Loader = (loaderType.TryCreateInstanceViaParameterlessConstructor() as IConfigurationLoader) ?? (loaderType.TryCreateInstanceViaSingleParameterConstructor(loaderParameter) as IConfigurationLoader);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaConfigurationLoaderAttribute" /> class. This is a shortcut way to add <see cref="LocalJsonConfigurationLoader" />, to instead <see cref="BeyovaConfigurationAttribute" />
        /// </summary>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <param name="configurationDirectory">The configuration directory.</param>
        public BeyovaConfigurationLoaderAttribute(string configurationName, string configurationDirectory = null)
        {
            if (!string.IsNullOrWhiteSpace(configurationName))
            {
                Loader = new LocalJsonConfigurationLoader(new BeyovaLocalConfigurationOptions(configurationName, configurationDirectory));
            }
        }
    }
}