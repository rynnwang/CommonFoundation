using Beyova.Diagnostic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Beyova.ApiTracking.SqlServer
{
    /// <summary>
    ///
    /// </summary>
    public class ExceptionInfoAccessController : GlobalApiUniqueIdentifierAccessController<ExceptionInfo>
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
            var result = new ExceptionInfo();
            FillExceptionInfo(result, sqlDataReader, false);
            return result;
        }

        /// <summary>
        /// Fills the exception information.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <param name="simpleMode">if set to <c>true</c> [simple mode].</param>
        protected void FillExceptionInfo(ExceptionInfo exception, SqlDataReader sqlDataReader, bool simpleMode = false)
        {
            if (exception != null && sqlDataReader != null)
            {
                exception.Key = sqlDataReader[column_Key].ObjectToGuid();
                exception.OperatorCredential = sqlDataReader[column_OperatorCredential].ObjectToJToken().ToObject<BaseCredential>();
                exception.Code = new ExceptionCode
                {
                    Major = sqlDataReader[column_MajorCode].ObjectToEnum<ExceptionCode.MajorCode>(),
                    Minor = sqlDataReader[column_MinorCode].ObjectToString()
                };
                exception.RawUrl = sqlDataReader[column_RawUrl].ObjectToString();
                exception.Message = sqlDataReader[column_Message].ObjectToString();
                exception.TargetSite = sqlDataReader[column_TargetSite].ObjectToString();
                exception.StackTrace = sqlDataReader[column_StackTrace].ObjectToString();
                exception.ExceptionType = sqlDataReader[column_ExceptionType].ObjectToString();
                exception.Source = simpleMode ? null : sqlDataReader[column_Source].ObjectToString();
                exception.EventKey = sqlDataReader[column_EventKey].ObjectToString();
                exception.InnerException = simpleMode ? null : sqlDataReader[column_InnerException].ObjectToJsonObject<ExceptionInfo>();
                exception.Data = simpleMode ? null : sqlDataReader[column_Data].ObjectToJsonObject<JToken>();
                exception.Scene = simpleMode ? null : sqlDataReader[column_Scene].ObjectToJsonObject<ExceptionScene>();
                exception.CreatedStamp = sqlDataReader[column_CreatedStamp].ObjectToDateTime();

                FillGlobalApiUniqueIdentifier(exception, sqlDataReader);
            }
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
                    GenerateSqlSpParameter(column_MajorCode,(exceptionInfo.Code?.Major).EnumToInt32()),
                    GenerateSqlSpParameter(column_MinorCode, exceptionInfo.Code?.Minor),
                    GenerateSqlSpParameter(column_ServiceIdentifier, exceptionInfo.ServiceIdentifier),
                    GenerateSqlSpParameter(column_ServerIdentifier, exceptionInfo.ServerIdentifier),
                    GenerateSqlSpParameter(column_HttpMethod, exceptionInfo.HttpMethod),
                    GenerateSqlSpParameter(column_Path, exceptionInfo.Path),
                    GenerateSqlSpParameter(column_RawUrl, exceptionInfo.RawUrl),
                    GenerateSqlSpParameter(column_Message, exceptionInfo.Message),
                    GenerateSqlSpParameter(column_TargetSite, exceptionInfo.TargetSite),
                    GenerateSqlSpParameter(column_StackTrace, exceptionInfo.StackTrace),
                    GenerateSqlSpParameter(column_ExceptionType, exceptionInfo.ExceptionType),
                    GenerateSqlSpParameter(column_Source, exceptionInfo.Source),
                    GenerateSqlSpParameter(column_EventKey, exceptionInfo.EventKey),
                    GenerateSqlSpParameter(column_OperatorCredential, ToSqlJson(exceptionInfo.OperatorCredential)),
                    GenerateSqlSpParameter(column_InnerException,ToSqlJson(exceptionInfo.InnerException)),
                    GenerateSqlSpParameter(column_Data,ToSqlJson( exceptionInfo.Data)),
                    GenerateSqlSpParameter(column_Scene, ToSqlJson(exceptionInfo.Scene)),
                    GenerateSqlSpParameter(column_CreatedStamp, exceptionInfo.CreatedStamp)
                };

                return ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { exceptionInfo });
            }
        }

        /// <summary>
        /// Queries the exception.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public List<ExceptionInfo> QueryException(ExceptionCriteria criteria)
        {
            const string spName = "sp_QueryException";

            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key,criteria.Key),
                    GenerateSqlSpParameter(column_MajorCode,criteria.MajorCode.EnumToInt32()),
                    GenerateSqlSpParameter(column_MinorCode, criteria.MinorCode),
                    GenerateSqlSpParameter(column_ServiceIdentifier, criteria.ServiceIdentifier),
                    GenerateSqlSpParameter(column_ServerIdentifier, criteria.ServerIdentifier),
                    GenerateSqlSpParameter(column_HttpMethod, criteria.HttpMethod),
                    GenerateSqlSpParameter(column_Path, criteria.Path),
                    GenerateSqlSpParameter(column_RawUrl, criteria.RawUrl),
                    GenerateSqlSpParameter(column_ExceptionType, criteria.ExceptionType),
                    GenerateSqlSpParameter(column_EventKey, criteria.EventKey),
                    GenerateSqlSpParameter(column_Keyword, criteria.Keyword),
                    GenerateSqlSpParameter(column_OperatorCredential, criteria.OperatorCredential),
                    GenerateSqlSpParameter(column_FromStamp, criteria.FromStamp),
                    GenerateSqlSpParameter(column_ToStamp, criteria.ToStamp),
                    GenerateSqlSpParameter(column_Count, criteria.Count)
                };

                return ExecuteReader<ExceptionInfo>(spName, parameters, sqlDataReader =>
                {
                    var result = new ExceptionInfo();
                    FillExceptionInfo(result, sqlDataReader, true);
                    return result;
                });
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { criteria });
            }
        }
    }
}