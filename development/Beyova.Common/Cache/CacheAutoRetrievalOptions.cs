using System;
using Beyova.ExceptionSystem;

namespace Beyova.Cache
{
    /// <summary>
    /// Class CacheAutoRetrievalOptions defines auto retrieval options for cache. <see cref="ExceptionProcessingImplementation"/> would be only effected during retrieval logic.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class CacheAutoRetrievalOptions<TKey, TEntity>
    {
        /// <summary>
        /// Gets or sets the entity retrieval implementation.
        /// </summary>
        /// <value>
        /// The entity retrieval implementation.
        /// </value>
        public Func<TKey, TEntity> EntityRetrievalImplementation { get; set; }

        /// <summary>
        /// Gets or sets the exception processing implementation.
        /// </summary>
        /// <value>
        /// The exception processing implementation.
        /// </value>
        public Func<BaseException, bool> ExceptionProcessingImplementation { get; set; }

        /// <summary>
        /// Gets or sets the failure expiration in second. If entity is failed to get, use this expiration if specified, otherwise use <see cref="ICacheParameter.ExpirationInSecond" />.
        /// </summary>
        /// <value>
        /// The failure expiration in second.
        /// </value>
        public long FailureExpirationInSecond { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheAutoRetrievalOptions{TKey, TEntity}" /> class.
        /// </summary>
        /// <param name="entityRetrievalImplementation">The entity retrieval implementation.</param>
        /// <param name="exceptionProcessingImplementation">The exception processing implementation.</param>
        /// <param name="failureExpirationInSecond">The failure expiration in second.</param>
        public CacheAutoRetrievalOptions(Func<TKey, TEntity> entityRetrievalImplementation, Func<BaseException, bool> exceptionProcessingImplementation = null, long? failureExpirationInSecond = null)
        {
            entityRetrievalImplementation.CheckNullObject(nameof(entityRetrievalImplementation));

            this.EntityRetrievalImplementation = entityRetrievalImplementation;
            this.ExceptionProcessingImplementation = exceptionProcessingImplementation;

            this.FailureExpirationInSecond = ((failureExpirationInSecond.HasValue && failureExpirationInSecond.Value > 0) ? failureExpirationInSecond : null) ?? DefaultCacheSettings.FailureExpirationInSecond;
        }
    }
}