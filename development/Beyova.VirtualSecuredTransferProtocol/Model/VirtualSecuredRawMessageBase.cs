using System;

namespace Beyova.VirtualSecuredTransferProtocol
{
    /// <summary>
    /// </summary>
    public abstract class VirtualSecuredRawMessageBase
    {
        /// <summary>
        /// Gets or sets the schema version.
        /// </summary>
        /// <value>
        /// The schema version.
        /// </value>
        public int SchemaVersion { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets or sets the stamp.
        /// </summary>
        /// <value>The stamp.</value>
        public DateTime? Stamp { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualSecuredRawMessageBase"/> class.
        /// </summary>
        protected VirtualSecuredRawMessageBase()
        {
            Stamp = DateTime.UtcNow;
        }
    }
}