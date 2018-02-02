using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Enum ApiEventTrackingOption
    /// </summary>
    [Flags]
    public enum ApiEventTrackingOption
    {
        /// <summary>
        /// The standard
        /// </summary>
        Standard = 0,
        /// <summary>
        /// The request raw
        /// </summary>
        RequestRaw = 1,
        /// <summary>
        /// The response raw
        /// </summary>
        ResponseRaw = 2
    }
}
