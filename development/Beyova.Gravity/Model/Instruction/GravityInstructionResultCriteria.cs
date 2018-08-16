using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityInstructionResultCriteria.
    /// </summary>
    public class GravityInstructionResultCriteria : GravityInstructionResultBase, IGravityMember
    {
        /// <summary>
        /// Gets or sets the gravity member key.
        /// </summary>
        /// <value>
        /// The gravity member key.
        /// </value>
        public Guid? GravityMemberKey { get; set; }
    }
}