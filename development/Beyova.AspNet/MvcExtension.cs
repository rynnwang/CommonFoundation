using Beyova.Api;
using Beyova.Diagnostic;
using System;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Beyova.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class MvcExtension
    {
        /// <summary>
        /// Gets the current executing method information.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns></returns>
        public static MethodInfo GetCurrentExecutingMethodInfo(this ActionExecutingContext filterContext)
        {
            return (filterContext?.ActionDescriptor as ReflectedActionDescriptor)?.MethodInfo;
        }

        /// <summary>
        /// Determines whether [is current executing method token required].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns>
        ///   <c>true</c> if [is current executing method token required] [the specified filter context]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCurrentExecutingMethodTokenRequired(this ActionExecutingContext filterContext)
        {
            var method = filterContext.GetCurrentExecutingMethodInfo();
            var tokenRequired = (method?.GetCustomAttribute<TokenRequiredAttribute>()) ?? (method.DeclaringType.GetCustomAttribute<TokenRequiredAttribute>());
            return tokenRequired?.TokenRequired ?? false;
        }

        /// <summary>
        /// Processes the exception.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="operationName">Name of the operation.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns></returns>
        public static ActionResult ProcessException(this Controller controller, Exception ex, 
            [CallerMemberName] string operationName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (controller != null)
            {
                var exception = ex?.Handle(new { routeData = controller.ControllerContext.RequestContext.RouteData.ToJson() }, operationName: operationName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber);

                if (exception != null)
                {
                    if (exception.Code.Major == ExceptionCode.MajorCode.OperationFailure)
                    {
                        Framework.ApiTracking?.LogException(exception.ToExceptionInfo());
                    }

                    controller.PackageResponse(null, exception);
                    return null;
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }
    }
}