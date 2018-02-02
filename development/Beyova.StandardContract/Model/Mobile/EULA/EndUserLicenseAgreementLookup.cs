using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Beyova.SimpleBaseObject" />
    public class EndUserLicenseAgreementLookup
    {
        /// <summary>
        /// Gets or sets the license key.
        /// </summary>
        /// <value>
        /// The license key.
        /// </value>
        public Guid? LicenseKey { get; set; }

        /// <summary>
        /// Gets or sets the latest license key.
        /// </summary>
        /// <value>
        /// The latest license key.
        /// </value>
        public Guid? LatestLicenseKey { get; set; }

        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>
        /// The user key.
        /// </value>
        public Guid? UserKey { get; set; }

        /// <summary>
        /// Gets or sets the product code.
        /// </summary>
        /// <value>
        /// The product code.
        /// </value>
        public string ProductCode { get; set; }
    }
}