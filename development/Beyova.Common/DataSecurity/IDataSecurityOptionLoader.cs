using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataSecurityOptionLoader
    {
        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <returns></returns>
        IDataSecurityProvider GetProvider();
    }
}
