using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Beyova.ExceptionSystem;

namespace Beyova.Cache
{
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

            this._originContainer = equalityComparer == null ? new Dictionary<TKey, TEntity>() : new Dictionary<TKey, TEntity>(equalityComparer);

            this.ContainerOptions = containerOptions ?? new CacheContainerOptions();
            this.AutoRetrievalOptions = retrievalOptions ?? new FullEntityCacheAutoRetrievalOptions<TKey, TEntity>(null, null, BaseCacheAutoRetrievalOptions.Default);

            CacheRealm.RegisterCacheContainer<TEntity>(this);
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

            if (this.AutoRetrievalOptions?.EntityRetrievalImplementation != null && this.AutoRetrievalOptions?.EntityKeyGetter != null)
            {
                try
                {
                    var entities = AutoRetrievalOptions.EntityRetrievalImplementation();
                    this._originContainer.Clear();
                    foreach (var one in entities)
                    {
                        var key = this.AutoRetrievalOptions.EntityKeyGetter(one);
                        if (key != null)
                        {
                            this._originContainer.Add(key, one);
                        }
                    }

                    _readOnlyContainer = new ReadOnlyDictionary<TKey, TEntity>(this._originContainer);
                    result = ContainerOptions.GetExpiredStamp();
                }
                catch (Exception ex)
                {
                    BaseException exception = ex.Handle();
                    result = this.AutoRetrievalOptions.GetFailureExpiredStamp();

                    if (this.AutoRetrievalOptions.ExceptionProcessingImplementation == null || this.AutoRetrievalOptions.ExceptionProcessingImplementation(exception))
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
                        this.ExpiredStamp = InternalUpdate();
                    }
                }
            }

            return _readOnlyContainer.Values.ToArray();
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
                        this.ExpiredStamp = InternalUpdate();
                    }
                }
            }

            TEntity result;
            return key != null && this._readOnlyContainer.TryGetValue(key, out result) ? result : default(TEntity);
        }

        /// <summary>
        /// Internally clear all items in cache container. It has locker inside.
        /// </summary>
        public void Clear()
        {
            lock (_itemChangeLocker)
            {
                this.ExpiredStamp = DateTime.MinValue;
                this._originContainer.Clear();                
            }
        }
    }
}