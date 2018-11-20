using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Beyova.ApiTracking.SqlServer
{
    /// <summary>
    ///
    /// </summary>
    public class ApiEventLogAccessController : ApiLogBaseAccessController<ApiEventLog>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventLogAccessController" /> class.
        /// </summary>
        protected ApiEventLogAccessController()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventLogAccessController"/> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        protected ApiEventLogAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
            : base(primarySqlConnectionString, readOnlySqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventLogAccessController"/> class.
        /// </summary>
        /// <param name="primarySqlConnection">The SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        protected ApiEventLogAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base(primarySqlConnection, readOnlySqlConnection)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns></returns>
        protected override ApiEventLog ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new ApiEventLog
            {
                ApiFullName = sqlDataReader[column_ApiFullName].ObjectToString(),
                ClientIdentifier = sqlDataReader[column_ClientIdentifier].ObjectToString(),
                Content = sqlDataReader[column_Content].ObjectToString(),
                ContentLength = sqlDataReader[column_ContentLength].ObjectToNullableInt64(),
                CultureCode = sqlDataReader[column_CultureCode].ObjectToString(),
                DeviceType = sqlDataReader[column_DeviceType].ObjectToNullableEnum<DeviceType>(),
                Duration = sqlDataReader[column_Duration].ObjectToNullableInt32(),
                EntryStamp = sqlDataReader[column_EntryStamp].ObjectToDateTime(),
                ExceptionKey = sqlDataReader[column_ExceptionKey].ObjectToGuid(),
                HitApiCache = sqlDataReader[column_HitApiCache].ObjectToBoolean(),
                IpAddress = sqlDataReader[column_IpAddress].ObjectToString(),
                Key = sqlDataReader[column_Key].ObjectToGuid(),
                ModuleName = sqlDataReader[column_ModuleName].ObjectToString(),
                OperatorCredential = sqlDataReader[column_OperatorCredential].ObjectToJToken().ToObject<BaseCredential>(),
                Platform = sqlDataReader[column_Platform].ObjectToNullableEnum<PlatformType>(),
                Protocol = sqlDataReader[column_Protocol].ObjectToString(),
                ResourceEntityKey = sqlDataReader[column_ResourceEntityKey].ObjectToString(),
                ResourceName = sqlDataReader[column_ResourceName].ObjectToString(),
                ExitStamp = sqlDataReader[column_ExitStamp].ObjectToDateTime(),
                ReferrerUrl = sqlDataReader[column_ReferrerUrl].ObjectToString(),
                TraceId = sqlDataReader[column_TraceId].ObjectToString(),
                UIElementId = sqlDataReader[column_UIElementId].ObjectToString(),
                UserAgent = sqlDataReader[column_UserAgent].ObjectToString(),
            };

            FillApiLogData(result, sqlDataReader);

            return result;
        }

        /// <summary>
        /// Logs the API event.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        /// <returns></returns>
        public Guid? LogApiEvent(ApiEventLog eventLog)
        {
            const string spName = "sp_LogApiEvent";

            try
            {
                eventLog.CheckNullObject(nameof(eventLog));

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_ApiFullName, eventLog.ApiFullName)
                };

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { eventLog });
            }
        }
    }
}