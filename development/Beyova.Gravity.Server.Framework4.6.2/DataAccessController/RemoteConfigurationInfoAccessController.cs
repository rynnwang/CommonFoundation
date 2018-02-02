using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Beyova.Gravity.DataAccessController
{
    /// <summary>
    /// Class RemoteConfigurationInfoAccessController.
    /// </summary>
    public class RemoteConfigurationInfoAccessController : GravityAccessController<RemoteConfigurationInfo>
    {
        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>Object instance in type {`0}.</returns>
        protected override RemoteConfigurationInfo ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new RemoteConfigurationInfo
            {
                Key = sqlDataReader[column_Key].ObjectToGuid(),
                SnapshotKey = sqlDataReader[column_SnapshotKey].ObjectToGuid(),
                OwnerKey = sqlDataReader[column_ProductKey].ObjectToGuid(),
                Name = sqlDataReader[column_Name].ObjectToString(),
                Configuration = sqlDataReader[column_Configuration].ObjectToJToken()
            };

            FillBaseObjectFields(result, sqlDataReader);

            return result;
        }
    
        /// <summary>
        /// Queries the central configuration snapshot.
        /// </summary>
        /// <param name="configurationKey">The configuration key.</param>
        /// <param name="snapshotKey">The snapshot key.</param>
        /// <returns>List&lt;RemoteConfigurationObject&gt;.</returns>
        public List<RemoteConfigurationInfo> QueryCentralConfigurationSnapshot(Guid? configurationKey, Guid? snapshotKey)
        {
            const string spName = "sp_QueryCentralConfigurationSnapshot";

            try
            {
                if (!(configurationKey.HasValue || snapshotKey.HasValue))
                {
                    throw ExceptionFactory.CreateInvalidObjectException("configurationKey or snapshotKey");
                }

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key, configurationKey),
                    GenerateSqlSpParameter(column_SnapshotKey, snapshotKey)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { configurationKey, snapshotKey });
            }
        }
    }
}
