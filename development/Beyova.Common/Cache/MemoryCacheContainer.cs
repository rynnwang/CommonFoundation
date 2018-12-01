using System;
using System.Collections.Generic;
using System.Linq;

namespace Beyova.Cache
{
    /// <summary>
    /// Class MemoryCacheContainer.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class MemoryCacheContainer<TKey, TEntity> : CacheContainerBase<TKey, TEntity>, IMemoryCacheContainer<TKey, TEntity>
    {
        #region Inner Container

        /// <summary>
        /// Class CacheItem.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected class CacheItem<T> : IExpirable
        {
            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public T Value { get; set; }

            /// <summary>
            /// Gets or sets the expired stamp.
            /// </summary>
            /// <value>The expired stamp.</value>
            public DateTime? ExpiredStamp { get; set; }

            /// <summary>
            /// Gets a value indicating whether this instance is expired.
            /// </summary>
            /// <value><c>true</c> if this instance is expired; otherwise, <c>false</c>.</value>
            public bool IsExpired
            {
                get { return ExpiredStamp.HasValue && ExpiredStamp < DateTime.UtcNow; }
            }
        }

        #endregion Inner Container

        /// <summary>
        /// The container
        /// </summary>
        protected SequencedKeyDictionary<TKey, CacheItem<TEntity>> container;

        /// <summary>
        /// The item change locker
        /// </summary>
        protected object itemChangeLocker = new object();

        /// <summary>
        /// The capacity
        /// </summary>
        public int? Capacity { get; protected set; }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get { return container.Count; } }

        /// <summary>
        /// Gets or sets the statistic.
        /// </summary>
        /// <value>
        /// The statistic.
        /// </value>
        public MemoryCacheStatistic Statistic { get; protected set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheContainer{TKey, TEntity}"/> class.
        /// </summary>
        /// <param name="containerOptions">The container options.</param>
        /// <param name="retrievalOptions">The retrieval options.</param>
        public MemoryCacheContainer(MemoryCacheContainerOptions<TKey> containerOptions, CacheAutoRetrievalOptions<TKey, TEntity> retrievalOptions = null)
       : base(containerOptions, retrievalOptions)
        {
            var capacity = containerOptions?.Capacity;
            this.Capacity = (capacity.HasValue && capacity.Value > 1) ? capacity : null;
            var equalityComparer = containerOptions?.EqualityComparer ?? EqualityComparer<TKey>.Default;

            this.container = capacity == null ? new SequencedKeyDictionary<TKey, CacheItem<TEntity>>(equalityComparer) : new SequencedKeyDictionary<TKey, CacheItem<TEntity>>(capacity.Value, equalityComparer);
            this.Statistic = new MemoryCacheStatistic();
        }

        #endregion Constructors

        #region protected methods

        /// <summary>
        /// Internals the maintain capacity.
        /// </summary>
        protected void InternalMaintainCapacity()
        {
            if (this.Capacity.HasValue && this.container.Count > this.Capacity.Value)
            {
                this.container.RemoveAt(0);
            }
        }

        /// <summary>
        /// Internals the update.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="ifNotExistsThenInsert">if set to <c>true</c> [if not exists then insert].</param>
        /// <param name="getExpiredStamp">The get expired stamp.</param>
        /// <returns>
        /// System.Nullable&lt;DateTime&gt;.
        /// </returns>
        protected override DateTime? InternalUpdate(TKey key, TEntity entity, bool ifNotExistsThenInsert, Func<DateTime?> getExpiredStamp)
        {
            DateTime? result = null;

            if (key != null)
            {
                try
                {
                    lock (itemChangeLocker)
                    {
                        // NOTE: Force to remove then add, is to ensure the sequence is correct. Otherwise, the most frequency updated item would still be knick out the container due to capacity limits.
                        if (container.ContainsKey(key))
                        {
                            container.Remove(key);
                        }
                        else
                        {
                            if (!ifNotExistsThenInsert)
                            {
                                return result;
                            }
                        }

                        result = getExpiredStamp();
                        container.Add(key, new CacheItem<TEntity> { Value = entity, ExpiredStamp = result });
                        InternalMaintainCapacity();
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(key);
                }
            }

            return result;
        }

        #endregion protected methods

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns></returns>
        public TEntity[] GetAllEntities(bool includeNull = false)
        {
            lock (itemChangeLocker)
            {
                return container.Values.Where(x => x != null && (includeNull || x.Value != null)).Select(x => x.Value).ToArray();
            }
        }

        /// <summary>
        /// Internally try get valid entity from cache. It has no locker inside.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected override bool InternalTryGetValidEntityFromCache(TKey key, out TEntity entity)
        {
            CacheItem<TEntity> cachedObject;
            if ((container.TryGetValue(key, out cachedObject) && !cachedObject.IsExpired))
            {
                entity = cachedObject.Value;
                return true;
            }
            else
            {
                entity = default(TEntity);
                return false;
            }
        }

        /// <summary>
        /// Internally clear all items in cache container. It has locker inside.
        /// </summary>
        protected override void InternalClear()
        {
            lock (itemChangeLocker)
            {
                this.container.Clear();
            }
        }
    }
}