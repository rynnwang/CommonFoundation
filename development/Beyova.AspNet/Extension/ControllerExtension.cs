using Beyova.Api.RestApi;
using Beyova.Diagnostic;
using System.Net;
using System.Web;
using System.Web.Mvc;

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

        /// <summary>
        /// Accepteds the result.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns></returns>
        public static ActionResult HttpAcceptedResult(this Controller controller)
        {
            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// HTTPs the created result.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns></returns>
        public static ActionResult HttpCreatedResult(this Controller controller)
        {
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }

        /// <summary>
        /// Clouds the content of the BLOB file.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="downloadCredential">The download credential.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static ActionResult CloudBlobFileResult(this Controller controller, BinaryStorageActionCredential downloadCredential, string contentType = null, string fileName = null)
        {
            if (controller != null && downloadCredential != null && !string.IsNullOrWhiteSpace(downloadCredential.CredentialUri))
            {
                var httpRequest = downloadCredential.CredentialUri.CreateHttpWebRequest();
                var stream = httpRequest.GetResponse().GetResponseStream();

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    controller.Response.SafeSetHttpHeader(HttpConstants.HttpHeader.ContentDisposition, fileName);
                }

                return new FileStreamResult(stream, contentType.SafeToString(HttpConstants.ContentType.BinaryDefault));
            }

            return new EmptyResult();
        }
    }
}