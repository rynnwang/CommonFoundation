using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.Gravity.DataAccessController
{
    /// <summary>
    /// Class ProductClientAccessController.
    /// </summary>
    public class ProductClientAccessController : GravityAccessController<ProductClient>
    {
        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>Object instance in type {`0}.</returns>
        protected override ProductClient ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new ProductClient
            {
                HostName = sqlDataReader[column_HostName].ObjectToString(),
                ServerName = sqlDataReader[column_ServerName].ObjectToString(),
                IpAddress = sqlDataReader[column_IpAddress].ObjectToString(),
                ProductKey = sqlDataReader[column_ProductKey].ObjectToGuid(),
                ConfigurationName = sqlDataReader[column_ConfigurationName].ObjectToString(),
                LastHeartbeatStamp = sqlDataReader[column_LastHeartbeatStamp].ObjectToDateTime(),
            };

            FillSimpleBaseObjectFields(result, sqlDataReader);

            return result;
        }

        /// <summary>
        /// Queries the product client.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;ProductClient&gt;.</returns>
        public List<ProductClient> QueryProductClient(ProductClientCriteria criteria)
        {
            const string spName = "sp_QueryProductClient";

            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_ProductKey, criteria.ProductKey),
                    GenerateSqlSpParameter(column_HostName, criteria.HostName),
                    GenerateSqlSpParameter(column_ServerName, criteria.ServerName),
                    GenerateSqlSpParameter(column_IpAddress, criteria.IpAddress),
                    GenerateSqlSpParameter(column_FromStamp, null),
                    GenerateSqlSpParameter(column_ToStamp, null)
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
