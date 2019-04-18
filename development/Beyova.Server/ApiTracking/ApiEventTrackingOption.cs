using System;

namespace Beyova.Diagnostic
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