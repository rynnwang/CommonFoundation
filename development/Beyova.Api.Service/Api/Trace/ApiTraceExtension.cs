using System;
using System.Collections.Generic;
using System.Reflection;
using Beyova.Api.RestApi;
using Beyova.ApiTracking;

namespace Beyova.Api
{
    /// <summary>
    /// Class ApiTraceExtension.
    /// </summary>
    public static class ApiTraceExtension
    {
        /// <summary>
        /// To the trace log.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        /// <returns>Beyova.ApiTracking.ApiTraceLogPiece.</returns>
        internal static ApiTraceLogPiece ToTraceLog(this RuntimeContext context, ApiTraceLogPiece parent, DateTime? entryStamp = null)
        {
            return context != null ? new ApiTraceLogPiece(parent, context.ApiMethod?.GetFullName(), entryStamp) : null;
        }

        /// <summary>
        /// To the trace log.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        /// <returns>Beyova.ApiTracking.ApiTraceLogPiece.</returns>
        internal static ApiTraceLogPiece ToTraceLog(this MethodInfo methodInfo, ApiTraceLogPiece parent, DateTime? entryStamp = null)
        {
            return methodInfo != null ? new ApiTraceLogPiece(parent, methodInfo.GetFullName(), entryStamp) : null;
        }

        ///// <summary>
        ///// To the trace log.
        ///// </summary>
        ///// <param name="methodCallMessage">The method call message.</param>
        ///// <param name="parent">The parent.</param>
        ///// <param name="entryStamp">The entry stamp.</param>
        ///// <returns></returns>
        //internal static ApiTraceLogPiece ToTraceLog(this MethodCallInfo methodCallMessage, ApiTraceLogPiece parent, DateTime? entryStamp = null)
        //{
        //    return methodCallMessage != null ? new ApiTraceLogPiece(parent, methodCallMessage.MethodFullName) : null;
        //}

        /// <summary>
        /// To the flat.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns>List&lt;ApiTraceLogPiece&gt;.</returns>
        public static List<ApiTraceLogPiece> ToFlat(this ApiTraceLog log)
        {
            List<ApiTraceLogPiece> result = new List<ApiTraceLogPiece>();

            if (log != null)
            {
                result.Add(log as ApiTraceLogPiece);

                FillInnerTraceLog(result, log);
            }

            return result;
        }

        /// <summary>
        /// Fills the inner trace log.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="log">The log.</param>
        private static void FillInnerTraceLog(List<ApiTraceLogPiece> container, ApiTraceLogPiece log)
        {
            if (log.InnerTraces != null)
            {
                foreach (var one in log.InnerTraces)
                {
                    container.Add(one);
                    FillInnerTraceLog(container, one);
                }
            }
        }
    }
}