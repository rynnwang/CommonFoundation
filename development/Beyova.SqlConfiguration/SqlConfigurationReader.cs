using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public sealed class SqlConfigurationReader : BaseJsonConfigurationReader
    {
        /// <summary>
        /// The instances
        /// </summary>
        internal static Dictionary<string, SqlConfigurationReader> _instances = new Dictionary<string, SqlConfigurationReader>();

        /// <summary>
        /// Gets the SQL connection string.
        /// </summary>
        /// <value>
        /// The SQL connection string.
        /// </value>
        public string SqlConnectionString { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConfigurationReader" /> class.
        /// </summary>
        /// <param name="sourceAssembly">The source assembly.</param>
        /// <param name="coreComponentVersion">The core component version.</param>
        /// <param name="sqlConnectionString">The SQL connection string.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        internal SqlConfigurationReader(string sourceAssembly, string coreComponentVersion, string sqlConnectionString, bool throwException = false)
            : base(sourceAssembly, coreComponentVersion, nameof(SqlConfigurationReader), throwException)
        {
            this.SqlConnectionString = sqlConnectionString;
            if (!string.IsNullOrWhiteSpace(sqlConnectionString))
            {
                _instances.Merge(sqlConnectionString, this, false);
            }
        }

        /// <summary>
        /// Initializes the specified throw exception.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        protected override Dictionary<string, RuntimeConfigurationItem> Initialize(bool throwException = false)
        {
            Dictionary<string, RuntimeConfigurationItem> result = new Dictionary<string, RuntimeConfigurationItem>();

            try
            {
                this.SqlConnectionString.CheckEmptyString(nameof(this.SqlConnectionString));

                using (var controller = new ConfigurationRawItemAccessController(this.SqlConnectionString))
                {
                    var rawConfigurations = controller.GetSystemConfigurations();

                    foreach (var item in rawConfigurations)
                    {
                        var runtimeConfigurationItem = RuntimeConfigurationItem.FromRaw(this.SourceAssembly, nameof(SqlConfigurationReader), CoreComponentVersion, item);
                        result.AddIfNotNull(runtimeConfigurationItem?.Key, runtimeConfigurationItem);
                    }
                }
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle();
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void UpdateConfiguration<T>(string key, T value)
        {
            try
            {
                key.CheckEmptyString(nameof(key));

                using (var controller = string.IsNullOrWhiteSpace(this.SqlConnection) ? new ConfigurationRawItemAccessController() : new ConfigurationRawItemAccessController(this.SqlConnection))
                {
                    var rawItem = new ConfigurationRawItem
                    {
                        Key = key,
                        Type = typeof(T).GetFullName(),
                        Value = (typeof(T) == typeof(string) ? value.ToString() : value.ToJson(false))
                    };

                    controller.UpdateSystemConfiguration(rawItem);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Deletes the configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        public void DeleteConfiguration(string key)
        {
            try
            {
                key.CheckEmptyString(nameof(key));

                using (var controller = string.IsNullOrWhiteSpace(this.SqlConnection) ? new ConfigurationRawItemAccessController() : new ConfigurationRawItemAccessController(this.SqlConnection))
                {
                    var rawItem = new ConfigurationRawItem { };

                    controller.DeleteSystemConfiguration(key);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(key);
            }
        }
    }
}