using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Beyova.Function.Generic
{
    /// <summary>
    /// Access controller for <see cref="BinaryStorageIdentifier" /> class instance.
    /// </summary>
    public class BinaryStorageIdentifierAccessController : BaseGenericServiceController<BinaryStorageIdentifier, BinaryStorageMetaDataCriteria>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageIdentifierAccessController" /> class.
        /// </summary>
        public BinaryStorageIdentifierAccessController()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageIdentifierAccessController"/> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        public BinaryStorageIdentifierAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
          : base(primarySqlConnectionString, readOnlySqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageIdentifierAccessController"/> class.
        /// </summary>
        /// <param name="primarySqlConnection">The primary SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        public BinaryStorageIdentifierAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base(primarySqlConnection, readOnlySqlConnection)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>BinaryStorageIdentifier.</returns>
        protected override BinaryStorageIdentifier ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            return new BinaryStorageIdentifier
            {
                Container = sqlDataReader[column_Container].ObjectToString(),
                Identifier = sqlDataReader[column_Identifier].ObjectToString()
            };
        }

        /// <summary>
        /// Gets the pending delete storages.
        /// </summary>
        /// <returns>List&lt;BinaryStorageIdentifier&gt;.</returns>
        public List<BinaryStorageIdentifier> GetPendingDeleteStorages()
        {
            const string spName = "sp_GetPendingDeleteBinaryStorages";

            try
            {
                return this.ExecuteReader(spName, new List<SqlParameter>());
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Converts the pending commit to pending commit.
        /// </summary>
        /// <param name="stamp">The stamp.</param>
        /// <returns>System.Int32.</returns>
        public void ConvertPendingCommitToPendingDelete(DateTime? stamp)
        {
            const string spName = "sp_ConvertPendingCommitToPendingDelete";

            try
            {
                stamp.CheckNullObject(nameof(stamp));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Stamp, stamp)
                };

                this.ExecuteNonQuery(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(stamp);
            }
        }

        /// <summary>
        /// Commits the deletion.
        /// </summary>
        /// <param name="identifiers">The identifiers.</param>
        /// <returns>System.Int32.</returns>
        public void CommitDeletion(List<BinaryStorageIdentifier> identifiers)
        {
            const string spName = "sp_CommitBinaryStorageDeletion";

            try
            {
                identifiers.CheckNullOrEmptyCollection(nameof(identifiers));

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Identifiers, identifiers.ToJson(false))
                };

                this.ExecuteNonQuery(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(identifiers);
            }
        }
    }
}