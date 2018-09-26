using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Beyova;
using Beyova.ExceptionSystem;

namespace Beyova.ApiTracking.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionInfoAccessController : ApiLogBaseAccessController<ExceptionInfo>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInfoAccessController" /> class.
        /// </summary>
        internal ExceptionInfoAccessController()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInfoAccessController"/> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        internal ExceptionInfoAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
            : base(primarySqlConnectionString, readOnlySqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInfoAccessController"/> class.
        /// </summary>
        /// <param name="primarySqlConnection">The SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        internal ExceptionInfoAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base(primarySqlConnection, readOnlySqlConnection)
        {
        }

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns></returns>
        protected override ExceptionInfo ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new ExceptionInfo
            {
                Key = sqlDataReader[column_Key].ObjectToGuid(),
                OperatorCredential = sqlDataReader[column_OperatorCredential].ObjectToJToken().ToObject<BaseCredential>(),

                //ApiFullName = sqlDataReader[column_ApiFullName].ObjectToString(),
                //ClientIdentifier = sqlDataReader[column_ClientIdentifier].ObjectToString(),
                //Content = sqlDataReader[column_Content].ObjectToString(),
                //ContentLength = sqlDataReader[column_ContentLength].ObjectToNullableInt64(),
                //CultureCode = sqlDataReader[column_CultureCode].ObjectToString(),
                //DeviceType = sqlDataReader[column_DeviceType].ObjectToNullableEnum<DeviceType>(),
                //Duration = sqlDataReader[column_Duration].ObjectToNullableInt32(),
                //EntryStamp = sqlDataReader[column_EntryStamp].ObjectToDateTime(),
                //ExceptionKey = sqlDataReader[column_ExceptionKey].ObjectToGuid(),
                //HitApiCache = sqlDataReader[column_HitApiCache].ObjectToBoolean(),
                //IpAddress = sqlDataReader[column_IpAddress].ObjectToString(),

                //ModuleName = sqlDataReader[column_ModuleName].ObjectToString(),

                //Platform = sqlDataReader[column_Platform].ObjectToNullableEnum<PlatformType>(),
                //Protocol = sqlDataReader[column_Protocol].ObjectToString(),
                //ResourceEntityKey = sqlDataReader[column_ResourceEntityKey].ObjectToString(),
                //ResourceName = sqlDataReader[column_ResourceName].ObjectToString(),
                //ExitStamp = sqlDataReader[column_ExitStamp].ObjectToDateTime(),
                //ReferrerUrl = sqlDataReader[column_ReferrerUrl].ObjectToString(),
                //TraceId = sqlDataReader[column_TraceId].ObjectToString(),
                //UIElementId = sqlDataReader[column_UIElementId].ObjectToString(),
                //UserAgent = sqlDataReader[column_UserAgent].ObjectToString(),
            };

            FillApiLogData(result, sqlDataReader);

            return result;
        }

        #endregion Constructor


        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="exceptionInfo">The exception information.</param>
        /// <returns></returns>
        public Guid? LogException(ExceptionInfo exceptionInfo)
        {
            const string spName = "sp_LogException";

            try
            {
                exceptionInfo.CheckNullObject(nameof(exceptionInfo));

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_MajorCode,(exceptionInfo.Code?.Major).EnumToInt32()),
                    this.GenerateSqlSpParameter(column_MinorCode, exceptionInfo.Code?.Minor),
                    this.GenerateSqlSpParameter(column_ServiceIdentifier, exceptionInfo.ServiceIdentifier),
                    this.GenerateSqlSpParameter(column_ServerIdentifier, exceptionInfo.ServerIdentifier),
                    this.GenerateSqlSpParameter(column_ServerHost, exceptionInfo.ServerHost),
                    this.GenerateSqlSpParameter(column_RawUrl, exceptionInfo.RawUrl),
                    this.GenerateSqlSpParameter(column_Message, exceptionInfo.Message),
                    this.GenerateSqlSpParameter(column_TargetSite, exceptionInfo.TargetSite),
                    this.GenerateSqlSpParameter(column_StackTrace, exceptionInfo.StackTrace),
                    this.GenerateSqlSpParameter(column_ExceptionType, exceptionInfo.ExceptionType),
                    this.GenerateSqlSpParameter(column_Level, exceptionInfo.Level.EnumToInt32()),
                    this.GenerateSqlSpParameter(column_Source, exceptionInfo.Source),
                    this.GenerateSqlSpParameter(column_EventKey, exceptionInfo.EventKey),
                    this.GenerateSqlSpParameter(column_OperatorCredential, exceptionInfo.OperatorCredential.ToJson(false)),
                    this.GenerateSqlSpParameter(column_InnerException, exceptionInfo.InnerException.ToJson(false)),
                    this.GenerateSqlSpParameter(column_Data, exceptionInfo.Data.ToJson(false)),
                    this.GenerateSqlSpParameter(column_Scene, exceptionInfo.Scene.ToJson(false)),
                    this.GenerateSqlSpParameter(column_Hint, exceptionInfo.Hint.ToJson(false)),
                };

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { exceptionInfo });
            }
        }
    }
}