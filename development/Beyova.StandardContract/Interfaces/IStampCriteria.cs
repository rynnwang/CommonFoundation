using System;

namespace Beyova
{
    /// <summary>
    /// Interface IStampCriteria
    /// </summary>
    public interface IStampCriteria
    {
        #region Properties

        /// <summary>
        /// Gets or sets from stamp.
        /// </summary>
        /// <value>
        /// From stamp.
        /// </value>
        DateTime? FromStamp { get; set; }

        /// <summary>
        /// Gets or sets to stamp.
        /// </summary>
        /// <value>
        /// To stamp.
        /// </value>
        DateTime? ToStamp { get; set; }

        #endregion Properties
    }
}