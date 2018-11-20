using System;

namespace Beyova.Cache
{
    /// <summary>
    /// Class CacheContainerOptions
    /// </summary>
    public class CacheContainerOptions
    {
        /// <summary>
        /// The expioration in second
        /// </summary>
        protected long? _expiorationInSecond;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

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
                return _expiorationInSecond;
            }
            set
            {
                _expiorationInSecond = (value.HasValue && value.Value > 0) ? value : null;
            }
        }

        /// <summary>
        /// Gets the expired stamp.
        /// </summary>
        /// <returns></returns>
        public DateTime? GetExpiredStamp()
        {
            return this.ExpirationInSecond.HasValue ? DateTime.UtcNow.AddSeconds(this.ExpirationInSecond.Value) as DateTime? : null;
        }
    }
}