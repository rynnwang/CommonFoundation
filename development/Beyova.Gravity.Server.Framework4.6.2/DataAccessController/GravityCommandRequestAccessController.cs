using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Beyova.Gravity.DataAccessController
{
    /// <summary>
    /// Class GravityCommandRequestAccessController.
    /// </summary>
    public class GravityCommandRequestAccessController : GravityAccessController<GravityCommandRequest>
    {
        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>Object instance in type {`0}.</returns>
        protected override GravityCommandRequest ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new GravityCommandRequest
            {
                Key = sqlDataReader[column_Key].ObjectToGuid(),
                Action = sqlDataReader[column_Action].ObjectToString(),
                ProductKey = sqlDataReader[column_ProductKey].ObjectToGuid(),
                Parameters = sqlDataReader[column_Parameters].ObjectToJToken(),
                ExpiredStamp = sqlDataReader[column_ExpiredStamp].ObjectToDateTime(),
            };

            return result;
        }

        /// <summary>
        /// Gets the pending command request.
        /// </summary>
        /// <param name="clientKey">The client key.</param>
        /// <returns>List&lt;GravityCommandRequest&gt;.</returns>
        public List<GravityCommandRequest> GetPendingCommandRequest(Guid? clientKey)
        {
            const string spName = "sp_GetPendingCommandRequest";

            try
            {
                clientKey.CheckNullObject(nameof(clientKey));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_ClientKey,clientKey)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { clientKey });
            }
        }

        /// <summary>
        /// Saves the heartbeat information.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? RequestCommand(GravityCommandRequest request)
        {
            const string spName = "sp_RequestCommand";

            try
            {
                request.CheckNullObject(nameof(request));
                request.ProductKey.CheckNullObject(nameof(request.ProductKey));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_ProductKey,request.ProductKey),
                    GenerateSqlSpParameter(column_Action, request.Action),
                    GenerateSqlSpParameter(column_Parameters, request.Parameters),
                    GenerateSqlSpParameter(column_ExpiredStamp, request.ExpiredStamp)
                };

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { request });
            }
        }
    }
}
