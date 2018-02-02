namespace Beyova
{
    /// <summary>
    /// Class MachineHealth.
    /// </summary>
    public class MachineHealth : IMachineHealth
    {
        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        /// <value>The name of the server.</value>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>The name of the host.</value>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public string IpAddress { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MachineHealth"/> class.
        /// </summary>
        public MachineHealth()
        {
        }
    }
}