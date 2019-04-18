using Beyova.Diagnostic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// Abstract class for SQL data access controller, which refers an entity in {T} type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SqlDataAccessController<T> : SqlDataAccessController
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController{T}"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        protected SqlDataAccessController(SqlDataAccessOptions options) : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        protected SqlDataAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
            : base(primarySqlConnectionString, readOnlySqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="primarySqlConnection">The SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        protected SqlDataAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base(primarySqlConnection, readOnlySqlConnection)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Converts the object.
        /// </summary>
        /// <typeparam name="TOutput">The type of the t output.</typeparam>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <param name="converter">The converter.</param>
        /// <returns>List{`0}.</returns>
        protected List<TOutput> ConvertObject<TOutput>(SqlDataReader sqlDataReader, Func<SqlDataReader, TOutput> converter)
        {
            try
            {
                sqlDataReader.CheckNullObject(nameof(sqlDataReader));
                converter.CheckNullObject(nameof(converter));

                var result = new List<TOutput>
                {
                    // When enter this method, Read() has been called for detect exception already.
                    converter(sqlDataReader)
                };

                while (sqlDataReader.Read())
                {
                    result.Add(converter(sqlDataReader));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { type = typeof(T).GetFullName() });
            }
            finally
            {
                if (sqlDataReader != null)
                {
                    sqlDataReader.Close();
                }

                if (_primaryDatabaseOperator != null)
                {
                    _primaryDatabaseOperator.Close();
                }
            }
        }

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>Object instance in type {`0}.</returns>
        protected abstract T ConvertEntityObject(SqlDataReader sqlDataReader);

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <typeparam name="TOutput">The type of the t output.</typeparam>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="converter">The converter.</param>
        /// <param name="preferReadOnlyOperator">The prefer read only operator.</param>
        /// <returns>List&lt;TOutput&gt;.</returns>
        protected List<TOutput> ExecuteReader<TOutput>(string spName, List<SqlParameter> sqlParameters, Func<SqlDataReader, TOutput> converter, bool preferReadOnlyOperator = false)
        {
            SqlDataReader reader = null;
            DatabaseOperator databaseOperator = null;

            try
            {
                converter.CheckNullObject(nameof(converter));

                reader = Execute(spName, sqlParameters, preferReadOnlyOperator, out databaseOperator);
                return reader == null ? new List<TOutput>() : ConvertObject(reader, converter);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { SpName = spName, Parameters = SqlParameterToList(sqlParameters), PreferReadOnlyOperator = preferReadOnlyOperator });
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                // use Close instead of Dispose so that operator can be reuse without re-initialize.
                databaseOperator?.Close();
            }
        }

        /// <summary>
        /// Executes the reader. If parameter <c>converter</c> is specified, use it. Otherwise, use <c>ConvertEntityObject</c>.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="preferReadOnlyOperator">The prefer read only operator.</param>
        /// <returns>List{`0}.</returns>
        protected List<T> ExecuteReader(string spName, List<SqlParameter> sqlParameters = null, bool preferReadOnlyOperator = false)
        {
            return ExecuteReader<T>(spName, sqlParameters, ConvertEntityObject, preferReadOnlyOperator);
        }
    }

    /// <summary>
    /// Class SqlDataAccessController.
    /// </summary>
    public abstract class SqlDataAccessController : IDisposable
    {
        /// <summary>
        /// The column_ SQL error code
        /// </summary>
        protected const string column_SqlErrorCode = "SqlErrorCode";

        /// <summary>
        /// The column_ SQL error reason
        /// </summary>
        protected const string column_SqlErrorReason = "SqlErrorReason";

        /// <summary>
        /// The column_ SQL error message
        /// </summary>
        protected const string column_SqlErrorMessage = "SqlErrorMessage";

        /// <summary>
        /// The column_ SQL stored procedure name
        /// </summary>
        protected const string column_SqlStoredProcedureName = "SqlStoredProcedureName";

        /// <summary>
        /// The primary database operator
        /// </summary>
        protected DatabaseOperator _primaryDatabaseOperator = null;

        /// <summary>
        /// The read only database operator
        /// </summary>
        protected DatabaseOperator _readOnlyDatabaseOperator = null;

        /// <summary>
        /// The SQL injection keywords
        /// </summary>
        protected static readonly string[] sqlInjectionKeywords = new string[] { "/*", "*/", "--" };

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController" /> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        protected SqlDataAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
            : this(new SqlDataAccessOptions(primarySqlConnectionString, readOnlySqlConnectionString))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="primarySqlConnection">The SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        protected SqlDataAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : this(new SqlDataAccessOptions(primarySqlConnection, readOnlySqlConnection))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessController"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        protected SqlDataAccessController(SqlDataAccessOptions options)
        {
            options.CheckNullObject(nameof(options));

            _primaryDatabaseOperator = new DatabaseOperator(options.PrimarySqlConnection);
            _readOnlyDatabaseOperator = options.ReadOnlySqlConnection == null ? null : new DatabaseOperator(options.ReadOnlySqlConnection);
        }

        #endregion Constructor

        #region Transanction

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="iso">The iso.</param>
        /// <param name="transactionName">Name of the transaction.</param>
        /// <returns>SqlTransactionScope.</returns>
        internal SqlTransactionScope BeginTransaction(IsolationLevel iso = IsolationLevel.Unspecified, string transactionName = null)
        {
            return _primaryDatabaseOperator.BeginTransaction(iso, transactionName);
        }

        #endregion Transanction

        /// <summary>
        /// Gets the database operator.
        /// </summary>
        /// <param name="preferReadOnlyOperator">if set to <c>true</c> [prefer read only operator].</param>
        /// <returns>DatabaseOperator.</returns>
        protected DatabaseOperator GetDatabaseOperator(bool preferReadOnlyOperator)
        {
            return (preferReadOnlyOperator && _readOnlyDatabaseOperator != null) ? _readOnlyDatabaseOperator : _primaryDatabaseOperator;
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="preferReadOnlyOperator">The prefer read only operator.</param>
        /// <returns>System.Object.</returns>
        protected object ExecuteScalar(string spName, List<SqlParameter> sqlParameters = null, bool preferReadOnlyOperator = false)
        {
            SqlDataReader reader = null;
            DatabaseOperator databaseOperator = null;

            try
            {
                reader = Execute(spName, sqlParameters, preferReadOnlyOperator, out databaseOperator);
                return reader == null ? DBNull.Value : reader[0];
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { SpName = spName, Parameters = SqlParameterToList(sqlParameters), PreferReadOnlyOperator = preferReadOnlyOperator });
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                // use Close instead of Dispose so that operator can be reuse without re-initialize.
                databaseOperator?.Close();
            }
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="preferReadOnlyOperator">The prefer read only operator.</param>
        /// <returns>System.Int32.</returns>
        protected void ExecuteNonQuery(string spName, List<SqlParameter> sqlParameters = null, bool preferReadOnlyOperator = false)
        {
            DatabaseOperator databaseOperator = null;
            SqlDataReader reader = null;

            try
            {
                reader = Execute(spName, sqlParameters, preferReadOnlyOperator, out databaseOperator);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { SpName = spName, Parameters = SqlParameterToList(sqlParameters), PreferReadOnlyOperator = preferReadOnlyOperator });
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                // use Close instead of Dispose so that operator can be reuse without re-initialize.
                databaseOperator?.Close();
            }
        }

        /// <summary>
        /// Executes the specified sp name.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="preferReadOnlyOperator">if set to <c>true</c> [prefer read only operator].</param>
        /// <param name="databaseOperator">The database operator.</param>
        /// <returns>SqlDataReader.</returns>
        protected SqlDataReader Execute(string spName, List<SqlParameter> sqlParameters, bool preferReadOnlyOperator, out DatabaseOperator databaseOperator)
        {
            try
            {
                databaseOperator = GetDatabaseOperator(preferReadOnlyOperator);
                databaseOperator.CheckNullObject(nameof(databaseOperator));

                var reader = databaseOperator.ExecuteReader(spName, sqlParameters);
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        var exception = TryGetSqlException(reader);

                        if (exception != null)
                        {
                            throw exception;
                        }
                    }

                    return reader;
                }
                else
                {
                    return null;
                }
            }
            catch (SqlStoredProcedureException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { SpName = spName, Parameters = SqlParameterToList(sqlParameters) });
            }
        }

        /// <summary>
        /// Abouts the SQL server.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        /// <returns>System.String.</returns>
        public static string AboutSqlServer(string sqlConnection)
        {
            return DatabaseOperator.AboutSqlServer(sqlConnection);
        }

        /// <summary>
        /// SQLs the parameter to list.
        /// </summary>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <returns>List{System.String}.</returns>
        protected static List<string> SqlParameterToList(List<SqlParameter> sqlParameters)
        {
            List<string> result = new List<string>();

            if (sqlParameters != null)
            {
                result.AddRange(sqlParameters.Select(one => string.Format("Name: [{0}], Type: [{1}], Value: [{2}]\r\n", one.ParameterName, one.TypeName, one.Value)));
            }

            return result;
        }

        /// <summary>
        /// Tries the get SQL exception.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>SqlStoredProcedureException.</returns>
        protected static SqlStoredProcedureException TryGetSqlException(SqlDataReader reader)
        {
            try
            {
                reader.CheckNullObject(nameof(reader));

                var storedProcedureName = reader.HasColumn(column_SqlStoredProcedureName) ? reader[column_SqlStoredProcedureName].ObjectToString() : null;
                var errorCode = reader.HasColumn(column_SqlErrorCode) ? reader[column_SqlErrorCode].ObjectToNullableInt32() : null;
                var errorReason = reader.HasColumn(column_SqlErrorReason) ? reader[column_SqlErrorReason].ObjectToString() : null;
                var errorMessage = reader.HasColumn(column_SqlErrorMessage) ? reader[column_SqlErrorMessage].ObjectToString() : null;

                if (!string.IsNullOrWhiteSpace(storedProcedureName) && errorCode != null)
                {
                    return new SqlStoredProcedureException(storedProcedureName, errorMessage, errorCode.Value, errorReason);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #region GenerateSqlSpParameter

        /// <summary>
        /// Generates the name of the SQL sp parameter.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="parameterObject">The parameter object.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>SqlParameter.</returns>
        protected SqlParameter GenerateSqlSpParameter(string columnName, object parameterObject, ParameterDirection direction = ParameterDirection.Input)
        {
            if (parameterObject != null)
            {
                if (parameterObject is Enum)
                {
                    parameterObject = (int)parameterObject;
                }
                else if (parameterObject is CryptoKey)
                {
                    parameterObject = (byte[])((CryptoKey)parameterObject);

                    // Somehow, when SQL type is VarBinary, DB NULL cannot work for null value.
                    // Need to specifically set SqlBinary.Null.
                    if (parameterObject == null)
                    {
                        parameterObject = System.Data.SqlTypes.SqlBinary.Null;
                    }
                }
                else if (parameterObject is JToken)
                {
                    parameterObject = parameterObject.ToString();
                }
                else if (parameterObject is IStringConvertable)
                {
                    parameterObject = parameterObject == null ? Convert.DBNull : parameterObject.ToString() as object;
                }
                else
                {
                    var boolParameterObject = parameterObject as bool?;
                    if (boolParameterObject.HasValue)
                    {
                        parameterObject = boolParameterObject.Value ? 1 : 0;
                    }
                }
            }

            return InternalGenerateSqlSpParameter(columnName, parameterObject ?? Convert.DBNull, direction);
        }

        /// <summary>
        /// To the SQL json.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <returns></returns>
        protected string ToSqlJson<TObject>(TObject anyObject)
        {
            return anyObject?.ToJson(false);
        }

        /// <summary>
        /// Internals the generate SQL sp parameter.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="parameterObject">The parameter object.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>SqlParameter.</returns>
        protected internal SqlParameter InternalGenerateSqlSpParameter(string columnName, object parameterObject, ParameterDirection direction = ParameterDirection.Input)
        {
            return new SqlParameter("@" + columnName.Trim(), parameterObject ?? Convert.DBNull) { Direction = direction };
        }

        #endregion GenerateSqlSpParameter

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _primaryDatabaseOperator?.Dispose();
            _readOnlyDatabaseOperator?.Dispose();
        }

        /// <summary>
        /// Creates the order by statement.
        /// </summary>
        /// <param name="orderOptions">The order options.</param>
        /// <returns></returns>
        public static string CreateOrderByStatement(List<DataOrderOption> orderOptions)
        {
            if (orderOptions.HasItem())
            {
                StringBuilder builder = new StringBuilder(orderOptions.Count * 30);

                foreach (var one in orderOptions)
                {
                    if (!string.IsNullOrWhiteSpace(one.By))
                    {
                        one.By = PreventInjection(one.By);

                        builder.AppendFormat("{0}{1},", one.By, one.Method == DataOrderOption.OrderMethod.Descending ? " DESC" : string.Empty);
                    }
                }

                builder.RemoveLastIfMatch(StringConstants.CommaChar);
                return builder.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Prevents the injection.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected static string PreventInjection(string expression)
        {
            return string.IsNullOrWhiteSpace(expression) ? expression : expression.Replace(sqlInjectionKeywords, string.Empty).Replace("'", "''");
        }

        /// <summary>
        /// Generates the where term.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <returns></returns>
        protected static string GenerateWhereTerm(KVMetaCriteriaExpression expression, string containerName = null)
        {
            if (expression != null)
            {
                StringBuilder builder = new StringBuilder(128);
                FillWhereTerm(builder, expression, containerName);
                builder.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Fills the where term.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="containerName">Name of the container.</param>
        protected static void FillWhereTerm(StringBuilder builder, KVMetaCriteriaExpression expression, string containerName = null)
        {
            if (builder != null
                && expression != null
                && !string.IsNullOrWhiteSpace(containerName)
                && !string.IsNullOrWhiteSpace(expression.ItemLeft)
                && !string.IsNullOrWhiteSpace(expression.Operator)
                && expression.ItemRight != null)
            {
                bool isStringMode = false;

                string valueInString = null;
                switch (expression.ItemRight.Type)
                {
                    case JTokenType.Boolean:
                        isStringMode = false;
                        valueInString = expression.ItemRight.Value<bool>() ? "1" : "0";
                        break;

                    case JTokenType.Integer:
                    case JTokenType.Float:
                        isStringMode = false;
                        valueInString = expression.ItemRight.ToString();
                        break;

                    case JTokenType.Bytes:
                        isStringMode = false;
                        valueInString = expression.ItemRight.Value<byte[]>().ToHex(true);
                        break;

                    case JTokenType.Date:
                        isStringMode = true;
                        valueInString = expression.ItemRight.ToObject<DateTime>().ToFullDateTimeString();
                        break;

                    case JTokenType.String:
                        valueInString = expression.ItemRight.Value<string>().PreventSqlInjection();
                        isStringMode = !string.Equals(expression.Operator, "IS", StringComparison.OrdinalIgnoreCase);
                        break;

                    case JTokenType.Guid:
                        isStringMode = true;
                        valueInString = expression.ItemRight.Value<Guid>().ToString();
                        break;

                    default:
                        return;
                }

                builder.Append("JSON_VALUE(");

                containerName = containerName.PreventSqlInjection();
                if (!string.IsNullOrWhiteSpace(containerName))
                {
                    builder.Append(containerName).Append(".");
                }
                builder.Append("[KVMeta], N'$.").Append(expression.ItemLeft.PreventSqlInjection()).Append("') ").Append(expression.Operator.PreventSqlInjection()).Append(" ");

                if (isStringMode)
                {
                    builder.Append("N'");
                }

                builder.Append(valueInString);

                if (isStringMode)
                {
                    builder.Append("'");
                }
            }
        }

        /// <summary>
        /// Generates the where term.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <returns></returns>
        protected static string GenerateWhereTerm(IEnumerable<KVMetaCriteriaExpression> expressions, string containerName = null)
        {
            if (expressions.HasItem() && !string.IsNullOrWhiteSpace(containerName))
            {
                StringBuilder builder = new StringBuilder(1024);

                foreach (var item in expressions)
                {
                    FillWhereTerm(builder, item, containerName);
                    builder.Append(" AND ");
                }

                builder.RemoveLastIfMatch(" AND ");
                return builder.ToString();
            }

            return string.Empty;
        }
    }
}