using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Beyova.Gravity.DataAccessController
{
    /// <summary>
    /// Class RemoteConfigurationObjectAccessController.
    /// </summary>
    public class RemoteConfigurationObjectAccessController : GravityAccessController<RemoteConfigurationObject>
    {
        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>Object instance in type {`0}.</returns>
        protected override RemoteConfigurationObject ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new RemoteConfigurationObject
            {
                Key = sqlDataReader[column_Key].ObjectToGuid(),
                SnapshotKey = sqlDataReader[column_SnapshotKey].ObjectToGuid(),
                OwnerKey = sqlDataReader[column_ProductKey].ObjectToGuid(),
                Name = sqlDataReader[column_Name].ObjectToString(),
                Configuration = sqlDataReader[column_Configuration].ObjectToJToken()
            };

            return result;
        }

        /// <summary>
        /// Creates the or update remote configuration object.
        /// </summary>
        /// <param name="configurationObject">The configuration object.</param>
        /// <param name="operatorKey">The operator key.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CreateOrUpdateRemoteConfigurationObject(RemoteConfigurationObject configurationObject, Guid? operatorKey)
        {
            const string spName = "sp_CreateOrUpdateCentralConfiguration";

            try
            {
                configurationObject.CheckNullObject(nameof(configurationObject));
                operatorKey.CheckNullObject(nameof(operatorKey));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key, configurationObject.Key),
                    GenerateSqlSpParameter(column_ProductKey, configurationObject.OwnerKey),
                    GenerateSqlSpParameter(column_Name, configurationObject.Name),
                    GenerateSqlSpParameter(column_Configuration, configurationObject.Configuration),
                    GenerateSqlSpParameter(column_OperatorKey, operatorKey)
                };

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { configurationObject, operatorKey });
            }
        }

        /// <summary>
        /// Queries the remote configuration object.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;RemoteConfigurationObject&gt;.</returns>
        public List<RemoteConfigurationObject> QueryRemoteConfigurationObject(RemoteConfigurationCriteria criteria)
        {
            const string spName = "sp_QueryCentralConfiguration";

            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key, criteria.Key),
                    GenerateSqlSpParameter(column_ProductKey, criteria.OwnerKey),
                    GenerateSqlSpParameter(column_Name, criteria.Name)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Deletes the central configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="operatorKey">The operator key.</param>
        public void DeleteCentralConfiguration(Guid? key, Guid? operatorKey)
        {
            const string spName = "sp_DeleteCentralConfiguration";

            try
            {
                key.CheckNullObject(nameof(key));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key, key),
                    GenerateSqlSpParameter(column_OperatorKey, operatorKey)
                };

                this.ExecuteNonQuery(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { operatorKey, key });
            }
        }
    }
}
