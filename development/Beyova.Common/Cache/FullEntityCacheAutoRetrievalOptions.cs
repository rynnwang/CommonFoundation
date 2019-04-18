using System;
using System.Collections.Generic;
using Beyova.Diagnostic;

namespace Beyova.Cache
{
    /// <summary>
    /// Class FullEntityCacheAutoRetrievalOptions defines auto retrieval options for cache. <see cref="BaseCacheAutoRetrievalOptions.ExceptionProcessingImplementation" /> would be only effected during retrieval logic.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class FullEntityCacheAutoRetrievalOptions<TEntity> : FullEntityCacheAutoRetrievalOptions<Guid, TEntity>
        where TEntity : IIdentifier
    {
        /// <summary>
        /// The default i identifier key getter
        /// </summary>
        private static Func<TEntity, Guid> _defaultIIdentifierKeyGetter = entity =>
         {
             entity.CheckNullObject(nameof(entity));
             entity.Key.CheckNullObject(nameof(entity.Key));
             return entity.Key.Value;
         };

        /// <summary>
        /// Initializes a new instance of the <see cref="FullEntityCacheAutoRetrievalOptions{TEntity}"/> class.
        /// </summary>
        /// <param name="entityRetrievalImplementation">The entity retrieval implementation.</param>
        /// <param name="retirevalOptions">The retireval options.</param>
        public FullEntityCacheAutoRetrievalOptions(
            Func<IEnumerable<TEntity>> entityRetrievalImplementation,
            BaseCacheAutoRetrievalOptions retirevalOptions)
            : base(entityRetrievalImplementation, _defaultIIdentifierKeyGetter, retirevalOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FullEntityCacheAutoRetrievalOptions{TEntity}"/> class.
        /// </summary>
        /// <param name="entityRetrievalImplementation">The entity retrieval implementation.</param>
        /// <param name="exceptionProcessingImplementation">The exception processing implementation.</param>
        /// <param name="failureExpirationInSecond">The failure expiration in second.</param>
        public FullEntityCacheAutoRetrievalOptions(
            Func<IEnumerable<TEntity>> entityRetrievalImplementation,
            Func<BaseException, bool> exceptionProcessingImplementation = null,
            long? failureExpirationInSecond = null)
            : base(entityRetrievalImplementation, _defaultIIdentifierKeyGetter, exceptionProcessingImplementation, failureExpirationInSecond)
        {
        }
    }

    /// <summary>
    /// Class FullEntityCacheAutoRetrievalOptions defines auto retrieval options for cache. <see cref="BaseCacheAutoRetrievalOptions.ExceptionProcessingImplementation" /> would be only effected during retrieval logic.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the ntity.</typeparam>
    public class FullEntityCacheAutoRetrievalOptions<TKey, TEntity> : BaseCacheAutoRetrievalOptions
    {
        /// <summary>
        /// Gets or sets the entity retrieval implementation.
        /// </summary>
        /// <value>
        /// The entity retrieval implementation.
        /// </value>
        public Func<IEnumerable<TEntity>> EntityRetrievalImplementation { get; set; }

        /// <summary>
        /// Gets or sets the entity key getter.
        /// </summary>
        /// <value>
        /// The entity key getter.
        /// </value>
        public Func<TEntity, TKey> EntityKeyGetter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheAutoRetrievalOptions{TKey, TEntity}" /> class.
        /// </summary>
        /// <param name="entityRetrievalImplementation">The entity retrieval implementation.</param>
        /// <param name="entityKeyGetter">The entity key getter.</param>
        /// <param name="exceptionProcessingImplementation">The exception processing implementation.</param>
        /// <param name="failureExpirationInSecond">The failure expiration in second.</param>
        public FullEntityCacheAutoRetrievalOptions(Func<IEnumerable<TEntity>> entityRetrievalImplementation, Func<TEntity, TKey> entityKeyGetter, Func<BaseException, bool> exceptionProcessingImplementation = null, long? failureExpirationInSecond = null)
            : base(exceptionProcessingImplementation, failureExpirationInSecond)
        {
            EntityRetrievalImplementation = entityRetrievalImplementation;
            EntityKeyGetter = entityKeyGetter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FullEntityCacheAutoRetrievalOptions{TKey, TEntity}"/> class.
        /// </summary>
        /// <param name="entityRetrievalImplementation">The entity retrieval implementation.</param>
        /// <param name="entityKeyGetter">The entity key getter.</param>
        /// <param name="baseRetrievalOptions">The base retrieval options.</param>
        public FullEntityCacheAutoRetrievalOptions(Func<IEnumerable<TEntity>> entityRetrievalImplementation, Func<TEntity, TKey> entityKeyGetter, BaseCacheAutoRetrievalOptions baseRetrievalOptions)
           : base(baseRetrievalOptions)
        {
            EntityRetrievalImplementation = entityRetrievalImplementation;
            EntityKeyGetter = entityKeyGetter;
        }
    }
}