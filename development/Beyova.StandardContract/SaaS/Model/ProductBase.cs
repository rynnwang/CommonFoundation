using System;

namespace Beyova.SaasPlatform
{
    /// <summary>
    /// Class ProductBase.
    /// </summary>
    public abstract class ProductBase : SimpleBaseObject, IRsaKeys, IExpirable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>The public key.</value>
        public CryptoKey PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        /// <value>The private key.</value>
        public CryptoKey PrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the expired stamp.
        /// </summary>
        /// <value>The expired stamp.</value>
        public DateTime? ExpiredStamp { get; set; }

        /// <summary>
        /// Gets or sets the size of the double word key.
        /// </summary>
        /// <value>
        /// The size of the double word key.
        /// </value>
        public int DoubleWordKeySize { get; set; }
    }
}