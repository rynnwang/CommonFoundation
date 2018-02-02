using System.Collections.Generic;
using System;

namespace Beyova
{
    /// <summary>
    /// Class EnvironmentInfo.
    /// </summary>
    public class EnvironmentInfo : MachineHealth, IMachineHealth
    {
        private static DateTime _systemStartupTime = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the assembly version.
        /// </summary>
        /// <value>The assembly version.</value>
        public Dictionary<string, object> AssemblyVersion { get; set; }

        /// <summary>
        /// Gets or sets the configuration belongs.
        /// </summary>
        /// <value>The configuration belongs.</value>
        public Dictionary<string, string> ConfigurationBelongs { get; set; }

        /// <summary>
        /// Gets or sets the gc memory.
        /// </summary>
        /// <value>The gc memory.</value>
        public long? GCMemory { get; set; }

        /// <summary>
        /// Gets or sets the assembly hash.
        /// </summary>
        /// <value>The assembly hash.</value>
        public string AssemblyHash { get; set; }

        /// <summary>
        /// Gets the system startup time.
        /// </summary>
        /// <value>
        /// The system startup time.
        /// </value>
        public DateTime SystemStartupTime { get { return _systemStartupTime; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentInfo"/> class.
        /// </summary>
        public EnvironmentInfo()
        {
            this.AssemblyVersion = new Dictionary<string, object>();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}