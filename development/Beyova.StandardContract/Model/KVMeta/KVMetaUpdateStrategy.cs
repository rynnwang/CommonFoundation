using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public enum KVMetaUpdateStrategy
    {
        /// <summary>
        /// The default
        /// </summary>
        Default = 0,
        /// <summary>
        /// The overwrite
        /// </summary>
        Overwrite = 1,
        /// <summary>
        /// The merge all
        /// </summary>
        MergeAll = 2,
        /// <summary>
        /// The merge new
        /// </summary>
        MergeNew = 3,
        /// <summary>
        /// The clear
        /// </summary>
        Clear = 4
    }
}