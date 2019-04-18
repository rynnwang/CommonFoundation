using System;

namespace Beyova.Diagnostic
{
    /// <summary>
    ///
    /// </summary>
    [Flags]
    public enum ApiTrackingType
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,
        /// <summary>
        /// The event
        /// </summary>
        Event = 1,
        /// <summary>
        /// The exception
        /// </summary>
        Exception = 2,
        /// <summary>
        /// The trace
        /// </summary>
        Trace = 4
    }
}