using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public sealed class AssemblyDataSecurityOptionsAttribute : Attribute
    {
        /// <summary>
        /// Gets the RSA keys.
        /// </summary>
        /// <value>
        /// The RSA keys.
        /// </value>
        internal IRsaKeys RsaKeys { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDataSecurityOptionsAttribute"/> class.
        /// </summary>
        public AssemblyDataSecurityOptionsAttribute(IRsaKeys rsaKeys)
        {
            rsaKeys.CheckNullObject(nameof(rsaKeys));
            rsaKeys.PrivateKey.CheckNullOrEmpty(nameof(rsaKeys.PrivateKey));
            rsaKeys.PublicKey.CheckNullOrEmpty(nameof(rsaKeys.PublicKey));

            RsaKeys = rsaKeys;
        }
    }
}