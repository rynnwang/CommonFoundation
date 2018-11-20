using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class DataSecurityAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the data security provider.
        /// </summary>
        /// <value>
        /// The data security provider.
        /// </value>
        public IDataSecurityProvider DataSecurityProvider { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSecurityAttribute"/> class.
        /// </summary>
        /// <param name="loader">The loader.</param>
        public DataSecurityAttribute(IDataSecurityOptionLoader loader)
        {
            this.DataSecurityProvider = loader?.GetProvider();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSecurityAttribute"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public DataSecurityAttribute(IDataSecurityProvider provider)
        {
            this.DataSecurityProvider = provider;
        }
    }
}