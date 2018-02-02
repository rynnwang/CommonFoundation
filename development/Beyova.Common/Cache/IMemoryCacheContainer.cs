using System;
using System.Collections.Generic;

namespace Beyova.Cache
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IMemoryCacheContainer<TKey, TEntity> : ICacheContainer<TKey, TEntity>, IMemoryCacheContainer
    {
        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="includeNull">if set to <c>true</c> [include null].</param>
        /// <returns></returns>
        TEntity[] GetAllEntities(bool includeNull = false);
    }

    /// <summary>
    /// Interface ICacheContainer
    /// </summary>
    public interface IMemoryCacheContainer : ICacheContainer
    {
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        int Count { get; }

        /// <summary>
        /// Gets the capacity.
        /// </summary>
        /// <value>The capacity.</value>
        int? Capacity { get; }

        /// <summary>
        /// Gets the statistic.
        /// </summary>
        /// <value>
        /// The statistic.
        /// </value>
        MemoryCacheStatistic Statistic { get; }
    }
}