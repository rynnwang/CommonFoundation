using System;
using System.Collections.Generic;

namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    public class ICPOptions : HashSet<ICPOption>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ICPOptions"/> class.
        /// </summary>
        public ICPOptions() : base(new LambdaEqualityComparer<ICPOption, string>(x => x.RootDomain, StringComparer.OrdinalIgnoreCase))
        {
        }

        /// <summary>
        /// Gets the icp option.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        public ICPOption GetICPOption(string host)
        {
            if (string.IsNullOrWhiteSpace(host) || host.IndexOf('.') < 0)
            {
                return null;
            }
            foreach (var item in this)
            {
                if (host.EndsWith("." + item.RootDomain))
                {
                    return item;
                }
            }

            return null;
        }
    }
}