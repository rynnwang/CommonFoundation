using System;

namespace Beyova.Cache
{
    /// <summary>
    /// Class MemoryCacheItem.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MemoryCacheItem<T> : IExpirable
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

        /// <summary>
        /// Expires this instance.
        /// </summary>
        public void Expire()
        {
            ExpiredStamp = DateTime.UtcNow.AddMinutes(-1);
        }
    }
}
