using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Beyova.ApiTracking.SqlServer
{
    /// <summary>
    ///
    /// </summary>
    public class ApiMessageAccessController : BaseDataAccessController<ApiMessage>
    {
        /// <summary>
        /// The column category
        /// </summary>
        protected const string column_Category = "Category";

        /// <summary>
        /// The column message
        /// </summary>
        protected const string column_Message = "Message";

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
                CreatedStamp = sqlDataReader[column_CreatedStamp].ObjectToDateTime()
            };

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
                    this.GenerateSqlSpParameter(column_Category, message.Category),
                    this.GenerateSqlSpParameter(column_Message, message.Message),
                };

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { message });
            }
        }
    }
}