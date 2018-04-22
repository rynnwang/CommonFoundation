using System;
using Beyova.ExceptionSystem;

namespace Beyova.Cache
{
    /// <summary>
    /// Class CacheContainerBase. It defines basic flow for cache logic.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class CacheContainerBase<TKey, TEntity> : ICacheContainer<TKey, TEntity>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return ContainerOptions.Name;
            }
        }

        /// <summary>
        /// Gets or sets the expiration in second.
        /// </summary>
        /// <value>
        /// The expiration in second.
        /// </value>
        public long? ExpirationInSecond
        {
            get
            {
                return ContainerOptions.ExpirationInSecond;
            }
        }

        /// <summary>
        /// Gets or sets the automatic retrieval options.
        /// </summary>
        /// <value>
        /// The automatic retrieval options.
        /// </value>
        public CacheAutoRetrievalOptions<TKey, TEntity> AutoRetrievalOptions { get; protected set; }

        /// <summary>
        /// Gets or sets the container options.
        /// </summary>
        /// <value>
        /// The container options.
        /// </value>
        public CacheContainerOptions ContainerOptions { get; protected set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheContainerBase{TKey, TEntity}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="expirationInSecond">The expiration in second.</param>
        /// <param name="retrievalOptions">The retrieval options.</param>
        [Obsolete]
        public CacheContainerBase(string name, long? expirationInSecond = null, CacheAutoRetrievalOptions<TKey, TEntity> retrievalOptions = null)
              : this(new CacheContainerOptions { Name = name, ExpirationInSecond = expirationInSecond }, retrievalOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheContainerBase{TKey, TEntity}"/> class.
        /// </summary>
        /// <param name="containerOptions">The container options.</param>
        /// <param name="retrievalOptions">The retrieval options.</param>
        public CacheContainerBase(CacheContainerOptions containerOptions, CacheAutoRetrievalOptions<TKey, TEntity> retrievalOptions = null)
        {
            containerOptions.CheckNullObject(nameof(containerOptions));
            retrievalOptions.CheckNullObject(nameof(retrievalOptions));

            this.ContainerOptions = containerOptions ?? new CacheContainerOptions();
            this.ContainerOptions.Name = this.ContainerOptions.Name.SafeToString(typeof(TEntity).GetFullName());

            var expirationInSecond = containerOptions.ExpirationInSecond;
            this.AutoRetrievalOptions = retrievalOptions ?? new CacheAutoRetrievalOptions<TKey, TEntity>(null, BaseCacheAutoRetrievalOptions.Default);

            CacheRealm.RegisterCacheContainer(this);
        }

        #endregion Constructors

        #region Public methods

        /// <summary>
        /// Updates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
        public virtual void Update(TKey key, TEntity entity)
        {
            InternalUpdate(key, entity, true, this.ContainerOptions.GetExpiredStamp);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Get(TKey key)
        {
            TEntity entity;

            bool isHit = true;
            bool isFailure = false;
            if (!InternalTryGetValidEntityFromCache(key, out entity))
            {
                isHit = false;
                if (this.AutoRetrievalOptions?.EntityRetrievalImplementation != null)
                {
                    try
                    {
                        entity = AutoRetrievalOptions.EntityRetrievalImplementation(key);
                        InternalUpdate(key, entity, true, this.ContainerOptions.GetExpiredStamp);
                    }
                    catch (Exception ex)
                    {
                        isFailure = true;
                        if (this.AutoRetrievalOptions.ExceptionProcessingImplementation != null)
                        {
                            BaseException exception = ex.Handle(key);
                            if (this.AutoRetrievalOptions.ExceptionProcessingImplementation(exception))
                            {
                                UpdateCounter(isHit, isFailure);
                                throw exception;
                            }
                        }
                        else
                        {
                            InternalUpdate(key, default(TEntity), true, this.AutoRetrievalOptions.GetFailureExpiredStamp);
                        }
                    }
                }
                else
                {
                    entity = default(TEntity);
                }
            }

            UpdateCounter(isHit, isFailure);
            return entity;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public virtual void Clear()
        {
            InternalClear();
        }

        #endregion Public methods

        #region Cache implementation related methods

        /// <summary>
        /// Tries the get valid entity from cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected abstract bool InternalTryGetValidEntityFromCache(TKey key, out TEntity entity);

        /// <summary>
        /// Internals the update.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="ifNotExistsThenInsert">if set to <c>true</c> [if not exists then insert].</param>
        /// <param name="getExpiredStamp">The get expired stamp.</param>
        /// <returns></returns>
        protected abstract DateTime? InternalUpdate(TKey key, TEntity entity, bool ifNotExistsThenInsert, Func<DateTime?> getExpiredStamp);

        /// <summary>
        /// Internals the clear.
        /// </summary>
        protected abstract void InternalClear();

        /// <summary>
        /// Updates the counter.
        /// </summary>
        /// <param name="isHit">if set to <c>true</c> [is hit].</param>
        /// <param name="hasException">if set to <c>true</c> [has exception].</param>
        protected virtual void UpdateCounter(bool isHit, bool hasException) { }

        #endregion Cache implementation related methods
    }
}