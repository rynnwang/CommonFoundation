using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova
{
    partial class BaseSandbox
    {
        /// <summary>
        /// Creates the application domain.
        /// </summary>
        /// <param name="applicationDirectory">The application directory.</param>
        /// <returns></returns>
        protected AppDomain CreateAppDomain(string applicationDirectory)
        {
            return AppDomain.CreateDomain(this.Key.ToString(), AppDomain.CurrentDomain.Evidence, new AppDomainSetup
            {
                ApplicationBase = applicationDirectory,
                PrivateBinPath = applicationDirectory
            });
        }
    }
}
