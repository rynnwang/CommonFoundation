using Beyova.Api;

namespace Beyova.Cache
{
    /// <summary>
    /// Interface IApiCacheContainer
    /// </summary>
    public interface IApiCacheContainer : ICacheContainer<ApiRouteIdentifier, string>
    {
        /// <summary>
        /// Gets the cache result.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        bool GetCacheResult(ApiRouteIdentifier key, out string result);
    }
}