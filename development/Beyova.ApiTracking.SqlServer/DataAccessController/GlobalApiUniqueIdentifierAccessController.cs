using System.Data.SqlClient;
using Beyova.Diagnostic;

namespace Beyova.ApiTracking.SqlServer
{
    /// <summary>
    ///
    /// </summary>
    public abstract class GlobalApiUniqueIdentifierAccessController<T> : BaseApiTrackingAccessController<T>
        where T : GlobalApiUniqueIdentifier
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalApiUniqueIdentifierAccessController{T}" /> class.
        /// </summary>
        protected GlobalApiUniqueIdentifierAccessController()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalApiUniqueIdentifierAccessController{T}"/> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        protected GlobalApiUniqueIdentifierAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
            : base(primarySqlConnectionString, readOnlySqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalApiUniqueIdentifierAccessController{T}"/> class.
        /// </summary>
        /// <param name="primarySqlConnection">The SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        protected GlobalApiUniqueIdentifierAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base(primarySqlConnection, readOnlySqlConnection)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Converts the global API unique identifier.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        protected void FillGlobalApiUniqueIdentifier(GlobalApiUniqueIdentifier entity, SqlDataReader sqlDataReader)
        {
            if (entity != null && sqlDataReader != null)
            {
                FillServiceIdentifiable(entity, sqlDataReader);
                entity.HttpMethod = sqlDataReader[column_HttpMethod].ObjectToString();
                entity.Path = sqlDataReader[column_Path].ObjectToString();
            }
        }
    }
}