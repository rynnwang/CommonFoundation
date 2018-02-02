using System.Web;
using System.Web.Routing;
using Beyova.Http;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class RestApiRouter, which deeply integrated with <see cref="ContextHelper"/> for common usage.
    /// </summary>
    public sealed class RestApiRouter : RestApiRouter<HttpRequest, HttpResponse>, IRouteHandler, IHttpHandler
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiRouter"/> class.
        /// </summary>
        public RestApiRouter()
                    : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiRouter" /> class.
        /// </summary>
        /// <param name="defaultApiSettings">The default API settings.</param>
        /// <param name="allowOptions">if set to <c>true</c> [allow options].</param>
        public RestApiRouter(RestApiSettings defaultApiSettings, bool allowOptions = false)
                    : base(defaultApiSettings, allowOptions)
        {
        }

        #endregion Constructor

        /// <summary>
        /// To the web route.
        /// </summary>
        /// <returns>Route.</returns>
        public Route ToWebRoute()
        {
            var routeValueDictionary = new RouteValueDictionary { { "apiUrl", RestApiExtension.apiUrlRegex } };

            return new Route("{*apiUrl}", defaults: null, routeHandler: this, constraints: routeValueDictionary);
        }

        #region IRouteHandler

        /// <summary>
        /// Provides the object that processes the request.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the request.</param>
        /// <returns>An object that processes the request.</returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }

        #endregion IRouteHandler

        #region IHttpHandler

        /// <summary>
        /// Gets a value indicating whether this instance is reusable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is reusable; otherwise, <c>false</c>.
        /// </value>
        public bool IsReusable { get { return true; } }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The context.</param>
        public void ProcessRequest(HttpContext context)
        {
            this.ProcessHttpApiContextContainer(new HttpApiContextContainer(context.Request, context.Response, new HttpContextOptions<HttpRequest>
            {
                IncomingHttpRequestExtensible = new HttpRequestExtensible()
            }));
        }

        #endregion IHttpHandler
    }
}