using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGravityMember
    {
        /// <summary>
        /// Gets or sets the gravity member key.
        /// </summary>
        /// <value>
        /// The gravity member key.
        /// </value>
        Guid? GravityMemberKey { get; set; }
    }
}
