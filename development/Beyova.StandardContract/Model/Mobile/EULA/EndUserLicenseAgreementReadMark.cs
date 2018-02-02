using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Beyova.SimpleBaseObject" />
    public class EndUserLicenseAgreementReadMark
    {
        /// <summary>
        /// Gets or sets the license key.
        /// </summary>
        /// <value>
        /// The license key.
        /// </value>
        public Guid? LicenseKey { get; set; }

        /// <summary>
        /// Gets or sets the platform key.
        /// </summary>
        /// <value>
        /// The platform key.
        /// </value>
        public Guid? PlatformKey { get; set; }

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