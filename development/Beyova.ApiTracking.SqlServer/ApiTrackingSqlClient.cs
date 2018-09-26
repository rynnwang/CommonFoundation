using System;
using Beyova.ExceptionSystem;

namespace Beyova.ApiTracking.SqlServer
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ApiTrackingSqlClient : IApiTracking
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

        public void LogApiEvent(ApiEventLog eventLog)
        {
            //TODO
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
        /// Logs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                LogApiMessage(new ApiMessage { Message = message });
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
    }
}