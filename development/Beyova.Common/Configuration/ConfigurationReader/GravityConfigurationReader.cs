using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Beyova.Gravity;

namespace Beyova
{
    /// <summary>
    /// Provides access to configuration files for client applications. Reader would try read AppConfig.JSON first, then read assembly name based JSON ({AssemblyName}.JSON) to override, based on dependency order.
    /// </summary>
    internal class GravityConfigurationReader : BaseJsonConfigurationReader
    {
        /// <summary>
        /// The _gravity client
        /// </summary>
        private GravityClient _gravityClient;

        /// <summary>
        /// Gets or sets the name of the configuration.
        /// </summary>
        /// <value>The name of the configuration.</value>
        public string ConfigurationName { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigurationReader" /> class.
        /// </summary>
        /// <param name="gravityClient">The gravity client.</param>
        /// <param name="sourceAssembly">The source assembly.</param>
        /// <param name="coreComponentVersion">The core component version.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        internal GravityConfigurationReader(GravityClient gravityClient, string sourceAssembly, string coreComponentVersion, string configurationName, bool throwException = false)
            : base(sourceAssembly, coreComponentVersion, nameof(GravityConfigurationReader), throwException)
        {
            _gravityClient = gravityClient;
            ConfigurationName = configurationName;
        }

        #region Initialization

        /// <summary>
        /// Initializes the specified throw exception.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>Dictionary&lt;System.String, ConfigurationItem&gt;.</returns>
        protected override Dictionary<string, RuntimeConfigurationItem> Initialize(bool throwException = false)
        {
            var componentAttribute = GravityShell.Host?.ComponentAttribute;
            Dictionary<string, RuntimeConfigurationItem> container = null;

            try
            {
                var remoteConfigurationList = _gravityClient.RetrieveConfiguration(this.ConfigurationName)?.Configuration?.ToObject<List<ConfigurationRawItem>>();

                if (remoteConfigurationList.HasItem())
                {
                    foreach (var one in remoteConfigurationList)
                    {
                        FillObjectCollection(container, this.CoreComponentVersion, one, this.SourceAssembly, this.ReaderType, throwException);
                    }

                    if (container.HasItem())
                    {
                        SaveConfigurationBackup(container);
                    }
                }

                return container;
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle(new
                    {
                        componentAttribute = componentAttribute?.UnderlyingObject.Id
                    });
                }

                container = RestoreBackup() ?? new Dictionary<string, RuntimeConfigurationItem>();
            }

            return container;
        }

        #endregion Initialization

        #region Backup

        /// <summary>
        /// The backup file name
        /// </summary>
        private const string backupFileName = "config.bak";

        /// <summary>
        /// The processing backup file name
        /// </summary>
        private const string processingBackupFileName = "config.bak.processing";

        /// <summary>
        /// Saves the configuration backup.
        /// </summary>
        /// <param name="configurations">The configurations.</param>
        protected void SaveConfigurationBackup(Dictionary<string, RuntimeConfigurationItem> configurations)
        {
            if (configurations != null)
            {
                try
                {
                    var tmpPath = Path.Combine(EnvironmentCore.ApplicationBaseDirectory, processingBackupFileName);
                    var rawContent = Encoding.UTF8.GetBytes(configurations.ToJson());
                    File.WriteAllBytes(tmpPath, rawContent.EncryptR3DES());

                    var filePath = Path.Combine(EnvironmentCore.ApplicationBaseDirectory, backupFileName);
                    File.Copy(tmpPath, filePath, true);
                    File.Delete(tmpPath);
                }
                catch { }
            }
        }

        /// <summary>
        /// Restores the backup.
        /// </summary>
        /// <returns>System.Collections.Generic.Dictionary&lt;System.String, Beyova.Configuration.BaseJsonConfigurationReader.ConfigurationItem&gt;.</returns>
        protected Dictionary<string, RuntimeConfigurationItem> RestoreBackup()
        {
            try
            {
                var filePath = Path.Combine(EnvironmentCore.ApplicationBaseDirectory, backupFileName);
                if (File.Exists(filePath))
                {
                    var raw = File.ReadAllBytes(filePath);
                    var jsonString = Encoding.UTF8.GetString(raw.DecryptR3DES());
                    return jsonString.TryConvertJsonToObject<Dictionary<string, RuntimeConfigurationItem>>();
                }
            }
            catch { }

            return null;
        }

        #endregion Backup
    }
}