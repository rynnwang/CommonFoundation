using System.Data.SqlClient;
using Beyova.Diagnostic;

namespace Beyova.ApiTracking.SqlServer
{
    /// <summary>
    ///
    /// </summary>
    public abstract class BaseApiTrackingAccessController<T> : BaseDataAccessController<T>
    {
        /// <summary>
        /// The column HTTP method
        /// </summary>
        protected const string column_HttpMethod = "HttpMethod";

        /// <summary>
        /// The column category
        /// </summary>
        protected const string column_Category = "Category";

        /// <summary>
        /// The column geo information
        /// </summary>
        protected const string column_GeoInfo = "GeoInfo";

        /// <summary>
        /// The column path
        /// </summary>
        protected const string column_Path = "Path";

        /// <summary>
        /// The column ApiUniqueIdentifier
        /// </summary>
        protected const string column_ApiUniqueIdentifier = "ApiUniqueIdentifier";

        /// <summary>
        /// The column raw URL
        /// </summary>
        protected const string column_RawUrl = "RawUrl";

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
        /// The column module name
        /// </summary>
        protected const string column_ModuleName = "ModuleName";

        /// <summary>
        /// The column protocol
        /// </summary>
        protected const string column_Protocol = "Protocol";

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
        /// Initializes a new instance of the <see cref="BaseApiTrackingAccessController{T}" /> class.
        /// </summary>
        protected BaseApiTrackingAccessController()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiTrackingAccessController{T}"/> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        protected BaseApiTrackingAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
            : base(primarySqlConnectionString, readOnlySqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiTrackingAccessController{T}"/> class.
        /// </summary>
        /// <param name="primarySqlConnection">The SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        protected BaseApiTrackingAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base(primarySqlConnection, readOnlySqlConnection)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Fills the service identifiable.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        protected void FillServiceIdentifiable(IServiceIdentifiable entity, SqlDataReader sqlDataReader)
        {
            if (entity != null && sqlDataReader != null)
            {
                entity.ServerIdentifier = sqlDataReader[column_ServerIdentifier].ObjectToString();
                entity.ServiceIdentifier = sqlDataReader[column_ServiceIdentifier].ObjectToString();
            }
        }
    }
}