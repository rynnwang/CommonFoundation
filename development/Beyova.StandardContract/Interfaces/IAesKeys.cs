namespace Beyova
{
    /// <summary>
    /// Interface IAesKeys
    /// </summary>
    public interface IAesKeys
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        CryptoKey Key { get; set; }

        /// <summary>
        /// Gets or sets the initialization vector.
        /// </summary>
        /// <value>
        /// The initialization vector.
        /// </value>
        CryptoKey InitializationVector { get; set; }

        /// <summary>
        /// Gets or sets the size of the key.
        /// </summary>
        /// <value>
        /// The size of the key.
        /// </value>
        int KeySize { get; set; }
    }
}