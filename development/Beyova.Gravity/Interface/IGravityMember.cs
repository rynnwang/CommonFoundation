using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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
