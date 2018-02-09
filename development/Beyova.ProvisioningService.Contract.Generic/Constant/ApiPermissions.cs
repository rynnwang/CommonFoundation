using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;
using Beyova.SaasPlatform;

namespace Beyova.FunctionService.Generic
{
    partial class ApiPermissions
    {
        /// <summary>
        /// The preference administrator
        /// </summary>
        public const string PreferenceAdministrator = nameof(PreferenceAdministrator);

        /// <summary>
        /// The remote configuration administrator
        /// </summary>
        public const string RemoteConfigurationAdministrator = nameof(RemoteConfigurationAdministrator);

        /// <summary>
        /// 
        /// </summary>
        public static class ProvisioningSystem
        {
            /// <summary>
            /// The application version administration
            /// </summary>
            public const string AppVersionAdministration = nameof(AppVersionAdministration);

            /// <summary>
            /// The application platform administration
            /// </summary>
            public const string AppPlatformAdministration = nameof(AppPlatformAdministration);

            /// <summary>
            /// The application provisioning administration
            /// </summary>
            public const string AppProvisioningAdministration = nameof(AppProvisioningAdministration);
        }
    }
}