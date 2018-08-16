using System;
using Beyova.ExceptionSystem;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityInstructionResultBase.
    /// </summary>
    public abstract class GravityInstructionResultBase : IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the instruction key.
        /// </summary>
        /// <value>
        /// The instruction key.
        /// </value>
        public Guid? InstructionKey { get; set; }

        /// <summary>
        /// Gets or sets the client key.
        /// </summary>
        /// <value>The client key.</value>
        public Guid? ClientKey { get; set; }
    }
}