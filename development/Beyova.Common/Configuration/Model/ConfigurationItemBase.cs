using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ConfigurationItemBase
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the minimum component version required.
        /// </summary>
        /// <value>The minimum component version required.</value>
        public string MinComponentVersionRequired { get; set; }

        /// <summary>
        /// Gets or sets the maximum component version limited.
        /// </summary>
        /// <value>The maximum component version limited.</value>
        public string MaxComponentVersionLimited { get; set; }
    }

    /// <summary>
    /// Class ConfigurationItemBase
    /// </summary>
    public abstract class ConfigurationItemBase<T> : ConfigurationItemBase
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationItemBase{T}"/> class.
        /// </summary>
        public ConfigurationItemBase() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationItemBase{T}"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        protected ConfigurationItemBase(ConfigurationItemBase item)
        {
            if (item != null)
            {
                this.Key = item.Key;
                this.Type = item.Type;
                this.MinComponentVersionRequired = item.MinComponentVersionRequired;
                this.MaxComponentVersionLimited = item.MaxComponentVersionLimited;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationItemBase{T}" /> class.
        /// </summary>
        /// <param name="item">The item.</param>
        protected ConfigurationItemBase(ConfigurationItemBase<T> item) : this(item as ConfigurationItemBase)
        {
            if (item != null)
            {
                this.Value = item.Value;
            }
        }
    }
}