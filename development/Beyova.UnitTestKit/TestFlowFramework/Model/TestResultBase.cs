using System;

namespace Beyova.UnitTestKit
{
    /// <summary>
    /// Class TestResultBase.
    /// </summary>
    public abstract class TestResultBase : IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; protected set; }

        /// <summary>
        /// Gets or sets the input object.
        /// </summary>
        /// <value>The input object.</value>
        public object InputObject { get; set; }

        /// <summary>
        /// Gets or sets the enter stamp.
        /// </summary>
        /// <value>The enter stamp.</value>
        public DateTime? EnterStamp { get; set; }

        /// <summary>
        /// Gets or sets the output object.
        /// </summary>
        /// <value>The output object.</value>
        public object OutputObject { get; set; }

        /// <summary>
        /// Gets or sets the exit stamp.
        /// </summary>
        /// <value>The exit stamp.</value>
        public DateTime?ExitStamp { get; set; }

        /// <summary>
        /// Gets or sets the expection.
        /// </summary>
        /// <value>The expection.</value>
        public object Expection { get; set; }
    }
}
