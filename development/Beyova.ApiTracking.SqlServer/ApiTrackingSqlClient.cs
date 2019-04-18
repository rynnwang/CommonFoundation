using System;
using System.Collections.Generic;
using System.Linq;
using Beyova.Diagnostic;

namespace Beyova.ApiTracking.SqlServer
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ApiTrackingSqlClient : IApiTracking, IApiTrackingRawDateReader
    {
        /// <summary>
        /// The SQL connection string
        /// </summary>
        private string _sqlConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTrackingSqlClient"/> class.
        /// </summary>
        public ApiTrackingSqlClient() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTrackingSqlClient"/> class.
        /// </summary>
        /// <param name="sqlConnectionString">The SQL connection string.</param>
        public ApiTrackingSqlClient(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString.SafeToString(Framework.PrimarySqlConnection);
        }

        /// <summary>
        /// Logs the API event.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        public void LogApiEvent(ApiEventLog eventLog)
        {
            throw new NotSupportedException();
        }

        public void LogApiTraceLog(ApiTraceLog traceLog)
        {
            //TODO
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="exceptionInfo">The exception information.</param>
        public void LogException(ExceptionInfo exceptionInfo)
        {
            try
            {
                exceptionInfo.CheckNullObject(nameof(exceptionInfo));

                using (var controller = new ExceptionInfoAccessController(_sqlConnectionString))
                {
                    controller.LogException(exceptionInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { exceptionInfo });
            }
        }

        /// <summary>
        /// Queries the exception information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public List<ExceptionInfo> QueryExceptionInfo(ExceptionCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                using (var controller = new ExceptionInfoAccessController(_sqlConnectionString))
                {
                    return controller.QueryException(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { criteria });
            }
        }

        /// <summary>
        /// Gets the exception by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public ExceptionInfo GetExceptionByKey(Guid? key)
        {
            try
            {
                key.CheckNullObject(nameof(key));

                using (var controller = new ExceptionInfoAccessController(_sqlConnectionString))
                {
                    return controller.QueryException(new ExceptionCriteria { Key = key }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { key });
            }
        }

        /// <summary>
        /// Logs the API message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogApiMessage(ApiMessage message)
        {
            try
            {
                message.CheckNullObject(nameof(message));

                using (var controller = new ApiMessageAccessController(_sqlConnectionString))
                {
                    controller.LogApiMessage(message);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { message });
            }
        }

        /// <summary>
        /// Queries the API message.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public List<ApiMessage> QueryApiMessage(ApiMessageCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                using (var controller = new ApiMessageAccessController(_sqlConnectionString))
                {
                    return controller.QueryApiMessage(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { criteria });
            }
        }

        public List<ApiTraceLog> GetApiTraceLogById(string traceId)
        {
            throw new NotImplementedException();
        }
    }
}