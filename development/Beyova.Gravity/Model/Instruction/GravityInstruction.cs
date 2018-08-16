using System;
using Newtonsoft.Json.Linq;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityInstruction.
    /// </summary>
    public class GravityInstruction : GravityInstructionBase
    {
        /// <summary>
        /// Gets or sets the expired stamp.
        /// </summary>
        /// <value>
        /// The expired stamp.
        /// </value>
        public DateTime? ExpiredStamp { get; set; }
    }
}