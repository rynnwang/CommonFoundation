namespace Beyova
{
    /// <summary>
    /// Class RsaKeys.
    /// </summary>
    public class RsaKeys : IRsaKeys
    {
        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        /// <value>The private key.</value>
        public CryptoKey PrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>The public key.</value>
        public CryptoKey PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the size of the double word key.
        /// </summary>
        /// <value>
        /// The size of the double word key.
        /// </value>
        public int DoubleWordKeySize { get; set; }

        /// <summary>
        /// Creates the specified dw key size.
        /// </summary>
        /// <param name="dwKeySize">Size of the dw key.</param>
        /// <returns></returns>
        public static RsaKeys Create(int dwKeySize = DefaultSecuritySettings.DoubleWordKeySize)
        {
            return EncodingOrSecurityExtension.CreateRsaKeys(dwKeySize);
        }
    }
}