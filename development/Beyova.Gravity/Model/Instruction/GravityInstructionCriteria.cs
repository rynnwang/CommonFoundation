using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityInstructionCriteria.
    /// </summary>
    public class GravityInstructionCriteria : GravityInstructionBase
    {
        /// <summary>
        /// Gets or sets from stamp.
        /// </summary>
        /// <value>
        /// From stamp.
        /// </value>
        public DateTime? FromStamp { get; set; }

        /// <summary>
        /// Gets or sets to stamp.
        /// </summary>
        /// <value>
        /// To stamp.
        /// </value>
        public DateTime? ToStamp { get; set; }
    }
}