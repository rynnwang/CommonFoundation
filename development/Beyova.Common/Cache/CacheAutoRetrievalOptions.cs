using System;
using Beyova.ExceptionSystem;

namespace Beyova.Cache
{
    /// <summary>
    /// Class CacheAutoRetrievalOptions defines auto retrieval options for cache. <see cref="BaseCacheAutoRetrievalOptions.ExceptionProcessingImplementation"/> would be only effected during retrieval logic.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class CacheAutoRetrievalOptions<TKey, TEntity> : BaseCacheAutoRetrievalOptions
    {
        /// <summary>
        /// Gets or sets the entity retrieval implementation.
        /// </summary>
        /// <value>
        /// The entity retrieval implementation.
        /// </value>
        public Func<TKey, TEntity> EntityRetrievalImplementation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheAutoRetrievalOptions{TKey, TEntity}" /> class.
        /// </summary>
        /// <param name="entityRetrievalImplementation">The entity retrieval implementation.</param>
        /// <param name="exceptionProcessingImplementation">The exception processing implementation.</param>
        /// <param name="failureExpirationInSecond">The failure expiration in second.</param>
        public CacheAutoRetrievalOptions(
            Func<TKey, TEntity> entityRetrievalImplementation,
            Func<BaseException, bool> exceptionProcessingImplementation = null,
            long? failureExpirationInSecond = null)
            : base(exceptionProcessingImplementation, failureExpirationInSecond)
        {
            entityRetrievalImplementation.CheckNullObject(nameof(entityRetrievalImplementation));
            this.EntityRetrievalImplementation = entityRetrievalImplementation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheAutoRetrievalOptions{TKey, TEntity}"/> class.
        /// </summary>
        /// <param name="entityRetrievalImplementation">The entity retrieval implementation.</param>
        /// <param name="cacheRetrievalOptions">The cache retrieval options.</param>
        public CacheAutoRetrievalOptions(Func<TKey, TEntity> entityRetrievalImplementation, BaseCacheAutoRetrievalOptions cacheRetrievalOptions)
            : base(cacheRetrievalOptions)
        {
            entityRetrievalImplementation.CheckNullObject(nameof(entityRetrievalImplementation));
            this.EntityRetrievalImplementation = entityRetrievalImplementation;
        }
    }
}