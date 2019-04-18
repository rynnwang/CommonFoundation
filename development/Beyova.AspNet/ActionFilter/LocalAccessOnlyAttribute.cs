using System;
using System.Web.Mvc;

namespace Beyova.Web
{
    /// <summary>
    /// Class LocalAccessOnlyAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class LocalAccessOnlyAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsLocal)
            {
                filterContext.Result = new HttpNotFoundResult();
            }
        }
    }
}