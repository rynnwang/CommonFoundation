using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova.QueueDispatch
{
    /// <summary>
    /// 
    /// </summary>
    public class QueueDispatchOptions
    {
        /// <summary>
        /// Gets or sets the idle interval. Unit: Second
        /// </summary>
        /// <value>
        /// The idle interval.
        /// </value>
        public int IdleInterval { get; set; }

        /// <summary>
        /// Gets or sets the size of the batch.
        /// </summary>
        /// <value>
        /// The size of the batch.
        /// </value>
        public int? BatchSize { get; set; }

        /// <summary>
        /// Gets or sets the invisibility timeout.
        /// </summary>
        /// <value>
        /// The invisibility timeout.
        /// </value>
        public int? InvisibilityTimeout { get; set; }
    }
}
