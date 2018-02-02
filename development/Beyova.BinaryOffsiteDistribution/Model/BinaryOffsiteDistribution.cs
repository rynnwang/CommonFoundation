using System;

namespace Beyova
{
    /// <summary>
    /// Class BinaryOffsiteDistribution.
    /// </summary>
    public class BinaryOffsiteDistribution : BinaryOffsiteDistributionIdentifier
    {
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public BinaryStorageState State { get; set; }

        /// <summary>
        /// Gets or sets the upload expired stamp.
        /// </summary>
        /// <value>The upload expired stamp.</value>
        public DateTime? UploadExpiredStamp { get; set; }
    }
}