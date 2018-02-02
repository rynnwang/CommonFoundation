using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Beyova.Gravity.DataAccessController
{
    /// <summary>
    /// Class GravityCommandResultAccessController.
    /// </summary>
    public class GravityCommandResultAccessController : GravityAccessController<GravityCommandResult>
    {
        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>Object instance in type {`0}.</returns>
        protected override GravityCommandResult ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new GravityCommandResult
            {
                ClientKey = sqlDataReader[column_Clientkey].ObjectToGuid(),
                Content = sqlDataReader[column_Content].ObjectToJToken(),
                RequestKey = sqlDataReader[column_RequestKey].ObjectToGuid(),
                Key = sqlDataReader[column_Key].ObjectToGuid()
            };

            return result;
        }

        /// <summary>
        /// Saves the heartbeat information.
        /// </summary>
        /// <param name="commandResult">The request.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CommitCommandResult(GravityCommandResult commandResult)
        {
            const string spName = "sp_CommitCommandResult";

            try
            {
                commandResult.CheckNullObject(nameof(commandResult));
                commandResult.RequestKey.CheckNullObject(nameof(commandResult.RequestKey));
                commandResult.ClientKey.CheckNullObject(nameof(commandResult.ClientKey));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_RequestKey,commandResult.RequestKey),
                    GenerateSqlSpParameter(column_ClientKey, commandResult.ClientKey),
                    GenerateSqlSpParameter(column_Content, commandResult.Content)
                };

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { commandResult });
            }
        }

        /// <summary>
        /// Queries the command result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;GravityCommandResult&gt;.</returns>
        public List<SimpleBaseObject<GravityCommandResult>> QueryCommandResult(GravityCommandResultCriteria criteria)
        {
            const string spName = "sp_QueryCommandResult";

            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key,criteria.Key),
                    GenerateSqlSpParameter(column_RequestKey,criteria.RequestKey),
                    GenerateSqlSpParameter(column_ProductKey,criteria.ProductKey),
                    GenerateSqlSpParameter(column_ClientKey, criteria.ClientKey)
                };

                return this.ExecuteReaderAsSimpleBaseObject(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }
    }
}
