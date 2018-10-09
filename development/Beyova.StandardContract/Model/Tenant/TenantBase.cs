using System;

namespace Beyova.SaasPlatform
{
    /// <summary>
    /// Class RealmBase.
    /// </summary>
    public abstract class TenantBase : IIdentifier, IExpirable
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the RSA keys.
        /// </summary>
        /// <value>
        /// The RSA keys.
        /// </value>
        public RsaKeys RsaKeys { get; set; }

        /// <summary>
        /// Gets or sets the expired stamp.
        /// </summary>
        /// <value>The expired stamp.</value>
        public DateTime? ExpiredStamp { get; set; }
    }
}