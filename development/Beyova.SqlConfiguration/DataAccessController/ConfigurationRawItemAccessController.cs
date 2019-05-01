using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    internal class ConfigurationRawItemAccessController : BaseDataAccessController<ConfigurationRawItem>
    {
        #region Constants

        /// <summary>
        /// The column minimum component version required
        /// </summary>
        public const string column_MinComponentVersionRequired = "MinComponentVersionRequired";

        /// <summary>
        /// The column maximum component version limited
        /// </summary>
        public const string column_MaxComponentVersionLimited = "MaxComponentVersionLimited";

        #endregion Constants

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRawItemAccessController"/> class.
        /// </summary>
        public ConfigurationRawItemAccessController()
            : this(Framework.PrimarySqlConnection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRawItemAccessController"/> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        public ConfigurationRawItemAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
            : base(primarySqlConnectionString, readOnlySqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRawItemAccessController"/> class.
        /// </summary>
        /// <param name="primarySqlConnection">The SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        public ConfigurationRawItemAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base(primarySqlConnection, readOnlySqlConnection)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>
        /// Object instance in type {`0}.
        /// </returns>
        protected override ConfigurationRawItem ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            return new ConfigurationRawItem
            {
                Key = sqlDataReader[column_Key].ObjectToString(),
                Type = sqlDataReader[column_Type].ObjectToString(),
                MinComponentVersionRequired = sqlDataReader[column_MinComponentVersionRequired].ObjectToString(),
                MaxComponentVersionLimited = sqlDataReader[column_MaxComponentVersionLimited].ObjectToString(),
                Value = Framework.DataSecurityProvider.DecryptFromString<string>(sqlDataReader[column_Value].ObjectToString())
            };
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public ConfigurationRawItem GetSystemConfiguration(string key)
        {
            try
            {
                key.CheckEmptyString(nameof(key));

                return InternalGetSystemConfigurations(key).SafeFirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle(key);
            }
        }

        /// <summary>
        /// Deletes the system configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        public void DeleteSystemConfiguration(string key)
        {
            const string spName = "sp_DeleteSystemConfiguration";

            try
            {
                key.CheckEmptyString(nameof(key));
                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key, key)
                };

                ExecuteNonQuery(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(key);
            }
        }

        /// <summary>
        /// Gets the configurations.
        /// </summary>
        /// <returns></returns>
        public List<ConfigurationRawItem> GetSystemConfigurations()
        {
            return InternalGetSystemConfigurations(null);
        }

        /// <summary>
        /// Gets the system configurations.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected List<ConfigurationRawItem> InternalGetSystemConfigurations(string key)
        {
            const string spName = "sp_GetSystemConfiguration";

            try
            {
                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key, key)
                };

                return ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(key);
            }
        }

        /// <summary>
        /// Updates the system configuration.
        /// </summary>
        /// <param name="item">The item.</param>
        public void UpdateSystemConfiguration(ConfigurationRawItem item)
        {
            const string spName = "sp_UpdateSystemConfiguration";

            try
            {
                item.CheckNullObject(nameof(item));
                item.Key.CheckEmptyString(nameof(item.Key));
                item.Type.CheckEmptyString(nameof(item.Type));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key, item.Key),
                    GenerateSqlSpParameter(column_Type, item.Type),
                    GenerateSqlSpParameter(column_MinComponentVersionRequired, item.MinComponentVersionRequired),
                    GenerateSqlSpParameter(column_MaxComponentVersionLimited, item.MaxComponentVersionLimited),
                    GenerateSqlSpParameter(column_Value, Framework.DataSecurityProvider.EncryptObjectAsString(item.Value)),
                };

                ExecuteNonQuery(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(item);
            }
        }
    }
}