using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class JsonFileConfigurationManifest.
    /// </summary>
    public class JsonFileConfigurationManifest
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the configurations.
        /// </summary>
        /// <value>The configurations.</value>
        public JToken Configurations { get; set; }
    }
}