using Beyova.Diagnostic;

namespace Beyova.QueueDispatch
{
    /// <summary>
    ///
    /// </summary>
    public class QueueDispatcherResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is succeed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is succeed; otherwise, <c>false</c>.
        /// </value>
        public bool IsSucceed { get; set; }

        /// <summary>
        /// Gets or sets the receipt.
        /// </summary>
        /// <value>
        /// The receipt.
        /// </value>
        public string Receipt { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public BaseException Exception { get; set; }
    }
}