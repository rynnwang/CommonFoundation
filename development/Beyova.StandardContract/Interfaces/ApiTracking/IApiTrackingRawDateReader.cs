using System;
using System.Collections.Generic;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;

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
        ExceptionInfo GetExceptionByKey(Guid? key);

        /// <summary>
        /// Gets the API trace log by identifier.
        /// </summary>
        /// <param name="traceId">The trace identifier.</param>
        /// <returns>List&lt;ApiTraceLog&gt;.</returns>
        List<ApiTraceLog> GetApiTraceLogById(string traceId);

        #endregion Query Entity
    }
}