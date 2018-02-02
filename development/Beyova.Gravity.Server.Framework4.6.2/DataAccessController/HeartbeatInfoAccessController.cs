using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Beyova.Gravity.DataAccessController
{
    /// <summary>
    /// 
    /// </summary>
    public class HeartbeatInfoAccessController : GravityAccessController<HeartbeatInfo>
    {
        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>
        /// Object instance in type {`0}.
        /// </returns>
        protected override HeartbeatInfo ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new HeartbeatInfo
            {
                Key= sqlDataReader[column_Key].ObjectToGuid(),
                HostName = sqlDataReader[column_HostName].ObjectToString(),
                IpAddress = sqlDataReader[column_IpAddress].ObjectToString(),
                ClientKey = sqlDataReader[column_ClientKey].ObjectToGuid(),
                ConfigurationName= sqlDataReader[column_ConfigurationName].ObjectToString(),
                CpuUsage = sqlDataReader[column_CpuUsage].ObjectToNullableDouble(),
                MemoryUsage = sqlDataReader[column_MemoryUsage].ObjectToNullableInt64(),
                TotalMemory = sqlDataReader[column_TotalMemory].ObjectToNullableInt64(),
                CreatedStamp = sqlDataReader[column_CreatedStamp].ObjectToDateTime()
            };

            return result;
        }

        /// <summary>
        /// Saves the heartbeat information.
        /// </summary>
        /// <param name="heartbeat">The heartbeat.</param>
        /// <param name="productKey">The product key.</param>
        /// <returns>Client Key.</returns>
        public Guid? SaveHeartbeatInfo(Heartbeat heartbeat, Guid? productKey)
        {
            const string spName = "sp_SaveHeartbeat";

            try
            {
                heartbeat.CheckNullObject(nameof(heartbeat));
                productKey.CheckNullObject(nameof(productKey));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_ProductKey, productKey),
                    GenerateSqlSpParameter(column_HostName, heartbeat.HostName),
                    GenerateSqlSpParameter(column_ServerName, heartbeat.ServerName),
                    GenerateSqlSpParameter(column_IpAddress, heartbeat.IpAddress),
                    GenerateSqlSpParameter(column_ConfigurationName, heartbeat.ConfigurationName),
                    GenerateSqlSpParameter(column_CpuUsage, heartbeat.CpuUsage),
                    GenerateSqlSpParameter(column_MemoryUsage, heartbeat.MemoryUsage),
                    GenerateSqlSpParameter(column_TotalMemory, heartbeat.TotalMemory)
                };

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { heartbeat, productKey });
            }
        }

        /// <summary>
        /// Queries the heartbeat information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;HeartbeatInfo&gt;.</returns>
        public List<HeartbeatInfo> QueryHeartbeatInfo(HeartbeatCriteria criteria)
        {
            const string spName = "sp_QueryHeartbeat";

            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_ProductKey, criteria.ProductKey),
                    GenerateSqlSpParameter(column_ClientKey, criteria.ClientKey),
                    GenerateSqlSpParameter(column_FromStamp, criteria.FromStamp),
                    GenerateSqlSpParameter(column_ToStamp, criteria.ToStamp)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }
    }
}
