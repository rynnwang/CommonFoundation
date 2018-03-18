using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SqlConfigurationWriter
    {
        /// <summary>
        /// Gets the SQL connection.
        /// </summary>
        /// <value>
        /// The SQL connection.
        /// </value>
        public string SqlConnection { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConfigurationReader" /> class.
        /// </summary>
        /// <param name="sqlConnectionString">The SQL connection string.</param>
        public SqlConfigurationWriter(string sqlConnectionString)
        {
            this.SqlConnection = sqlConnectionString;
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

                TriggerReload();
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

                TriggerReload();
            }
            catch (Exception ex)
            {
                throw ex.Handle(key);
            }
        }

        /// <summary>
        /// Triggers the reload.
        /// </summary>
        private void TriggerReload()
        {
            SqlConfigurationReader reader = null;
            if (!string.IsNullOrWhiteSpace(this.SqlConnection) && SqlConfigurationReader._instances.TryGetValue(this.SqlConnection, out reader))
            {
                reader.Reload();
            }
        }
    }
}
