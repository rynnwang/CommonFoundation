using System.Data.SqlClient;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public class SqlDataAccessOptions
    {
        /// <summary>
        /// Gets or sets the primary SQL connection.
        /// </summary>
        /// <value>
        /// The primary SQL connection.
        /// </value>
        public SqlConnection PrimarySqlConnection { get; protected set; }

        /// <summary>
        /// Gets or sets the read only SQL connection.
        /// </summary>
        /// <value>
        /// The read only SQL connection.
        /// </value>
        public SqlConnection ReadOnlySqlConnection { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessOptions"/> class.
        /// </summary>
        public SqlDataAccessOptions()
            : this(Framework.PrimarySqlConnection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessOptions"/> class.
        /// </summary>
        /// <param name="primarySqlConnection">The primary SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        public SqlDataAccessOptions(string primarySqlConnection, string readOnlySqlConnection = null)
        {
            PrimarySqlConnection = string.IsNullOrWhiteSpace(primarySqlConnection) ? null : new SqlConnection(primarySqlConnection);
            ReadOnlySqlConnection = string.IsNullOrWhiteSpace(readOnlySqlConnection) ? null : new SqlConnection(readOnlySqlConnection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessOptions"/> class.
        /// </summary>
        /// <param name="primarySqlConnection">The primary SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        public SqlDataAccessOptions(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
        {
            PrimarySqlConnection = primarySqlConnection;
            ReadOnlySqlConnection = readOnlySqlConnection;
        }
    }
}