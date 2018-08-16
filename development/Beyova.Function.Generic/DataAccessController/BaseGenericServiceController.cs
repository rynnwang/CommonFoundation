using System.Collections.Generic;
using System.Data.SqlClient;

namespace Beyova.Function.Generic
{
    /// <summary>
    /// Class BaseGenericServiceController.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <typeparam name="TCriteria">The type of the t criteria.</typeparam>
    public abstract class BaseGenericServiceController<TEntity, TCriteria> : BaseDataAccessController<TEntity>
        where TEntity : new()
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGenericServiceController{TEntity,TCriteria}" /> class.
        /// </summary>
        public BaseGenericServiceController()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGenericServiceController{TEntity, TCriteria}"/> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        public BaseGenericServiceController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
          : base(primarySqlConnectionString, readOnlySqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGenericServiceController{TEntity, TCriteria}"/> class.
        /// </summary>
        /// <param name="primarySqlConnection">The primary SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        public BaseGenericServiceController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base(primarySqlConnection, readOnlySqlConnection)
        {
        }

        #endregion

        /// <summary>
        /// Fills the additional field value.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="entity">The entity.</param>
        protected virtual void FillAdditionalFieldValue(List<SqlParameter> parameters, TEntity entity) { }

        /// <summary>
        /// Fills the additional field value.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="criteria">The criteria.</param>
        protected virtual void FillAdditionalFieldValue(List<SqlParameter> parameters, TCriteria criteria) { }

        /// <summary>
        /// Fills the additional field value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="reader">The reader.</param>
        protected virtual void FillAdditionalFieldValue(TEntity entity, SqlDataReader reader) { }
    }
}