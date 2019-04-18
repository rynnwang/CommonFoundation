using System;
using System.Collections.Generic;
using System.Reflection;
using Beyova.Api.RestApi;

namespace Beyova.Diagnostic
{
    /// <summary>
    /// Class ApiTraceExtension.
    /// </summary>
    public static class ApiTraceExtension
    {
        /// <summary>
        /// Converts to apitracestep.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        /// <returns></returns>
        internal static ApiTraceStep ToApiTraceStep(this RuntimeContext context, ApiTraceStep parent, DateTime? entryStamp = null)
        {
            return context != null ? new ApiTraceStep(parent, context.ApiMethod?.GetFullName(), entryStamp) : null;
        }

        /// <summary>
        /// Converts to apitracestep.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        /// <returns></returns>
        internal static ApiTraceStep ToApiTraceStep(this MethodInfo methodInfo, ApiTraceStep parent, DateTime? entryStamp = null)
        {
            return methodInfo != null ? new ApiTraceStep(parent, methodInfo.GetFullName(), entryStamp) : null;
        }

        /// <summary>
        /// Converts to flat.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public static List<ApiTraceStep> ToFlat(this ApiTraceLog log)
        {
            List<ApiTraceStep> result = new List<ApiTraceStep>();

            if (log != null)
            {
                result.Add(log as ApiTraceStep);

                FillInnerTraceLog(result, log);
            }

            return result;
        }

        /// <summary>
        /// Fills the inner trace log.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="step">The step.</param>
        private static void FillInnerTraceLog(List<ApiTraceStep> container, ApiTraceStep step)
        {
            if (step.InnerTraces != null)
            {
                foreach (var one in step.InnerTraces)
                {
                    container.Add(one);
                    FillInnerTraceLog(container, one);
                }
            }
        }
    }
}