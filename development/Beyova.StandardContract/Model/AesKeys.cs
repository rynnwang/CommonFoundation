namespace Beyova
{
    /// <summary>
    /// Class AesKeys.
    /// </summary>
    public class AesKeys : IAesKeys
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public CryptoKey Key { get; set; }

        /// <summary>
        /// Gets or sets the initialization vector.
        /// </summary>
        /// <value>
        /// The initialization vector.
        /// </value>
        public CryptoKey InitializationVector { get; set; }

        /// <summary>
        /// Gets or sets the size of the key.
        /// </summary>
        /// <value>
        /// The size of the key.
        /// </value>
        public int KeySize { get; set; }

        /// <summary>
        /// Creates the specified key size.
        /// </summary>
        /// <param name="keySize">Size of the key.</param>
        /// <returns></returns>
        public static AesKeys Create(int keySize = DefaultSecuritySettings.DoubleWordKeySize)
        {
            return EncodingOrSecurityExtension.CreateAesKeys(keySize);
        }
    }
}