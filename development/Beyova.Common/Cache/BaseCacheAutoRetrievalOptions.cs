using Beyova.ExceptionSystem;
using System;

namespace Beyova.Cache
{
    /// <summary>
    /// Class BaseCacheAutoRetrievalOptions defines auto retrieval options for cache. <see cref="ExceptionProcessingImplementation"/> would be only effected during retrieval logic.
    /// </summary>
    public class BaseCacheAutoRetrievalOptions
    {
        /// <summary>
        /// The default exception handler
        /// </summary>
        private static Func<BaseException, bool> defaultExceptionHandler = (baseEx) =>
        {
            if (baseEx != null)
            {
                if (Framework.ApiTracking != null)
                {
                    Framework.ApiTracking.LogException(baseEx.ToExceptionInfo());
                    return false;
                }
            }

            return true;
        };

        /// <summary>
        /// Gets or sets the exception processing implementation. Return true if need to further process.
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
        /// Initializes a new instance of the <see cref="BaseCacheAutoRetrievalOptions" /> class.
        /// </summary>
        /// <param name="exceptionProcessingImplementation">The exception processing implementation.</param>
        /// <param name="failureExpirationInSecond">The failure expiration in second.</param>
        protected BaseCacheAutoRetrievalOptions(Func<BaseException, bool> exceptionProcessingImplementation = null, long? failureExpirationInSecond = null)
        {
            this.ExceptionProcessingImplementation = exceptionProcessingImplementation ?? defaultExceptionHandler;
            this.FailureExpirationInSecond = (failureExpirationInSecond.HasValue && failureExpirationInSecond.Value > 0) ? failureExpirationInSecond.Value : DefaultCacheSettings.FailureExpirationInSecond;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCacheAutoRetrievalOptions" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        protected BaseCacheAutoRetrievalOptions(BaseCacheAutoRetrievalOptions options) : this(options?.ExceptionProcessingImplementation, options?.FailureExpirationInSecond)
        {
        }

        /// <summary>
        /// Gets the failure expired stamp.
        /// </summary>
        /// <returns></returns>
        public DateTime? GetFailureExpiredStamp()
        {
            return DateTime.UtcNow.AddSeconds(this.FailureExpirationInSecond) as DateTime?;
        }

        #region Default

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static BaseCacheAutoRetrievalOptions Default { get; } = new BaseCacheAutoRetrievalOptions();

        #endregion Default
    }
}