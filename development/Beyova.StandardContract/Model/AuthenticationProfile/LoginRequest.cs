using System;

namespace Beyova
{
    /// <summary>
    /// Class LoginRequest.
    /// </summary>
    [Obsolete("Use AuthenticationRequest")]
    public class LoginRequest : AccessCredential
    {
        /// <summary>
        /// Gets or sets the security key. Value should be RSA encrypted by public key of service.
        /// </summary>
        /// <value>The security key.</value>
        public string SecurityKey { get; set; }
    }
}