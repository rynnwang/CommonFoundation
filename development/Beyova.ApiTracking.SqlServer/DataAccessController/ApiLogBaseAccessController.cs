using System.Data.SqlClient;

namespace Beyova.ApiTracking.SqlServer
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ApiLogBaseAccessController<T> : BaseDataAccessController<T>
        where T : ApiLogBase
    {
        /// <summary>
        /// The column raw URL
        /// </summary>
        protected const string column_RawUrl = "RawUrl";

        /// <summary>
        /// The column server host
        /// </summary>
        protected const string column_ServerHost = "ServerHost";

        /// <summary>
        /// The column server identifier
        /// </summary>
        protected const string column_ServerIdentifier = "ServerIdentifier";

        /// <summary>
        /// The column service identifier
        /// </summary>
        protected const string column_ServiceIdentifier = "ServiceIdentifier";

        /// <summary>
        /// The column operator credential
        /// </summary>
        protected const string column_OperatorCredential = "OperatorCredential";

        /// <summary>
        /// The column API full name
        /// </summary>
        protected const string column_ApiFullName = "ApiFullName";

        /// <summary>
        /// The column client identifier
        /// </summary>
        protected const string column_ClientIdentifier = "ClientIdentifier";

        /// <summary>
        /// The column content length
        /// </summary>
        protected const string column_ContentLength = "ContentLength";

        /// <summary>
        /// The column duration
        /// </summary>
        protected const string column_Duration = "Duration";

        /// <summary>
        /// The column entry stamp
        /// </summary>
        protected const string column_EntryStamp = "EntryStamp";

        /// <summary>
        /// The column exception key
        /// </summary>
        protected const string column_ExceptionKey = "ExceptionKey";

        /// <summary>
        /// The column hit API cache
        /// </summary>
        protected const string column_HitApiCache = "HitApiCache";

        /// <summary>
        /// The column module name
        /// </summary>
        protected const string column_ModuleName = "ModuleName";

        /// <summary>
        /// The column protocol
        /// </summary>
        protected const string column_Protocol = "Protocol";

        /// <summary>
        /// The column resource entity key
        /// </summary>
        protected const string column_ResourceEntityKey = "ResourceEntityKey";

        /// <summary>
        /// The column resource name
        /// </summary>
        protected const string column_ResourceName = "ResourceName";

        /// <summary>
        /// The column exit stamp
        /// </summary>
        protected const string column_ExitStamp = "ExitStamp";

        /// <summary>
        /// The column UI element identifier
        /// </summary>
        protected const string column_UIElementId = "UIElementId";

        /// <summary>
        /// The column trace identifier
        /// </summary>
        protected const string column_TraceId = "TraceId";

        /// <summary>
        /// The column referrer URL
        /// </summary>
        protected const string column_ReferrerUrl = "ReferrerUrl";

        /// <summary>
        /// The column major code
        /// </summary>
        protected const string column_MajorCode = "MajorCode";

        /// <summary>
        /// The column minor code
        /// </summary>
        protected const string column_MinorCode = "MinorCode";

        /// <summary>
        /// The column message
        /// </summary>
        protected const string column_Message = "Message";

        /// <summary>
        /// The column target site
        /// </summary>
        protected const string column_TargetSite = "TargetSite";

        /// <summary>
        /// The column stack trace
        /// </summary>
        protected const string column_StackTrace = "StackTrace";

        /// <summary>
        /// The column exception type
        /// </summary>
        protected const string column_ExceptionType = "ExceptionType";

        /// <summary>
        /// The column level
        /// </summary>
        protected const string column_Level = "Level";

        /// <summary>
        /// The column source
        /// </summary>
        protected const string column_Source = "Source";

        /// <summary>
        /// The column event key
        /// </summary>
        protected const string column_EventKey = "EventKey";

        /// <summary>
        /// The column inner exception
        /// </summary>
        protected const string column_InnerException = "InnerException";

        /// <summary>
        /// The column data
        /// </summary>
        protected const string column_Data = "Data";

        /// <summary>
        /// The column scene
        /// </summary>
        protected const string column_Scene = "Scene";

        /// <summary>
        /// The column hint
        /// </summary>
        protected const string column_Hint = "Hint";

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiLogBaseAccessController{T}" /> class.
        /// </summary>
        protected ApiLogBaseAccessController()
            : this(Framework.PrimarySqlConnection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        protected ApiLogBaseAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
            : base(primarySqlConnectionString, readOnlySqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="primarySqlConnection">The SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        protected ApiLogBaseAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base(primarySqlConnection, readOnlySqlConnection)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Fills the API log data.
        /// </summary>
        /// <param name="logData">The log data.</param>
        /// <param name="reader">The reader.</param>
        protected void FillApiLogData(ApiLogBase logData, SqlDataReader reader)
        {
            if (logData != null && reader != null)
            {
                logData.RawUrl = reader[column_RawUrl].ObjectToString();
                logData.ServerHost = reader[column_ServerHost].ObjectToString();
                logData.ServerIdentifier = reader[column_ServerIdentifier].ObjectToString();
                logData.ServiceIdentifier = reader[column_ServiceIdentifier].ObjectToString();
            }
        }
    }
}