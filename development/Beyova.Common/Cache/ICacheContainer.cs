namespace Beyova.Cache
{
    /// <summary>
    /// Interface ICacheContainer. It defines all required interfaces for all kinds of cache. (Including memory and distributed)
    /// </summary>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public interface ICacheContainer<TKey, TEntity> : ICacheContainer
    {
        /// <summary>
        /// Gets the automatic retrieval options.
        /// </summary>
        /// <value>
        /// The automatic retrieval options.
        /// </value>
        CacheAutoRetrievalOptions<TKey, TEntity> AutoRetrievalOptions { get; }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>TEntity.</returns>
        TEntity Get(TKey key);

        /// <summary>
        /// Updates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        void Update(TKey key, TEntity entity);
    }

    /// <summary>
    /// Interface ICacheContainer
    /// </summary>
    public interface ICacheContainer : ICacheParameter
    {
        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
    }
}