using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova.Web
{
    /// <summary>
    /// 
    /// </summary>
    public enum ActionResultType
    {
        /// <summary>
        /// The default
        /// </summary>
        Default = 0,
        /// <summary>
        /// The view
        /// </summary>
        View = 1,
        /// <summary>
        /// The partial view
        /// </summary>
        PartialView = 2,
        /// <summary>
        /// The json
        /// </summary>
        Json = 3,
        /// <summary>
        /// The redirection
        /// </summary>
        Redirection = 4
    }
}
