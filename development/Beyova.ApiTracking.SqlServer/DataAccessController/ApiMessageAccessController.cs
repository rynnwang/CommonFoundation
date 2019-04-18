using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Beyova.Diagnostic;

namespace Beyova.ApiTracking.SqlServer
{
    /// <summary>
    ///
    /// </summary>
    public class ApiMessageAccessController : BaseApiTrackingAccessController<ApiMessage>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiMessageAccessController" /> class.
        /// </summary>
        public ApiMessageAccessController()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiMessageAccessController"/> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        public ApiMessageAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
            : base(primarySqlConnectionString, readOnlySqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiMessageAccessController"/> class.
        /// </summary>
        /// <param name="primarySqlConnection">The SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        public ApiMessageAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base(primarySqlConnection, readOnlySqlConnection)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns></returns>
        protected override ApiMessage ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new ApiMessage
            {
                Message = sqlDataReader[column_Message].ObjectToString(),
                Category = sqlDataReader[column_Category].ObjectToString(),
                Key = sqlDataReader[column_Key].ObjectToGuid(),
                CreatedStamp = sqlDataReader[column_CreatedStamp].ObjectToDateTime(),
            };

            FillServiceIdentifiable(result, sqlDataReader);

            return result;
        }

        /// <summary>
        /// Logs the API message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public Guid? LogApiMessage(ApiMessage message)
        {
            const string spName = "sp_LogApiMessage";

            try
            {
                message.CheckNullObject(nameof(message));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_ServerIdentifier, message.ServerIdentifier),
                    GenerateSqlSpParameter(column_ServiceIdentifier, message.ServiceIdentifier),
                    GenerateSqlSpParameter(column_Category, message.Category),
                    GenerateSqlSpParameter(column_Message, message.Message),
                };

                return ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { message });
            }
        }

        /// <summary>
        /// Queries the API message.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public List<ApiMessage> QueryApiMessage(ApiMessageCriteria criteria)
        {
            const string spName = "sp_QueryApiMessage";

            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key, criteria.Key),
                    GenerateSqlSpParameter(column_Count, criteria.Count),
                    GenerateSqlSpParameter(column_ServerIdentifier, criteria.ServerIdentifier),
                    GenerateSqlSpParameter(column_ServiceIdentifier, criteria.ServiceIdentifier),
                    GenerateSqlSpParameter(column_Category, criteria.Category),
                    GenerateSqlSpParameter(column_Message, criteria.Message),
                    GenerateSqlSpParameter(column_FromStamp, criteria.FromStamp),
                    GenerateSqlSpParameter(column_ToStamp, criteria.ToStamp)
                };

                return ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { criteria });
            }
        }

    }
}