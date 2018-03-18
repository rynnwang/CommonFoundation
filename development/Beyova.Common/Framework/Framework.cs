﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// Class Framework.
    /// </summary>
    public static class Framework
    {
        /// <summary>
        /// The global culture resource collection
        /// </summary>
        internal static GlobalCultureResourceCollection GlobalCultureResourceCollection = new GlobalCultureResourceCollection();

        /// <summary>
        /// The assembly version
        /// </summary>
        private readonly static Dictionary<string, object> _assemblyVersion = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The data security provider
        /// </summary>
        private static IDataSecurityProvider _dataSecurityProvider = null;

        /// <summary>
        /// Gets the data security provider.
        /// </summary>
        /// <value>
        /// The data security provider.
        /// </value>
        internal static IDataSecurityProvider DataSecurityProvider { get { return _dataSecurityProvider; } }

        /// <summary>
        /// The primary SQL connection
        /// </summary>
        private static string _primarySqlConnection = null;

        /// <summary>
        /// The primary SQL connection locker
        /// </summary>
        private static object _primarySqlConnectionLocker = new object();

        #region Public

        /// <summary>
        /// Gets the primary SQL connection.
        /// </summary>
        /// <value>
        /// The primary SQL connection.
        /// </value>
        public static string PrimarySqlConnection
        {
            get
            {
                if (_primarySqlConnection == null)
                {
                    lock (_primarySqlConnectionLocker)
                    {
                        if (_primarySqlConnection == null)
                        {
                            _primarySqlConnection = GetConfiguration("SqlConnection").SafeToString(
                                GetConfiguration("PrimarySqlConnection").SafeToString(
                                    System.Configuration.ConfigurationManager.ConnectionStrings.HasItem() ? System.Configuration.ConfigurationManager.ConnectionStrings[0].ConnectionString : string.Empty
                                ));
                        }
                    }
                }

                return _primarySqlConnection;
            }
        }

        /// <summary>
        /// Sets the global default api tracking.
        /// </summary>
        /// <param name="apiTracking">The API tracking.</param>
        [Obsolete("Use BeyovaComponent Attribute in assembly to set default ApiTracking instance.")]
        public static void SetGlobalDefaultApiTracking(IApiTracking apiTracking)
        {
            if (apiTracking != null)
            {
                ApiTracking = apiTracking;
            }
        }

        /// <summary>
        /// Abouts the service.
        /// </summary>
        /// <returns>ServiceVersion.</returns>
        public static EnvironmentInfo AboutService()
        {
            try
            {
                var result = new EnvironmentInfo { AssemblyVersion = _assemblyVersion };

                result.GCMemory = SystemManagementExtension.GetGCMemory();
                result.ServerName = EnvironmentCore.ServerName;
                result.IpAddress = EnvironmentCore.LocalMachineIpAddress;
                result.HostName = EnvironmentCore.LocalMachineHostName;
                result.AssemblyHash = EnvironmentCore.GetAssemblyHash();

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Gets the resource by key.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="languageCompatibility">The language compatibility.</param>
        /// <returns>System.String.</returns>
        public static string GetResourceString(string resourceKey, bool languageCompatibility = true)
        {
            return string.IsNullOrWhiteSpace(resourceKey) ? string.Empty : GlobalCultureResourceCollection.GetResourceString(resourceKey);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// T.
        /// </returns>
        public static T GetConfiguration<T>(string key, T defaultValue = default(T))
        {
            return ConfigurationHub.GetConfiguration<T>(key, defaultValue);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.String.</returns>
        public static object GetConfiguration(string key, object defaultValue = null)
        {
            return ConfigurationHub.GetConfiguration(key, defaultValue);
        }

        /// <summary>
        /// Gets the configuration setting count.
        /// </summary>
        /// <value>The configuration setting count.</value>
        public static int ConfigurationSettingCount
        {
            get
            {
                return ConfigurationHub.ConfigurationSettingCount;
            }
        }

        /// <summary>
        /// Gets the configuration values.
        /// </summary>
        /// <value>
        /// The configuration values.
        /// </value>
        public static Dictionary<string, RuntimeConfigurationItem> ConfigurationValues
        {
            get
            {
                return ConfigurationHub.ConfigurationItems;
            }
        }

        /// <summary>
        /// The API tracking
        /// </summary>
        public static IApiTracking ApiTracking { get; private set; }

        /// <summary>
        /// Gets the current culture information.
        /// </summary>
        /// <value>The current culture information.</value>
        public static CultureInfo CurrentCultureInfo
        {
            get
            {
                return ContextHelper.CurrentCultureInfo ?? GlobalCultureResourceCollection?.DefaultCultureInfo;
            }
        }

        #endregion Public

        /// <summary>
        /// Initializes static members of the <see cref="Framework"/> class.
        /// </summary>
        static Framework()
        {
            InitializeByCustomAssemblyAttributes();
            ApiTracking?.LogMessage(string.Format("{0} is initialized.", EnvironmentCore.ProductName));
        }

        #region Initialization

        /// <summary>
        /// Initializes the by custom assembly attributes.
        /// </summary>
        private static void InitializeByCustomAssemblyAttributes()
        {
            string currentAssemblyName = null;

            try
            {
                foreach (var assembly in EnvironmentCore.DescendingAssemblyDependencyChain)
                {
                    BeyovaComponentInfo componentInfo = null;
                    var assemblyName = assembly.GetName();
                    currentAssemblyName = assemblyName.Name;

                    if (!assemblyName.IsSystemAssembly())
                    {
                        #region BeyovaComponentAttribute

                        var componentAttribute = assembly.GetCustomAttribute<BeyovaComponentAttribute>();
                        if (componentAttribute != null)
                        {
                            // APITRACKING
                            if (ApiTracking == null)
                            {
                                ApiTracking = componentAttribute.UnderlyingObject.GetApiTrackingInstance();
                            }

                            //Version
                            componentInfo = componentAttribute.UnderlyingObject;
                        }

                        #endregion BeyovaComponentAttribute

                        #region DataSecurityAttribute

                        var dataSecurty = assembly.GetCustomAttribute<DataSecurityAttribute>();
                        if (dataSecurty != null)
                        {
                            if (_dataSecurityProvider == null)
                            {
                                _dataSecurityProvider = dataSecurty.DataSecurityProvider;
                            }
                        }

                        #endregion DataSecurityAttribute

                        #region BeyovaConfigurationLoaderAttribute

                        var configurationLoaders = assembly.GetCustomAttributes<BeyovaConfigurationLoaderAttribute>();
                        if (configurationLoaders.HasItem())
                        {
                            foreach (var one in configurationLoaders)
                            {
                                ConfigurationHub.RegisterConfigurationReader(one?.Loader?.GetReader(currentAssemblyName, componentInfo?.Version));
                            }
                        }
                        else
                        {
                            // To be obsoleted
                            var configurationAttribute = assembly.GetCustomAttribute<BeyovaConfigurationAttribute>();
                            if (configurationAttribute != null)
                            {
                                ConfigurationHub.RegisterConfigurationReader(new JsonConfigurationReader(currentAssemblyName, componentInfo?.Version, nameof(JsonConfigurationReader), configurationAttribute.Options));
                            }
                        }

                        #endregion BeyovaConfigurationLoaderAttribute

                        #region BeyovaCultureResourceAttribute

                        var cultureResourceAttribute = assembly.GetCustomAttribute<BeyovaCultureResourceAttribute>();
                        if (cultureResourceAttribute != null)
                        {
                            if (!string.IsNullOrWhiteSpace(cultureResourceAttribute.UnderlyingObject.DefaultCultureCode))
                            {
                                GlobalCultureResourceCollection.DefaultCultureInfo = cultureResourceAttribute.UnderlyingObject.DefaultCultureCode.AsCultureInfo();
                            }

                            cultureResourceAttribute?.UnderlyingObject.FillResources(GlobalCultureResourceCollection.cultureBasedResources);
                        }

                        #endregion BeyovaCultureResourceAttribute
                    }
                }

                // To check and ensure
                if (_dataSecurityProvider == null)
                {
                    _dataSecurityProvider = DefaultDataSecurityProvider.Instance;
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { currentAssemblyName });
            }
        }

        #endregion Initialization

        /// <summary>
        /// Gets the default text encoding.
        /// </summary>
        /// <value>
        /// The default text encoding.
        /// </value>
        public static Encoding DefaultTextEncoding { get { return Encoding.UTF8; } }
    }
}