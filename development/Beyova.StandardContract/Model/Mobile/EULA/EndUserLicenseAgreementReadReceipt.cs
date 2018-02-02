using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Beyova.SimpleBaseObject" />
    public class EndUserLicenseAgreementReadReceipt : SimpleBaseObject
    {
        /// <summary>
        /// Gets or sets the license key.
        /// </summary>
        /// <value>
        /// The license key.
        /// </value>
        public Guid? LicenseKey { get; set; }

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

        /// <summary>
        /// Gets or sets the application version. (Optional)
        /// </summary>
        /// <value>
        /// The application version.
        /// </value>
        public string AppVersion { get; set; }

        /// <summary>
        /// Gets or sets the device identifier. (Optional)
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        public string DeviceId { get; set; }
    }
}