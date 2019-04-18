using System;
using System.Collections.Generic;
using Beyova.Api.RestApi;
using Beyova.Diagnostic;

namespace Beyova
{
    /// <summary>
    /// Interface IApiTrackingRawDateReader
    /// </summary>
    public interface IApiTrackingRawDateReader
    {
        #region Get Entity

        /// <summary>
        /// Gets the exception by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [ApiOperation(nameof(ExceptionInfo), HttpConstants.HttpMethod.Get)]
        ExceptionInfo GetExceptionByKey(Guid? key);

        /// <summary>
        /// Queries the exception.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [ApiOperation(nameof(ExceptionInfo), HttpConstants.HttpMethod.Post)]
        List<ExceptionInfo> QueryExceptionInfo(ExceptionCriteria criteria);

        /// <summary>
        /// Gets the API trace log by identifier.
        /// </summary>
        /// <param name="traceId">The trace identifier.</param>
        /// <returns>List&lt;ApiTraceLog&gt;.</returns>
        [ApiOperation(nameof(ApiTraceLog), HttpConstants.HttpMethod.Get)]
        List<ApiTraceLog> GetApiTraceLogById(string traceId);

        /// <summary>
        /// Queries the API message.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [ApiOperation(nameof(ApiMessage), HttpConstants.HttpMethod.Post)]
        List<ApiMessage> QueryApiMessage(ApiMessageCriteria criteria);

        #endregion Get Entity
    }
}