namespace Beyova
{
    /// <summary>
    /// Class ConfigurationRawItem. It maps to raw configuration values from file/db/...
    /// </summary>
    public class ConfigurationRawItem : ConfigurationItemBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRawItem"/> class.
        /// </summary>
        public ConfigurationRawItem() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRawItem"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        internal ConfigurationRawItem(ConfigurationRawItem item)
            : base(item)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRawItem"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public ConfigurationRawItem(ConfigurationItemBase item)
            : base(item)
        {
        }
    }
}