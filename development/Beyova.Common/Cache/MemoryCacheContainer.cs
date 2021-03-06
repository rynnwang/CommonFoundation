﻿using System;
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
        /// <summary>
        /// The container
        /// </summary>
        protected SequencedKeyDictionary<TKey, MemoryCacheItem<TEntity>> container;

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
            Capacity = (capacity.HasValue && capacity.Value > 1) ? capacity : null;
            var equalityComparer = containerOptions?.EqualityComparer ?? EqualityComparer<TKey>.Default;

            container = capacity == null ? new SequencedKeyDictionary<TKey, MemoryCacheItem<TEntity>>(equalityComparer) : new SequencedKeyDictionary<TKey, MemoryCacheItem<TEntity>>(capacity.Value, equalityComparer);
            Statistic = new MemoryCacheStatistic();
        }

        #endregion Constructors

        #region protected methods

        /// <summary>
        /// Internals the maintain capacity.
        /// </summary>
        protected void InternalMaintainCapacity()
        {
            if (Capacity.HasValue && container.Count > Capacity.Value)
            {
                container.RemoveAt(0);
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
                        container.Add(key, new MemoryCacheItem<TEntity> { Value = entity, ExpiredStamp = result });
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
        /// Clears all.
        /// </summary>
        public void ClearAll()
        {
            InternalClear();
        }

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
            MemoryCacheItem<TEntity> cachedObject;
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
        /// Expireses the specific item.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Expires(TKey key)
        {
            MemoryCacheItem<TEntity> cachedObject;
            if (key != null && container.TryGetValue(key, out cachedObject))
            {
                cachedObject.Expire();
            }
        }

        /// <summary>
        /// Internally clear all items in cache container. It has locker inside.
        /// </summary>
        protected override void InternalClear()
        {
            lock (itemChangeLocker)
            {
                container.Clear();
            }
        }
    }
}