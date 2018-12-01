using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova.Cache
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class CultureBasedMemoryFullEntityCacheContainer<TEntity> : CultureBasedMemoryFullEntityCacheContainer<Guid, TEntity>
        where TEntity : IIdentifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CultureBasedMemoryFullEntityCacheContainer{TEntity}"/> class.
        /// </summary>
        /// <param name="containerOptions">The container options.</param>
        /// <param name="retrievalOptions">The retrieval options.</param>
        public CultureBasedMemoryFullEntityCacheContainer(MemoryCacheContainerOptions<Guid> containerOptions, CultureBasedFullEntityCacheAutoRetrievalOptions<TEntity> retrievalOptions)
         : base(containerOptions, retrievalOptions)
        {
        }

        /// <summary>
        /// Creates the specified entity retrieval implementation.
        /// </summary>
        /// <typeparam name="TCriteria">The type of the criteria.</typeparam>
        /// <param name="entityRetrievalImplementation">The entity retrieval implementation.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static CultureBasedMemoryFullEntityCacheContainer<TEntity> Create<TCriteria>(
           Func<TCriteria, List<TEntity>> entityRetrievalImplementation,
           string name = nameof(TEntity))
           where TCriteria : IGlobalObjectName, new()
        {
            return new CultureBasedMemoryFullEntityCacheContainer<TEntity>(new MemoryCacheContainerOptions<Guid>
            {
                ExpirationInSecond = 60 * 15,
                Name = name
            }, new CultureBasedFullEntityCacheAutoRetrievalOptions<TEntity>((cultureCode) =>
            {
                return entityRetrievalImplementation(new TCriteria { CultureCode = cultureCode });
            }, null));
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="cultureCode">The culture code.</param>
        /// <param name="defaultCultureCode">The default culture code.</param>
        /// <returns></returns>
        public TEntity Get(Guid? key, string cultureCode = null, string defaultCultureCode = null)
        {
            if (key.HasValue)
            {
                cultureCode = (cultureCode.SafeToString(ContextHelper.CurrentCultureInfo?.Name)).SafeToString(defaultCultureCode);
                MemoryFullEntityCacheContainer<Guid, TEntity> container = string.IsNullOrWhiteSpace(cultureCode) ? null : GetCultureSpecificCacheContainer(cultureCode);

                return container == null ? default(TEntity) : container.Get(key.Value);
            }
            else
            {
                return default(TEntity);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class CultureBasedMemoryFullEntityCacheContainer<TKey, TEntity> : ICacheContainer
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the expiration in second.
        /// </summary>
        /// <value>
        /// The expiration in second.
        /// </value>
        public long? ExpirationInSecond { get; protected set; }

        /// <summary>
        /// The container options
        /// </summary>
        protected MemoryCacheContainerOptions<TKey> _containerOptions;

        /// <summary>
        /// The retrieval options
        /// </summary>
        protected CultureBasedFullEntityCacheAutoRetrievalOptions<TKey, TEntity> _retrievalOptions;

        /// <summary>
        /// The root container locker
        /// </summary>
        protected object rootContainerLocker = new object();

        /// <summary>
        /// The cache containers
        /// </summary>
        protected Dictionary<string, MemoryFullEntityCacheContainer<TKey, TEntity>> cacheContainers = new Dictionary<string, MemoryFullEntityCacheContainer<TKey, TEntity>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureBasedMemoryFullEntityCacheContainer{TKey, TEntity}"/> class.
        /// </summary>
        /// <param name="containerOptions">The container options.</param>
        /// <param name="retrievalOptions">The retrieval options.</param>
        public CultureBasedMemoryFullEntityCacheContainer(MemoryCacheContainerOptions<TKey> containerOptions, CultureBasedFullEntityCacheAutoRetrievalOptions<TKey, TEntity> retrievalOptions)
        {
            _containerOptions = containerOptions;
            _retrievalOptions = retrievalOptions;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            lock (rootContainerLocker)
            {
                cacheContainers.Clear();
            }
        }

        /// <summary>
        /// Gets the culture specific cache container.
        /// </summary>
        /// <param name="cultureCode">The culture code.</param>
        /// <returns></returns>
        public MemoryFullEntityCacheContainer<TKey, TEntity> GetCultureSpecificCacheContainer(string cultureCode)
        {
            try
            {
                cultureCode.CheckEmptyString(nameof(cultureCode));
                cultureCode.EnsureCultureCode();

                MemoryFullEntityCacheContainer<TKey, TEntity> result = null;
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    if (cacheContainers.TryGetValue(cultureCode, out result) && !result.IsExpired)
                    {
                        return result;
                    }
                    else
                    {
                        lock (rootContainerLocker)
                        {
                            if (cacheContainers.TryGetValue(cultureCode, out result) && !result.IsExpired)
                            {
                                return result;
                            }
                            else if (_retrievalOptions?.EntityRetrievalImplementation != null)
                            {
                                Func<IEnumerable<TEntity>> implementation = () => { return _retrievalOptions.EntityRetrievalImplementation(cultureCode); };

                                result = new MemoryFullEntityCacheContainer<TKey, TEntity>(
                                    new MemoryCacheContainerOptions<TKey>
                                    {
                                        EqualityComparer = this._containerOptions.EqualityComparer,
                                        ExpirationInSecond = this._containerOptions.ExpirationInSecond,
                                        Name = string.Format("full-{0}-{1}", this._containerOptions.Name, cultureCode)
                                    },
                                    new FullEntityCacheAutoRetrievalOptions<TKey, TEntity>(
                                        implementation,
                                        _retrievalOptions.EntityKeyGetter,
                                        _retrievalOptions.ExceptionProcessingImplementation,
                                        _retrievalOptions.FailureExpirationInSecond));

                                cacheContainers.Merge(cultureCode, result);

                                return result;
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { cultureCode });
            }
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="cultureCode">The culture code.</param>
        /// <param name="defaultCultureCode">The default culture code.</param>
        /// <returns></returns>
        public TEntity Get(TKey key, string cultureCode = null, string defaultCultureCode = null)
        {
            cultureCode = (cultureCode.SafeToString(ContextHelper.CurrentCultureInfo?.Name)).SafeToString(defaultCultureCode);
            MemoryFullEntityCacheContainer<TKey, TEntity> container = string.IsNullOrWhiteSpace(cultureCode) ? null : GetCultureSpecificCacheContainer(cultureCode);

            return container == null ? default(TEntity) : container.Get(key);
        }
    }
}
