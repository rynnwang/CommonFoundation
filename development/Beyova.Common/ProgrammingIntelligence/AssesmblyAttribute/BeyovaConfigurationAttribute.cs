using System;
using System.IO;

namespace Beyova
{
    /// <summary>
    /// Class BeyovaConfigurationAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public class BeyovaConfigurationAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the underlying object.
        /// </summary>
        /// <value>
        /// The underlying object.
        /// </value>
        public BeyovaConfigurationInfo UnderlyingObject { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaConfigurationAttribute" /> class.
        /// If configurationDirectory is not specified, then use <c>{BaseDirectory}/Configurations/{configurationName}</c>
        /// </summary>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <param name="configurationDirectory">The configuration directory.</param>
        public BeyovaConfigurationAttribute(string configurationName, string configurationDirectory = null)
        {
            UnderlyingObject = new BeyovaConfigurationInfo(configurationName, configurationDirectory);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return UnderlyingObject?.GetConfigurationFullPath();
        }
    }
}