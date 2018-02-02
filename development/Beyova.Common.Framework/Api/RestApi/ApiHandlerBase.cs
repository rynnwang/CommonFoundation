using System.Web;
using Beyova.Http;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class ApiHandlerBase.
    /// </summary>
    public abstract class ApiHandlerBase : ApiHandlerBase<HttpRequest, HttpResponse>, IHttpHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiHandlerBase" /> class.
        /// </summary>
        /// <param name="defaultApiSettings">The default API settings.</param>
        /// <param name="allowOptions">if set to <c>true</c> [allow options].</param>
        protected ApiHandlerBase(RestApiSettings defaultApiSettings, bool allowOptions = false) : base(defaultApiSettings, allowOptions)
        {
        }

        #region IHttpHandler

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            base.ProcessHttpApiContextContainer(new HttpApiContextContainer(context.Request, context.Response, new HttpContextOptions<HttpRequest>
            {
                IncomingHttpRequestExtensible = new HttpRequestExtensible()
            }));
        }

        #endregion IHttpHandler
    }
}