using System;

namespace Beyova
{
    /// <summary>
    /// Interface ITenantable
    /// </summary>
    public interface ITenantable
    {
        /// <summary>
        /// Gets or sets Tenant Key.
        /// </summary>
        /// <value>
        /// The tenant key.
        /// </value>
        Guid? TenantKey { get; set; }
    }
}