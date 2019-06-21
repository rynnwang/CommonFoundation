using Newtonsoft.Json;
using System;

namespace Beyova
{
    /// <summary>
    /// </summary>
    public interface IStatusAudit
    {
        /// <summary>
        /// Gets or sets the operated by.
        /// </summary>
        /// <value>
        /// The operated by.
        /// </value>
        Guid? OperatedBy { get; set; }

        /// <summary>
        /// Gets or sets the stamp.
        /// </summary>
        /// <value>
        /// The stamp.
        /// </value>
        DateTime? Stamp { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        string Comments { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        int Status { get; set; }
    }
}