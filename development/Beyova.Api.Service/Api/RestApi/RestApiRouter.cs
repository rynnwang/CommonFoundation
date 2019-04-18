using System.Linq;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class RestApiRouter, which deeply integrated with <see cref="ContextHelper"/> for common usage.
    /// </summary>
    public abstract class RestApiRouter<TRequest, TResponse> : ApiHandlerBase<TRequest, TResponse>
    {
        #region Protected fields

        ///// <summary>
        ///// The route operation locker
        ///// </summary>
        //protected static object routeOperationLocker = new object();

        ///// <summary>
        ///// The routes
        ///// </summary>
        //protected static volatile Dictionary<ApiRouteIdentifier, RuntimeRoute> routes =
        //    new Dictionary<ApiRouteIdentifier, RuntimeRoute>(EqualityComparer<ApiRouteIdentifier>.Default);

        ///// <summary>
        ///// Gets the runtime routes.
        ///// </summary>
        ///// <value>
        ///// The runtime routes.
        ///// </value>
        //internal static List<RuntimeRoute> RuntimeRoutes
        //{
        //    get
        //    {
        //        return routes.Values.ToList();
        //    }
        //}

        ///// <summary>
        ///// The initialized types
        ///// </summary>
        //protected static volatile HashSet<string> initializedTypes = new HashSet<string>();

        /// <summary>
        /// The _first instance
        /// </summary>
        protected static object _firstInstance;

        /// <summary>
        /// Gets the first instance.
        /// </summary>
        /// <value>The first instance.</value>
        public static object FirstInstance { get { return _firstInstance; } }

        #endregion Protected fields

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
            if (_firstInstance == null)
            {
                _firstInstance = this;
            }
        }

        #endregion Constructor

        /// <summary>
        /// Adds the handler (instance and settings) into route.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="settings">The settings.</param>
        public void Add(object instance, RestApiSettings settings = null)
        {
            RestApiRoutePool.Add(instance, settings);
        }

        /// <summary>
        /// Processes the build in feature.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="runtimeContext">The runtime context.</param>
        /// <param name="isLocalhost">The is localhost.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        /// System.Object.
        /// </returns>
        protected override object ProcessBuiltInFeature(HttpApiContextContainer<TRequest, TResponse> context, RuntimeContext runtimeContext, bool isLocalhost, out string contentType)
        {
            object result = null;
            contentType = HttpConstants.ContentType.Json;

            switch ((runtimeContext?.ResourceName).SafeToString().ToLowerInvariant())
            {
                case "apilist":
                    result = RestApiRoutePool.Routes.Select(x => new
                    {
                        Url = x.Key.ToRoutePath(true),
                        TokenRequired = x.Value?.OperationParameters?.IsTokenRequired
                    }).ToList();
                    break;

                case "configuration":
                    result = isLocalhost ? Framework.ConfigurationValues : localhostTip as object;
                    break;

                case "doc":
                case "doc.zip":
                    result = ApiRouterExtensionFeature.DocumentatGeneration;

                    //DocumentGenerator generator = new DocumentGenerator(RestApiSettingPool.DefaultRestApiSettings?.TokenHeaderKey.SafeToString(HttpConstants.HttpHeader.TOKEN));
                    //result = generator.WriteHtmlDocumentToZipByRoutes((from item in RestApiRoutePool.Routes select item.Value).Distinct().ToArray());
                    if (result != null)
                    {
                        contentType = HttpConstants.ContentType.ZipFile;
                    }
                    break;

                default: break;
            }

            return result ?? base.ProcessBuiltInFeature(context, runtimeContext, isLocalhost, out contentType);
        }
    }
}