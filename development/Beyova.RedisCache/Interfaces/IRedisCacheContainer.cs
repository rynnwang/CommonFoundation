namespace Beyova.Cache
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRedisCacheContainer<TKey, TEntity> : ICacheContainer<TKey, TEntity>, IRedisCacheContainer
    {
    }

    /// <summary>
    ///
    /// </summary>
    public interface IRedisCacheContainer : ICacheContainer
    {
        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        StackExchange.Redis.IDatabase Database { get; }
    }
}