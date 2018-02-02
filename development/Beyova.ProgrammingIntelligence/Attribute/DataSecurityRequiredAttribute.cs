using System;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class EncryptionRequiredAttribute. Field or property which own this field
    /// NOTE: It is auto effected only when using <see cref="SqlDataAccessController"/> and <see cref="IDataSecurityProvider"/> instance is specified, and of course CodeSmith.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DataSecurityRequiredAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the encryption provider.
        /// </summary>
        /// <value>
        /// The encryption provider.
        /// </value>
        public IDataSecurityProvider EncryptionProvider { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSecurityRequiredAttribute"/> class.
        /// </summary>
        /// <param name="encryptionProvider">The encryption provider.</param>
        public DataSecurityRequiredAttribute(IDataSecurityProvider encryptionProvider = null)
        {
            EncryptionProvider = encryptionProvider;
        }
    }
}