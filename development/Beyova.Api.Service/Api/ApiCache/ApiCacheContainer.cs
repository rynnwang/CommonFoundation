using Beyova.Api;

namespace Beyova.Cache
{
    /// <summary>
    /// Class ApiCacheContainer.
    /// </summary>
    public class ApiCacheContainer : MemoryCacheContainer<ApiRouteIdentifier, string>, IApiCacheContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiCacheContainer" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="cacheParameter">The cache parameter.</param>
        internal ApiCacheContainer(string name, ApiCacheParameter cacheParameter)
            : base(new MemoryCacheContainerOptions<ApiRouteIdentifier> { Name = name, Capacity = cacheParameter.Capacity, ExpirationInSecond = cacheParameter.ExpirationInSecond })
        {
        }

        /// <summary>
        /// Gets the cache result.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public bool GetCacheResult(ApiRouteIdentifier key, out string result)
        {
            return InternalTryGetValidEntityFromCache(key, out result);
        }
    }
}