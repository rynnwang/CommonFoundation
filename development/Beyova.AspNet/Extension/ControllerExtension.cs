using System.Web;
using System.Web.Mvc;
using Beyova.Api.RestApi;
using Beyova.Diagnostic;

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

        /// <summary>
        /// Jsons the net.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static JsonNetResult JsonNet(this Controller controller, object obj)
        {
            return new JsonNetResult
            {
                Data = obj,
                ContentType = HttpConstants.ContentType.Json
            };
        }
    }
}