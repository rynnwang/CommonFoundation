using Beyova;
using Beyova.Api;
using Beyova.Api.RestApi;
using Beyova.Diagnostic;
using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Beyova.Web
{
    /// <summary>
    /// Class RestApiContextConsistenceAttribute.
    /// This attribute is used in MVC to ensure following things are kept as same as REST API framework in Beyova.Common.
    /// <list type="number">
    /// <item><c>Token</c>, <c>User-Agent</c> and <c>IP address</c> would be initialized in <see cref="ContextHelper"/></item>
    /// <item><c>Exception</c>, <c>API Event</c> and <c>API Trace</c> would be handled.</item>
    /// </list>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RestApiContextConsistenceAttribute : ActionFilterAttribute, IExceptionFilter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="baseException">The base exception.</param>
        /// <returns></returns>
        public delegate RouteValueDictionary GetUnauthenticationRedirectionDelegate(ExceptionInfo baseException);

        /// <summary>
        /// The redirect route value
        /// </summary>
        private static GetUnauthenticationRedirectionDelegate getUnauthenticationRedirection = DefaultGetUnauthenticationRedirection;

        /// <summary>
        /// Defaults the get unauthentication redirection.
        /// </summary>
        /// <param name="baseException">The base exception.</param>
        /// <returns></returns>
        private static RouteValueDictionary DefaultGetUnauthenticationRedirection(ExceptionInfo baseException)
        {
            return new RouteValueDictionary{
                {"controller", "Error"},
                {"action", "Index"},
                {"code", 401},
                {"minor", baseException.Code.Minor},
                {"message", baseException.Message}
            };
        }

        /// <summary>
        /// Sets the get unauthentication redirection.
        /// </summary>
        /// <value>
        /// The get unauthentication redirection.
        /// </value>
        internal static GetUnauthenticationRedirectionDelegate GetUnauthenticationRedirection
        {
            set { getUnauthenticationRedirection = value ?? getUnauthenticationRedirection; }
        }

        /// <summary>
        /// Gets or sets the API tracking.
        /// </summary>
        /// <value>The API tracking.</value>
        public IApiTracking ApiTracking { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [log API event].
        /// </summary>
        /// <value><c>true</c> if [log API event]; otherwise, <c>false</c>.</value>
        public bool LogApiEvent { get; set; }

        /// <summary>
        /// The settings
        /// </summary>
        private RestApiSettings settings;

        /// <summary>
        /// The API event
        /// </summary>
        [ThreadStatic]
        internal static ApiEventLog ApiEvent;

        /// <summary>
        /// The depth
        /// </summary>
        [ThreadStatic]
        internal static int Depth = 0;

        /// <summary>
        /// The is options
        /// </summary>
        [ThreadStatic]
        internal static bool IsOptions = false;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiContextConsistenceAttribute"/> class.
        /// </summary>
        /// <param name="restApiSetting">The rest API settings.</param>
        public RestApiContextConsistenceAttribute(RestApiSettings restApiSetting) : base()
        {
            settings = restApiSetting ?? RestApiSettingPool.DefaultRestApiSettings;
            ApiTracking = restApiSetting?.ApiTracking;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiContextConsistenceAttribute"/> class.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        public RestApiContextConsistenceAttribute(string settingName = null)
            : this(RestApiSettingPool.GetRestApiSettingByName(settingName, true))
        {
        }

        #endregion Constructor

        /// <summary>
        /// Called when [result executed].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            if (!IsOptions)
            {
                // If using BeyovaBaseController, exception should be logger there, and in this method, no exception should appear.
                var baseException = filterContext.Exception?.Handle(new ExceptionScene
                {
                    MethodName = string.Format("{0}: {1}", filterContext.HttpContext.Request.HttpMethod, filterContext.HttpContext.Request.RawUrl)
                }, data: (filterContext.Exception as BaseException)?.ReferenceData);

                if (baseException != null)
                {
                    filterContext.Exception = baseException;
                }

                if (Depth < 2)
                {
                    if (ApiTracking != null)
                    {
                        DateTime exitStamp = DateTime.UtcNow;

                        // API EXCEPTION
                        if (baseException != null)
                        {
                            try
                            {
                                ApiTracking.LogException(baseException.ToExceptionInfo());
                            }
                            catch { }
                        }

                        // API EVENT
                        if (ApiEvent != null)
                        {
                            try
                            {
                                ApiEvent.ExitStamp = exitStamp;
                                ApiEvent.ExceptionKey = baseException?.Key;

                                ApiTracking.LogApiEvent(ApiEvent);
                            }
                            catch { }
                        }

                        // API TRACE
                        try
                        {
                            ApiTraceContext.Exit((ApiEvent?.ExceptionKey) ?? (baseException?.Key), exitStamp);
                            var traceLog = ApiTraceContext.GetCurrentTraceLog(true);

                            if (traceLog != null)
                            {
                                ApiTracking.LogApiTraceLog(traceLog);
                            }
                        }
                        catch { }
                    }

                    ContextHelper.Clear();
                }

                Depth--;
            }
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            IsOptions = HandleOptionsRequests(filterContext);

            if (Depth < 1)
            {
                Depth = 0;
                ContextHelper.Reinitialize();

                DateTime entryStamp = DateTime.UtcNow;
                var request = filterContext.HttpContext.Request;

                var traceId = request.TryGetHeader(HttpConstants.HttpHeader.TRACEID);
                var traceSequence = request.TryGetHeader(HttpConstants.HttpHeader.TRACESEQUENCE).ObjectToNullableInt32();
                var methodInfo = (filterContext.ActionDescriptor as ReflectedActionDescriptor)?.MethodInfo;

                var httpContextContainer = new HttpBaseApiContextContainer(request, filterContext.HttpContext.Response);
                ContextHelper.ConsistContext(httpContextContainer, settings);

                WebThreadPool.Register();

                if (!string.IsNullOrWhiteSpace(traceId))
                {
                    ApiTraceContext.Initialize(traceId, traceSequence, entryStamp);
                }

                if (settings != null && settings.TrackingEvent)
                {
                    var context = filterContext.HttpContext;
                    ApiEvent = new ApiEventLog
                    {
                        RawUrl = context.Request.RawUrl,
                        EntryStamp = entryStamp,
                        TraceId = traceId,
                        // If request came from ApiTransport or other proxy ways, ORIGINAL stands for the IP ADDRESS from original requester.
                        IpAddress = context.Request.TryGetHeader(settings?.OriginalIpAddressHeaderKey.SafeToString(HttpConstants.HttpHeader.ORIGINAL)).SafeToString(context.Request.UserHostAddress),
                        CultureCode = ContextHelper.ApiContext.CultureCode,
                        ContentLength = context.Request.ContentLength,
                        OperatorCredential = ContextHelper.CurrentCredential as BaseCredential,
                        ServerIdentifier = EnvironmentCore.MachineName,
                        ServiceIdentifier = EnvironmentCore.ProductName
                    };
                }

                var controllerType = methodInfo?.DeclaringType;

                var tokenRequiredAttribute = methodInfo?.GetCustomAttribute<TokenRequiredAttribute>(true) ?? controllerType?.GetCustomAttribute<TokenRequiredAttribute>(true);
                var permissionAttributes = controllerType?.GetCustomAttributes<ApiPermissionAttribute>(true).ToDictionary();
                permissionAttributes.Merge(methodInfo?.GetCustomAttributes<ApiPermissionAttribute>(true).ToDictionary(), true);

                var tokenRequired = tokenRequiredAttribute != null && tokenRequiredAttribute.TokenRequired;

                if (tokenRequired)
                {
                    if (ContextHelper.CurrentCredential == null)
                    {
                        var baseException = (new UnauthorizedTokenException(ContextHelper.Token)).Handle(
                       filterContext.HttpContext.Request.ToExceptionScene(filterContext.RouteData?.GetControllerName()), data: new { filterContext.HttpContext.Request.RawUrl });

                        HandleUnauthorizedAction(filterContext, baseException);
                    }
                    else if (permissionAttributes.HasItem())
                    {
                        var baseException = ContextHelper.CurrentUserInfo?.Permissions.ValidateApiPermission(permissionAttributes, ContextHelper.Token, methodInfo?.GetFullName());

                        if (baseException != null)
                        {
                            HandleUnauthorizedAction(filterContext, baseException);
                        }
                    }
                }

                ApiTraceContext.Enter(methodInfo.GetFullName(), setNameAsMajor: true);
            }

            Depth++;
        }

        /// <summary>
        /// Handles the unauthorized action.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <param name="baseException">The base exception.</param>
        protected virtual void HandleUnauthorizedAction(ActionExecutingContext filterContext, BaseException baseException)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                ApiHandlerBase<HttpRequestBase, HttpResponseBase>.PackageResponse(new HttpBaseApiContextContainer(filterContext.HttpContext.Request, filterContext.HttpContext.Response), null, null, ex: baseException, settings: settings);
                filterContext.Result = null;
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(routeValues: getUnauthenticationRedirection.Invoke(baseException?.ToExceptionInfo()));
            }
        }

        /// <summary>
        /// Handles the options requests.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns></returns>
        protected bool HandleOptionsRequests(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.HttpMethod.Equals(HttpConstants.HttpMethod.Options, StringComparison.OrdinalIgnoreCase))
            {
                filterContext.Result = new EmptyResult();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Called when [exception].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception?.ToExceptionInfo();
            ContextHelper.Clear();
            Framework.ApiTracking?.LogException(exception);
            filterContext.Result = new JsonNetResult { Data = exception };
        }
    }
}