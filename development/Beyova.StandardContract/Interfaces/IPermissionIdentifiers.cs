using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Beyova
{
    /// <summary>
    /// Interface IPermissionIdentifiers
    /// </summary>
    public interface IPermissionIdentifiers
    {
        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        List<string> Permissions
        {
            get; set;
        }
    }
}