using System.Collections.Generic;
using System.IO;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public enum IntrastructureHealthStatus
    {
        /// <summary>
        /// The undefined
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// The healthy. Indicating it works well.
        /// </summary>
        Healthy = 1,
        /// <summary>
        /// The high load. Indicating it works well, but is busy.
        /// </summary>
        HighLoad = 2,
        /// <summary>
        /// The broken. Indicating it does not work, but can be located.
        /// </summary>
        Broken = 3,
        /// <summary>
        /// The missing. Indicating no idea about status, because it can not be located.
        /// </summary>
        Missing = 4,
        /// <summary>
        /// The under deployment. Indicating it is under deployment, so it would be back soon.
        /// </summary>
        UnderDeployment = 5,
        /// <summary>
        /// The out of service. Indicating it is out of service by purpose, by all possible reasons.
        /// </summary>
        OutOfService = 6
    }
}