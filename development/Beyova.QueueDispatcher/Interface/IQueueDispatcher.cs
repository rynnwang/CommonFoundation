using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova.QueueDispatch
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQueueDispatcher
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        QueueDispatchOptions Options { get; }
    }
}
