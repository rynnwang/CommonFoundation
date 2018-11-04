using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Beyova.ApiTracking;
using Beyova.Cache;
using Beyova.ExceptionSystem;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// 
    /// </summary>
    public static class RestApiSettingPool
    {
        /// <summary>
        /// The default setting name
        /// </summary>
        const string defaultSettingName = "default";

        /// <summary>
        /// The default rest API settings
        /// </summary>
        private static RestApiSettings defaultRestApiSettings = new RestApiSettings
        {
            TokenHeaderKey = HttpConstants.HttpHeader.TOKEN,
            ClientIdentifierHeaderKey = HttpConstants.HttpHeader.CLIENTIDENTIFIER,
            EnableContentCompression = true,
            Name = defaultSettingName,
            ApiTracking = Framework.ApiTracking
        };

        /// <summary>
        /// Gets the default rest API settings.
        /// </summary>
        /// <value>
        /// The default rest API settings.
        /// </value>
        public static RestApiSettings DefaultRestApiSettings
        {
            get
            {
                return defaultRestApiSettings;
            }
            private set
            {
                if (value != null)
                {
                    defaultRestApiSettings = value;
                }
            }
        }

        /// <summary>
        /// The settings container
        /// </summary>
        internal static Dictionary<string, RestApiSettings> settingsContainer = new Dictionary<string, RestApiSettings>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Adds the setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="overrideIfExists">if set to <c>true</c> [override if exists].</param>
        /// <returns></returns>
        public static bool AddSetting(RestApiSettings setting, bool overrideIfExists = false)
        {
            return (setting != null) ? settingsContainer.Merge(setting.Name.SafeToString(), setting, overrideIfExists) : false;
        }

        /// <summary>
        /// Gets the name of the rest API setting by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="useDefaultIfNotFound">The use default if not found.</param>
        /// <returns>Beyova.RestApi.RestApiSettings.</returns>
        public static RestApiSettings GetRestApiSettingByName(string name, bool useDefaultIfNotFound = true)
        {
            RestApiSettings setting;
            return settingsContainer.TryGetValue(name.SafeToString(), out setting) ? setting : (useDefaultIfNotFound ? settingsContainer[defaultSettingName] : null);
        }

        /// <summary>
        /// Initializes the <see cref="RestApiSettingPool"/> class.
        /// </summary>
        static RestApiSettingPool()
        {
            settingsContainer.Add(defaultRestApiSettings.Name, defaultRestApiSettings);
        }
    }
}