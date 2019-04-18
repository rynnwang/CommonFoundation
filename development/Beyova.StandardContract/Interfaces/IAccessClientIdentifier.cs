namespace Beyova
{
    /// <summary>Interface IAccessClientIdentifier</summary>
    public interface IAccessClientIdentifier
    {
        /// <summary>
        /// Gets or sets Ip V4 Address.
        /// </summary>
        /// <value>
        /// The ip v4 address.
        /// </value>
        System.String IpV4Address { get; set; }

        /// <summary>
        /// Gets or sets Ip V6 Address.
        /// </summary>
        /// <value>
        /// The ip v6 address.
        /// </value>
        System.String IpV6Address { get; set; }

        /// <summary>
        /// Gets or sets User Agent.
        /// </summary>
        /// <value>
        /// The user agent.
        /// </value>
        System.String UserAgent { get; set; }

        /// <summary>
        /// Gets or sets Device Id.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        System.String DeviceId { get; set; }

        /// <summary>
        /// Gets or sets Device Name.
        /// </summary>
        /// <value>
        /// The name of the device.
        /// </value>
        System.String DeviceName { get; set; }
    }
}