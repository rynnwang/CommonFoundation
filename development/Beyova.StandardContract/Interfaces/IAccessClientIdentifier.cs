namespace Beyova
{
    /// <summary>Interface IAccessClientIdentifier</summary>
    public interface IAccessClientIdentifier
    {
        /// <summary>Gets or sets Ip V4 Address.</summary>
        /// <value></value>
        System.String IpV4Address { get; set; }

        /// <summary>Gets or sets Ip V6 Address.</summary>
        /// <value></value>
        System.String IpV6Address { get; set; }

        /// <summary>Gets or sets User Agent.</summary>
        /// <value></value>
        System.String UserAgent { get; set; }

        /// <summary>Gets or sets Device Id.</summary>
        /// <value></value>
        System.String DeviceId { get; set; }

        /// <summary>Gets or sets Device Name.</summary>
        /// <value></value>
        System.String DeviceName { get; set; }

    }
}
