using System;

namespace Beyova.Diagnostic
{
    /// <summary>
    /// Interface IApiEventLog.
    /// </summary>
    public interface IApiEventLog : IApiEventLogBase, ICreatedStamp
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the entry stamp.
        /// </summary>
        /// <value>The entry stamp.</value>
        DateTime? EntryStamp { get; set; }

        /// <summary>
        /// Gets or sets the exit stamp.
        /// </summary>
        /// <value>The exit stamp.</value>
        DateTime? ExitStamp { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        long? ContentLength { get; set; }

        /// <summary>
        /// Gets or sets the geo information.
        /// </summary>
        /// <value>The geo information.</value>
        GeoInfoBase GeoInfo { get; set; }
    }
}