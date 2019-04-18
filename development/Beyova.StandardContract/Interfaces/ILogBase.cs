using System;

namespace Beyova
{
    /// <summary>
    /// Interface ILogBase.
    /// </summary>
    public interface ILogBase : IIdentifier
    {
        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>
        /// The created stamp.
        /// </value>
        DateTime? CreatedStamp { get; set; }
    }
}