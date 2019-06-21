using System;
using System.Collections.Generic;

namespace Beyova.Cache
{
    /// <summary>
    /// Class CacheRealm.
    /// </summary>
    public static class CacheRealm
    {
        /// <summary>
        /// The containers
        /// </summary>
        private static MatrixList<Type, ICacheContainer> containers = new MatrixList<Type, ICacheContainer>();

        /// <summary>
        /// Registers the cache container.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="cacheContainer">The cache container.</param>
        internal static void RegisterCacheContainer<TKey, TEntity>(MemoryCacheContainer<TKey, TEntity> cacheContainer)
        {
            if (cacheContainer != null)
            {
                containers.Add(typeof(TEntity), cacheContainer);
            }
        }

        /// <summary>
        /// Registers the cache container.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="cacheContainer">The cache container.</param>
        internal static void RegisterCacheContainer<TKey, TEntity>(ICacheContainer<TKey, TEntity> cacheContainer)
        {
            if (cacheContainer != null)
            {
                containers.Add(typeof(TEntity), cacheContainer);
            }
        }

        /// <summary>
        /// Registers the cache container.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="cacheContainer">The cache container.</param>
        internal static void RegisterCacheContainer<TEntity>(ICacheContainer cacheContainer)
        {
            if (cacheContainer != null)
            {
                containers.Add(typeof(TEntity), cacheContainer);
            }
        }

        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;ICacheContainer&gt;.</returns>
        public static List<ICacheContainer> GetContainers<T>()
        {
            return GetContainers(typeof(T));
        }

        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>List&lt;ICacheContainer&gt;.</returns>
        public static List<ICacheContainer> GetContainers(Type type)
        {
            return type != null ? containers[type] : null;
        }

        /// <summary>
        /// Clears all.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public static void ClearAll()
        {
            foreach (var one in containers)
            {
                foreach (var item in one.Value)
                {
                    item.Clear();
                }
            }
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void ClearCache<T>()
        {
            ClearCache(typeof(T));
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Int32.</returns>
        public static void ClearCache(Type type)
        {
            List<ICacheContainer> cacheContainers;

            if (containers.TryGetValue(type, out cacheContainers))
            {
                foreach (var one in cacheContainers)
                {
                    one.Clear();
                }
            }
        }

        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <returns>MatrixList&lt;CacheContainerSummary&gt;.</returns>
        public static MatrixList<MemoryCacheContainerSummary> GetSummary()
        {
            var result = new MatrixList<MemoryCacheContainerSummary>();

            foreach (var item in containers)
            {
                result.Add(item.Key.FullName, item.Value.AsCovariance<ICacheContainer, IMemoryCacheContainer>().ToMemoryCacheContainerSummary());
            }

            return result;
        }

        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>System.Collections.Generic.List&lt;Beyova.Cache.CacheContainerSummary&gt;.</returns>
        public static List<MemoryCacheContainerSummary> GetSummary<T>()
        {
            return ToMemoryCacheContainerSummary(GetContainers(typeof(T)).ConvertAsCollection<ICacheContainer, IMemoryCacheContainer>());
        }

        /// <summary>
        /// To the cache container summary.
        /// </summary>
        /// <param name="containers">The containers.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.Cache.CacheContainerSummary&gt;.</returns>
        private static List<MemoryCacheContainerSummary> ToMemoryCacheContainerSummary(this ICollection<IMemoryCacheContainer> containers)
        {
            var result = new List<MemoryCacheContainerSummary>();

            if (containers != null)
            {
                foreach (var one in containers)
                {
                    result.AddIfNotNull(one.ToMemoryCacheContainerSummary());
                }
            }

            return result;
        }

        /// <summary>
        /// To the memory cache container summary.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        private static MemoryCacheContainerSummary ToMemoryCacheContainerSummary(this IMemoryCacheContainer container)
        {
            return container == null ? null : new MemoryCacheContainerSummary
            {
                Capacity = container.Capacity,
                Count = container.Count,
                ExpirationInSecond = container.ExpirationInSecond,
                Name = container.Name,
                Statistic = container.Statistic
            };
        }
    }
}