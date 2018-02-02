namespace Beyova
{
    /// <summary>
    /// Interface IRsaKeys
    /// </summary>
    public interface IRsaKeys
    {
        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>The public key.</value>
        CryptoKey PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        /// <value>The private key.</value>
        CryptoKey PrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the size of the double word key.
        /// </summary>
        /// <value>
        /// The size of the double word key.
        /// </value>
        int DoubleWordKeySize { get; set; }
    }
}