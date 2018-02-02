namespace Beyova.Cache
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IRedisKeyGenerator<TKey>
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string GetKey(TKey key);
    }
}