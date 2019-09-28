using System;
using System.Web.Mvc;

namespace Beyova.Web
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SessionAuthAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// </summary>
        protected string _loginUrl, _sessionKey, _returnUrlKey;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionAuthAttribute"/> class.
        /// </summary>
        /// <param name="loginUrl">The login URL.</param>
        /// <param name="sessionKey">The session key.</param>
        /// <param name="returnUrlKey">The return URL key.</param>
        public SessionAuthAttribute(string loginUrl, string sessionKey = null, string returnUrlKey = null) : base()
        {
            _loginUrl = loginUrl.SafeToString("/login/");
            _sessionKey = sessionKey.SafeToString("userKey");
            _returnUrlKey = returnUrlKey.SafeToString("returnUrl");
        }

        #endregion Constructor

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var isOptions = HandleOptionsRequests(filterContext);

            if (!isOptions)
            {
                var isTokenRequired = filterContext.IsCurrentExecutingMethodTokenRequired();

                if (isTokenRequired)
                {
                    if (filterContext.HttpContext.Session[_sessionKey] == null)
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            filterContext.Result = new HttpUnauthorizedResult();
                        }
                        else
                        {
                            filterContext.Result = new RedirectResult(string.Format("{0}?{1}={2}", _loginUrl, _returnUrlKey, filterContext.HttpContext.Request.RawUrl.ToUrlPathEncodedText()));
                        }
                    }
                }
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
    }
}