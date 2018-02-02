﻿namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticationRequest : AccessCredential
    {
        /// <summary>
        /// Gets or sets the security key.
        /// </summary>
        /// <value>
        /// The security key.
        /// </value>
        public string SecurityKey { get; set; }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>The platform.</value>
        public PlatformType? Platform { get; set; }

        /// <summary>
        /// Gets or sets the type of the device.
        /// </summary>
        /// <value>The type of the device.</value>
        public DeviceType? DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>The device identifier.</value>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        public string DeviceName { get; set; }

        /// <summary>
        /// Gets or sets the realm.
        /// </summary>
        /// <value>The realm.</value>
        public string Realm { get; set; }
    }
}