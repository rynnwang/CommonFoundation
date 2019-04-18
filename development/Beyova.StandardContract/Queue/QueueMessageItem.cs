using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueMessageItem<T>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>
        /// The created stamp.
        /// </value>
        public DateTime? CreatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public T Message { get; set; }

        /// <summary>
        /// Gets or sets the pop receipt.
        /// </summary>
        /// <value>
        /// The pop receipt.
        /// </value>
        public string PopReceipt { get; set; }

        /// <summary>
        /// Gets or sets the revisible stamp.
        /// </summary>
        /// <value>
        /// The revisible stamp.
        /// </value>
        public DateTime? RevisibleStamp { get; set; }
    }
}