using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// Provides access to configuration files for client applications. Reader would try read AppConfig.JSON first, then read assembly name based JSON ({AssemblyName}.JSON) to override, based on dependency order.
    /// </summary>
    public class JsonConfigurationReader : BaseJsonConfigurationReader
    {
        /// <summary>
        /// The configuration file extension
        /// </summary>
        public const string configurationFileExtension = ".json";

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public BeyovaLocalConfigurationOptions Options { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigurationReader" /> class.
        /// </summary>
        /// <param name="sourceAssembly">The source assembly.</param>
        /// <param name="coreVersion">The core version.</param>
        /// <param name="readerType">Type of the reader.</param>
        /// <param name="options">The options.</param>
        /// <param name="dataSecurityProvider">The data security provider.</param>
        internal JsonConfigurationReader(string sourceAssembly, string coreVersion, string readerType, BeyovaLocalConfigurationOptions options, IDataSecurityProvider dataSecurityProvider = null)
            : base(sourceAssembly, coreVersion, readerType, false)
        {
            Options = options;
        }

        #region Initialization

        /// <summary>
        /// Initializes the by json manifest.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="manifest">The manifest.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        protected void InitializeByJsonManifest(IDictionary<string, RuntimeConfigurationItem> container, JsonFileConfigurationManifest manifest, bool throwException = false)
        {
            try
            {
                container.Clear();

                manifest.CheckNullObject(nameof(manifest));

                var name = manifest.Name.SafeToString(manifest.Version);
                foreach (JProperty one in manifest.Configurations.Children())
                {
                    var localRaw = one.Value.ToObject<LocalConfigurationRawItem>();
                    localRaw.Key = one.Name;

                    var rawItem = new ConfigurationRawItem(localRaw)
                    {
                        Value = (localRaw.Encrypted ?? false) ?
                        Framework.DataSecurityProvider.DecryptFromString<string>(localRaw.Value.ToObject<string>()) :
                        (localRaw.Value.Type == JTokenType.String ? localRaw.Value.ToObject<string>() : localRaw.Value.ToString())
                    };

                    FillObjectCollection(container, CoreComponentVersion, rawItem, SourceAssembly, ReaderType, throwException);
                }
            }
            catch (Exception ex)
            {
                var exception = ex.Handle(new { manifest });
                if (throwException)
                {
                    throw exception;
                }
            }
        }

        /// <summary>
        /// Initializes the by options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        protected Dictionary<string, RuntimeConfigurationItem> InitializeByOptions(BeyovaLocalConfigurationOptions options, bool throwException = false)
        {
            try
            {
                var result = new Dictionary<string, RuntimeConfigurationItem>();

                if (options != null)
                {
                    var settingContainer = new Dictionary<string, RuntimeConfigurationItem>();
                    var configurationPath = options.GetConfigurationFullPath();

                    if (!string.IsNullOrWhiteSpace(configurationPath))
                    {
                        string jsonString = string.Empty;

                        if (File.Exists(configurationPath))
                        {
                            jsonString = File.ReadAllText(configurationPath, Encoding.UTF8);
                        }

                        if (!string.IsNullOrWhiteSpace(jsonString))
                        {
                            InitializeByJsonManifest(result, jsonString.TryConvertJsonToObject<JsonFileConfigurationManifest>(), throwException);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle(new { options });
                }
                else
                {
                    return new Dictionary<string, RuntimeConfigurationItem>();
                }
            }
        }

        /// <summary>
        /// Initializes the specified throw exception.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>Dictionary&lt;System.String, ConfigurationItem&gt;.</returns>
        protected override Dictionary<string, RuntimeConfigurationItem> Initialize(bool throwException = false)
        {
            return InitializeByOptions(Options);
        }

        #endregion Initialization
    }
}