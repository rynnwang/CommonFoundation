using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public interface INameObject
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        string Description { get; set; }
    }
}
