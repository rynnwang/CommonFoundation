using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Beyova.ApiTracking;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class Framework.
    /// </summary>
    public static class Framework
    {
        /// <summary>
        /// The resource hub
        /// </summary>
        internal static GlobalCultureResourceHub _resourceHub = new GlobalCultureResourceHub();

        /// <summary>
        /// The assembly version
        /// </summary>
        private readonly static Dictionary<string, object> _assemblyVersion = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the data security provider.
        /// </summary>
        /// <value>
        /// The data security provider.
        /// </value>
        internal static IDataSecurityProvider DataSecurityProvider { get; private set; } = null;

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
        /// Gets the primary SQL connection. It would try get configuration by name <c>SqlConnection</c> and <c>PrimarySqlConnection</c>.
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
                            _primarySqlConnection = GetConfiguration("SqlConnection").SafeToString(GetConfiguration("PrimarySqlConnection").SafeToString());
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
                result.MachineName = EnvironmentCore.ServerName;
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

        #region GlobalCultureResourceCollection

        /// <summary>
        /// Gets the resource by key.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="typeRequired">The type required.</param>
        /// <param name="languageCompatibility">The language compatibility.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public static string GetResourceString(string resourceKey, GlobalCultureResourceType? typeRequired = null, bool languageCompatibility = true)
        {
            return string.IsNullOrWhiteSpace(resourceKey) ? string.Empty : _resourceHub.GetResourceString(resourceKey, typeRequired, languageCompatibility: languageCompatibility);
        }

        /// <summary>
        /// Gets the enum resource string.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="languageCompatibility">if set to <c>true</c> [language compatibility].</param>
        /// <param name="forceMatchType">if set to <c>true</c> [force match type].</param>
        /// <returns></returns>
        public static string GetEnumResourceString<TEnum>(TEnum enumValue, bool languageCompatibility = true, bool forceMatchType = false)
           where TEnum : struct, IConvertible
        {
            return GetResourceString(string.Format("{0}_{1}",
                typeof(TEnum).Name,
                enumValue.ToInt64(null)),
                forceMatchType ? GlobalCultureResourceType.EnumValue as GlobalCultureResourceType? : null,
                languageCompatibility);
        }

        /// <summary>
        /// Gets the culture aspect json.
        /// </summary>
        /// <param name="cultureCode">The culture code.</param>
        /// <param name="languageCompatibility">if set to <c>true</c> [language compatibility].</param>
        /// <returns></returns>
        public static JToken GetCultureAspectJson(CultureInfo cultureCode, bool languageCompatibility = true)
        {
            return _resourceHub.GetJson(cultureCode, languageCompatibility);
        }

        /// <summary>
        /// Gets the global resource available culture information.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CultureInfo> GetGlobalResourceAvailableCultureInfo()
        {
            return _resourceHub.AvailableCultureInfo;
        }

        #endregion

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

        #endregion Public

        /// <summary>
        /// Initializes static members of the <see cref="Framework"/> class.
        /// </summary>
        static Framework()
        {
            InitializeByCustomAssemblyAttributes();
            ApiTracking?.LogApiMessage(new ApiMessage
            {
                Category = "Initialization",
                Message = string.Format("{0} is initialized.", EnvironmentCore.ProductName)
            });
        }

        #region Initialization

        /// <summary>
        /// Initializes the by custom assembly attributes.
        /// </summary>
        private static void InitializeByCustomAssemblyAttributes()
        {
            string currentAssemblyName = null;

            //Api track type need to stay on top and out of this. It is set in loop and be used after loop, to ensure required parameter from configuration is initialized.
            List<Type> apiTrackTypes = new List<Type>();

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
                            componentInfo = componentAttribute.UnderlyingObject;

                            // APITRACKING
                            if (ApiTracking == null)
                            {
                                apiTrackTypes.Add(componentInfo?.ApiTrackingType);
                            }
                        }

                        #endregion BeyovaComponentAttribute

                        #region DataSecurityAttribute

                        var dataSecurty = assembly.GetCustomAttribute<DataSecurityAttribute>();
                        if (dataSecurty != null)
                        {
                            if (DataSecurityProvider == null)
                            {
                                DataSecurityProvider = dataSecurty.DataSecurityProvider;
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
                                _resourceHub.DefaultCultureInfo = cultureResourceAttribute.UnderlyingObject.DefaultCultureCode.AsCultureInfo();
                            }

                            cultureResourceAttribute?.UnderlyingObject.FillResources(_resourceHub._cultureBasedResources);
                        }

                        #endregion BeyovaCultureResourceAttribute
                    }
                }

                if (ApiTracking == null && apiTrackTypes.HasItem())
                {
                    foreach (var one in apiTrackTypes)
                    {
                        var instance = one.CreateInstance() as IApiTracking;
                        if (instance != null)
                        {
                            ApiTracking = instance;
                            break;
                        }
                    }
                }

                // To check and ensure
                if (DataSecurityProvider == null)
                {
                    DataSecurityProvider = DefaultDataSecurityProvider.Instance;
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new
                {
                    currentAssemblyName
                });
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

        /// <summary>
        /// Initializes the default function injection.
        /// </summary>
        private static void InitializeDefaultFunctionInjection()
        {
            FunctionInjection<CultureInfo> injection = delegate ()
            {
                return _resourceHub?.DefaultCultureInfo;
            };

            CurrentCultureInfo = new PrioritizedFunctionInjection<CultureInfo>(injection);
        }

        /// <summary>
        /// Gets the get current operator credential.
        /// </summary>
        /// <value>
        /// The get current operator credential.
        /// </value>
        public static FunctionInjection<BaseCredential> GetCurrentOperatorCredential { get; private set; }

        /// <summary>
        /// Gets the current operator credential.
        /// </summary>
        /// <value>
        /// The current operator credential.
        /// </value>
        public static BaseCredential CurrentOperatorCredential
        {
            get
            {
                return GetCurrentOperatorCredential?.Invoke();
            }
        }

        /// <summary>
        /// Gets the get current culture information.
        /// </summary>
        /// <value>
        /// The get current culture information.
        /// </value>
        public static FunctionInjection<CultureInfo> GetCurrentCultureInfo { get; private set; }

        /// <summary>
        /// Gets the current culture information.
        /// </summary>
        /// <value>
        /// The current culture information.
        /// </value>
        public static PrioritizedFunctionInjection<CultureInfo> CurrentCultureInfo { get; private set; } = new PrioritizedFunctionInjection<CultureInfo>();

        #region ApplyInjection

        /// <summary>
        /// Applies the injection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="injectionCandidate">The injection candidate.</param>
        internal static void ApplyInjection<T>(string name, FunctionInjection<T> injectionCandidate)
        {
            if (!string.IsNullOrWhiteSpace(name) && injectionCandidate != null)
            {
                var hitProperty = typeof(Framework).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty).FirstOrDefault(x => x.Name == name);
                if (hitProperty != null)
                {
                    if (typeof(PrioritizedFunctionInjection<T>).IsAssignableFrom(hitProperty.PropertyType))
                    {
                        PrioritizedFunctionInjection<T> existed = hitProperty.GetValue(null) as PrioritizedFunctionInjection<T>;
                        if (existed == null)
                        {
                            existed = new PrioritizedFunctionInjection<T>();
                            hitProperty.SetValue(null, existed);
                        }

                        existed.Prepend(injectionCandidate);
                    }
                    if (hitProperty.PropertyType == typeof(FunctionInjection<>).MakeGenericType(typeof(T)))
                    {
                        hitProperty.SetValue(null, injectionCandidate);
                    }
                }
            }
        }

        #endregion
    }
}