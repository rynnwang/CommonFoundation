using System;
using System.Web.Mvc;
using System.Web.Routing;
using Beyova;
using Beyova.ExceptionSystem;

namespace Beyova.Portal
{
    /// <summary>
    /// Class ErrorController.
    /// </summary>
    public class ErrorController : BeyovaPortalController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Beyova.WebExtension.BeyovaBaseController" /> class.
        /// </summary>
        /// <param name="apiTracking">The API tracking.</param>
        /// <param name="returnExceptionAsFriendly">if set to <c>true</c> [return exception as friendly].</param>
        public ErrorController(IApiTracking apiTracking = null, bool returnExceptionAsFriendly = false)
            : base("Error", apiTracking, returnExceptionAsFriendly)
        {
        }

        /// <summary>
        /// Indexes the specified error code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="message">The message.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Index(int? code = null, string minor = null, string message = null)
        {
            var exceptionInfo = new ExceptionInfo
            {
                Code = new ExceptionCode
                {
                    Major = ((ExceptionCode.MajorCode?)code) ?? ExceptionCode.MajorCode.Undefined,
                    Minor = minor
                },
                Message = message
            };

            return View(ErrorView, exceptionInfo);
        }

        /// <summary>
        /// Forbiddens the by ownership.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ForbiddenByOwnership()
        {
            return View("ForbiddenByOwnership");
        }

        /// <summary>
        /// Registers to route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterToRoute(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Error",
                url: "Error/{code}",
                defaults: new { controller = "Error", action = "Index", code = UrlParameter.Optional },
                namespaces: new string[] { "Beyova.Portal" }
            );
        }
    }
}