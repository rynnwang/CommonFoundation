using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class LocalConfigurationRawItem. It maps to raw configuration values from file.
    /// </summary>
    public class LocalConfigurationRawItem : ConfigurationRawItem
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ConfigurationRawItem"/> is encrypted.
        /// </summary>
        /// <value><c>null</c> if [encrypted] contains no value, <c>true</c> if [encrypted]; otherwise, <c>false</c>.</value>
        public bool? Encrypted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalConfigurationRawItem"/> class.
        /// </summary>
        public LocalConfigurationRawItem() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalConfigurationRawItem"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        internal LocalConfigurationRawItem(ConfigurationRawItem item)
            : base(item)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRawItem"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public LocalConfigurationRawItem(ConfigurationItemBase item)
            : base(item)
        {
        }
    }
}