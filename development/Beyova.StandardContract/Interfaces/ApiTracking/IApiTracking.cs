using Beyova.Api.RestApi;
using Beyova.Diagnostic;

namespace Beyova
{
    /// <summary>
    /// Interface IApiTracking
    /// </summary>
    public interface IApiTracking
    {
        /// <summary>
        /// Logs the API event asynchronous.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        [ApiOperation(nameof(ApiEventLog), HttpConstants.HttpMethod.Put)]
        void LogApiEvent(ApiEventLog eventLog);

        /// <summary>
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exceptionInfo">The exception information.</param>
        [ApiOperation(nameof(ExceptionInfo), HttpConstants.HttpMethod.Put)]
        void LogException(ExceptionInfo exceptionInfo);

        /// <summary>
        /// Logs the API trace log asynchronous.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        [ApiOperation(nameof(ApiTraceLog), HttpConstants.HttpMethod.Put)]
        void LogApiTraceLog(ApiTraceLog traceLog);

        /// <summary>
        /// Logs the API message.
        /// </summary>
        /// <param name="message">The message.</param>
        [ApiOperation(nameof(ApiMessage), HttpConstants.HttpMethod.Put)]
        void LogApiMessage(ApiMessage message);
    }
}