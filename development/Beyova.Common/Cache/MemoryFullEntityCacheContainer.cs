using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Beyova.Diagnostic;

namespace Beyova.Cache
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class MemoryFullEntityCacheContainer<TEntity> : MemoryFullEntityCacheContainer<Guid, TEntity>
        where TEntity : IIdentifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryFullEntityCacheContainer{TEntity}"/> class.
        /// </summary>
        /// <param name="containerOptions">The container options.</param>
        /// <param name="retrievalOptions">The retrieval options.</param>
        public MemoryFullEntityCacheContainer(MemoryCacheContainerOptions<Guid> containerOptions, FullEntityCacheAutoRetrievalOptions<TEntity> retrievalOptions)
            : base(containerOptions, retrievalOptions)
        {
        }
    }

    /// <summary>
    /// Class MemoryFullEntityCacheContainer.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class MemoryFullEntityCacheContainer<TKey, TEntity> : ICacheContainer
    {
        /// <summary>
        /// Gets or sets the expired stamp.
        /// </summary>
        /// <value>
        /// The expired stamp.
        /// </value>
        public DateTime? ExpiredStamp { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this instance is expired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is expired; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpired
        {
            get { return ExpiredStamp.HasValue && ExpiredStamp < DateTime.UtcNow; }
        }

        /// <summary>
        /// The container
        /// </summary>
        protected Dictionary<TKey, TEntity> _originContainer;

        /// <summary>
        /// The read only container
        /// </summary>
        protected ReadOnlyDictionary<TKey, TEntity> _readOnlyContainer;

        /// <summary>
        /// The retrieval options
        /// </summary>
        /// <value>
        /// The automatic retrieval options.
        /// </value>
        public FullEntityCacheAutoRetrievalOptions<TKey, TEntity> AutoRetrievalOptions { get; protected set; }

        /// <summary>
        /// Gets or sets the container options.
        /// </summary>
        /// <value>
        /// The container options.
        /// </value>
        public CacheContainerOptions ContainerOptions { get; protected set; }

        /// <summary>
        /// The item change locker
        /// </summary>
        protected object _itemChangeLocker = new object();

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get { return _originContainer.Count; } }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get { return ContainerOptions.Name; } }

        /// <summary>
        /// Gets or sets the expiration in second.
        /// </summary>
        /// <value>
        /// The expiration in second.
        /// </value>
        public long? ExpirationInSecond { get { return ContainerOptions.ExpirationInSecond; } }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryFullEntityCacheContainer{TKey, TEntity}"/> class.
        /// </summary>
        /// <param name="containerOptions">The container options.</param>
        /// <param name="retrievalOptions">The retrieval options.</param>
        public MemoryFullEntityCacheContainer(MemoryCacheContainerOptions<TKey> containerOptions, FullEntityCacheAutoRetrievalOptions<TKey, TEntity> retrievalOptions)
        {
            var equalityComparer = containerOptions?.EqualityComparer ?? EqualityComparer<TKey>.Default;

            _originContainer = equalityComparer == null ? new Dictionary<TKey, TEntity>() : new Dictionary<TKey, TEntity>(equalityComparer);

            ContainerOptions = containerOptions ?? new CacheContainerOptions();
            AutoRetrievalOptions = retrievalOptions ?? new FullEntityCacheAutoRetrievalOptions<TKey, TEntity>(null, null, BaseCacheAutoRetrievalOptions.Default);

            CacheRealm.RegisterCacheContainer<TEntity>(this);

            ExpiredStamp = InternalUpdate();
        }

        #endregion Constructors

        #region protected methods

        /// <summary>
        /// Internals the update.
        /// </summary>
        /// <returns>
        /// System.Nullable&lt;DateTime&gt;.
        /// </returns>
        protected DateTime? InternalUpdate()
        {
            DateTime? result = null;

            if (AutoRetrievalOptions?.EntityRetrievalImplementation != null && AutoRetrievalOptions?.EntityKeyGetter != null)
            {
                try
                {
                    // Regarding check null already, not need to worry about null ref.
                    var entities = AutoRetrievalOptions.EntityRetrievalImplementation();
                    _originContainer.Clear();
                    foreach (var one in entities)
                    {
                        // Regarding check null already, not need to worry about null ref.
                        var key = AutoRetrievalOptions.EntityKeyGetter(one);
                        if (key != null)
                        {
                            _originContainer.Add(key, one);
                        }
                    }

                    _readOnlyContainer = new ReadOnlyDictionary<TKey, TEntity>(_originContainer);
                    result = ContainerOptions.GetExpiredStamp();
                }
                catch (Exception ex)
                {
                    BaseException exception = ex.Handle();
                    result = AutoRetrievalOptions.GetFailureExpiredStamp();

                    if (AutoRetrievalOptions.ExceptionProcessingImplementation == null || AutoRetrievalOptions.ExceptionProcessingImplementation(exception))
                    {
                        throw exception;
                    }
                }
            }

            return result;
        }

        #endregion protected methods

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns></returns>
        public TEntity[] GetAllEntities()
        {
            if (IsExpired)
            {
                lock (_itemChangeLocker)
                {
                    if (IsExpired)
                    {
                        ExpiredStamp = InternalUpdate();
                    }
                }
            }

            return _readOnlyContainer?.Values.ToArray() ?? new TEntity[] { };
        }

        /// <summary>
        /// Queries the specified predict.
        /// </summary>
        /// <param name="predict">The predict.</param>
        /// <returns></returns>
        public IEnumerable<TEntity> Query(Func<TEntity, bool> predict)
        {
            return predict == null ? (_readOnlyContainer?.Values) : _readOnlyContainer?.Values?.Where(predict);
        }

        /// <summary>
        /// Expires this instance.
        /// </summary>
        public void Expire()
        {
            lock (_itemChangeLocker)
            {
                ExpiredStamp = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Gets the specified predict.
        /// </summary>
        /// <param name="predict">The predict.</param>
        /// <returns></returns>
        public TEntity Get(Func<TEntity, bool> predict)
        {
            if (predict != null && _readOnlyContainer != null && _readOnlyContainer.Values != null)
            {
                foreach (var item in _readOnlyContainer.Values)
                {
                    if (predict(item))
                    {
                        return item;
                    }
                }
            }
            return default(TEntity);
        }

        /// <summary>
        /// Gets the specified selector.
        /// </summary>
        /// <typeparam name="TComparableType">The type of the comparable type.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="equaltyValue">The equalty value.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public TEntity Get<TComparableType>(Func<TEntity, TComparableType> selector, TComparableType equaltyValue, IEqualityComparer<TComparableType> comparer = null)
        {
            if (selector != null && comparer != null && _readOnlyContainer != null && _readOnlyContainer.Values != null)
            {
                var equaltyComparer = new LambdaEqualityComparer<TEntity, TComparableType>(selector, comparer);

                foreach (var item in _readOnlyContainer.Values)
                {
                    if (equaltyComparer.Equals(item, equaltyValue))
                    {
                        return item;
                    }
                }
            }
            return default(TEntity);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual TEntity Get(TKey key)
        {
            if (IsExpired)
            {
                lock (_itemChangeLocker)
                {
                    if (IsExpired)
                    {
                        ExpiredStamp = InternalUpdate();
                    }
                }
            }

            TEntity result;
            return (key != null && _readOnlyContainer != null && _readOnlyContainer.TryGetValue(key, out result)) ? result : default(TEntity);
        }

        /// <summary>
        /// Internally clear all items in cache container. It has locker inside.
        /// </summary>
        public void Clear()
        {
            lock (_itemChangeLocker)
            {
                ExpiredStamp = DateTime.MinValue;
                _originContainer.Clear();
            }
        }
    }
}