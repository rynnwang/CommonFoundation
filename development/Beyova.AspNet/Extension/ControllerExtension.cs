using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Beyova.Api.RestApi;
using Beyova.ExceptionSystem;

namespace Beyova.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class ControllerExtension
    {
        /// <summary>
        /// Packages the response.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="data">The data.</param>
        /// <param name="ex">The ex.</param>
        public static void PackageResponse(this Controller controller, object data, BaseException ex = null)
        {
            ApiHandlerBase<HttpRequestBase, HttpResponseBase>.PackageResponse(
                new HttpBaseApiContextContainer(controller.Request, controller.Response), data, null, ex);
        }
    }
}
