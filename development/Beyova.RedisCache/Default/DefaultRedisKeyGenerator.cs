namespace Beyova.Cache
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class DefaultRedisKeyGenerator<TKey> : IRedisKeyGenerator<TKey>
    {
        /// <summary>
        /// Gets or sets the entitiy identifier.
        /// </summary>
        /// <value>
        /// The entitiy identifier.
        /// </value>
        public string EntitiyIdentifier { get; protected set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetKey(TKey key)
        {
            return string.Format("{0}_{1}", EntitiyIdentifier, key);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRedisKeyGenerator{TKey}"/> class.
        /// </summary>
        /// <param name="entitiyIdentifier">The entitiy identifier.</param>
        public DefaultRedisKeyGenerator(string entitiyIdentifier)
        {
            entitiyIdentifier.CheckEmptyString(nameof(entitiyIdentifier));
            EntitiyIdentifier = entitiyIdentifier;
        }
    }
}