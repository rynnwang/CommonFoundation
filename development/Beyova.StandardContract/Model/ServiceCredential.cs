using Newtonsoft.Json;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class ServiceCredential.
    /// </summary>
    public class ServiceCredential : BaseCredential, IPermissionIdentifiers
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        [JsonProperty("permissions")]
        public HashSet<string> Permissions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCredential"/> class.
        /// </summary>
        public ServiceCredential()
        {
            Permissions = new HashSet<string>();
        }
    }
}