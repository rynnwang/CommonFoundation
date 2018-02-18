using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Beyova.ApiTracking;
using Beyova.Cache;
using Beyova.ExceptionSystem;
using Beyova.ProgrammingIntelligence;

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

        #region Protected Methods

        /// <summary>
        /// Processes the route.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override RuntimeContext ProcessRoute(HttpApiContextContainer<TRequest, TResponse> context)
        {
            return ProcessRequestToRuntimeContext(context.HttpMethod, context.Url, context.RequestHeaders, true);
        }

        /// <summary>
        /// Processes the request to runtime context.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="doAuthentication">The do authentication.</param>
        /// <returns>Beyova.RestApi.RuntimeContext.</returns>
        internal RuntimeContext ProcessRequestToRuntimeContext(string httpMethod, Uri uri, NameValueCollection headers, bool doAuthentication = true)
        {
            uri.CheckNullObject(nameof(uri));

            var result = new RuntimeContext();
            var rawFullUrl = string.Format("{0}: {1}", httpMethod, uri.ToString());

            if (!uri.FillRouteInfo(result))
            {
                throw ExceptionFactory.CreateInvalidObjectException("URL");
            }

            if (result.Version.Equals(ApiConstants.BuiltInFeatureVersionKeyword, StringComparison.OrdinalIgnoreCase))
            {
                return result;
            }

            if (string.IsNullOrWhiteSpace(result.ResourceName))
            {
                throw new ResourceNotFoundException(rawFullUrl, nameof(result.ResourceName));
            }

            RuntimeRoute runtimeRoute;

            if (!RestApiRoutePool.Routes.TryGetValue(new ApiRouteIdentifier(result.Realm, result.Version, result.ResourceName, httpMethod, result.Parameter1), out runtimeRoute))
            {
                RestApiRoutePool.Routes.TryGetValue(new ApiRouteIdentifier(result.Realm, result.Version, result.ResourceName, httpMethod, null), out runtimeRoute);
            }
            else
            {
                if (runtimeRoute != null && (!string.IsNullOrWhiteSpace(result.Parameter1) && !runtimeRoute.IsActionUsed))
                {
                    throw new ResourceNotFoundException(rawFullUrl);
                }
            }

            if (runtimeRoute == null)
            {
                throw new ResourceNotFoundException(rawFullUrl);
            }

            // Override out parameters
            result.OperationParameters = runtimeRoute.OperationParameters ?? new RuntimeApiOperationParameters();

            result.ApiMethod = runtimeRoute.ApiMethod;
            result.ApiInstance = runtimeRoute.ApiInstance;
            result.IsActionUsed = runtimeRoute.IsActionUsed;
            result.IsVoid = runtimeRoute.IsVoid;
            result.Settings = runtimeRoute.Setting;
            result.OmitApiEvent = runtimeRoute.OmitApiTracking?.Omit(ApiTrackingType.Event) ?? false;

            if (runtimeRoute.ApiCacheAttribute != null)
            {
                result.ApiCacheIdentity = runtimeRoute.ApiRouteIdentifier.Clone() as ApiRouteIdentifier;
                if (runtimeRoute.ApiCacheAttribute.CacheParameter.CachedByParameterizedIdentity)
                {
                    result.ApiCacheIdentity.SetParameterizedIdentifier(uri.ToQueryString());
                }

                result.ApiCacheContainer = runtimeRoute.ApiCacheContainer;

                if (result.ApiCacheContainer != null)
                {
                    string cachedResponseBody;
                    if (result.ApiCacheContainer.GetCacheResult(result.ApiCacheIdentity, out cachedResponseBody))
                    {
                        result.CachedResponseBody = cachedResponseBody;
                        result.ApiCacheStatus = ApiCacheStatus.UseCache;
                    }
                    else
                    {
                        result.ApiCacheStatus = ApiCacheStatus.UpdateCache;
                    }
                }
                else
                {
                    result.ApiCacheStatus = ApiCacheStatus.NoCache;
                }
            }

            var tokenHeaderKey = (result.Settings ?? DefaultSettings)?.TokenHeaderKey;
            var token = (headers != null && !string.IsNullOrWhiteSpace(tokenHeaderKey)) ? headers.Get(tokenHeaderKey).SafeToString() : string.Empty;

            string userIdentifier = ContextHelper.ApiContext.Token = token;

            var authenticationException = doAuthentication ? Authenticate(runtimeRoute, token, out userIdentifier) : null;

            if (authenticationException != null)
            {
                throw authenticationException.Handle(new { result.ApiMethod.Name, token });
            }

            return result;
        }

        #endregion Protected Methods

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
            const string localhostTip = "This API is available at localhost machine.";

            switch (runtimeContext?.ResourceName.SafeToLower())
            {
                case "apilist":
                    result = RestApiRoutePool.Routes.Select(x => new
                    {
                        Url = x.Key.ToString().EnsureEndWith('/'),
                        Method = x.Value.ApiMethod?.Name,
                        TokenRequired = x.Value?.OperationParameters?.IsTokenRequired
                    }).ToList();
                    break;

                case "configuration":
                    result = isLocalhost ? Framework.ConfigurationValues : localhostTip as object;
                    break;

                case "featureswitch":
                    result = FeatureModuleSwitch.GetModuleWorkStatus();
                    break;

                case "doc":
                case "doc.zip":
                    DocumentGenerator generator = new DocumentGenerator(DefaultSettings.TokenHeaderKey.SafeToString(HttpConstants.HttpHeader.TOKEN));
                    result = generator.WriteHtmlDocumentToZipByRoutes((from item in RestApiRoutePool.Routes select item.Value).Distinct().ToArray());
                    contentType = HttpConstants.ContentType.ZipFile;
                    break;

                default: break;
            }

            return result ?? base.ProcessBuiltInFeature(context, runtimeContext, isLocalhost, out contentType);
        }

        /// <summary>
        /// Authenticates the specified service type.
        /// </summary>
        /// <param name="runtimeRoute">The runtime route.</param>
        /// <param name="token">The token.</param>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        private BaseException Authenticate(RuntimeRoute runtimeRoute, string token, out string userIdentifier)
        {
            userIdentifier = token;
            ICredential credential = null;

            if (!string.IsNullOrWhiteSpace(token))
            {
                var eventHandlers = (runtimeRoute.Setting ?? DefaultSettings)?.EventHandlers;

                if (eventHandlers != null)
                {
                    try
                    {
                        credential = eventHandlers.GetCredentialByToken(token);
                    }
                    catch { }

                    if (credential != null)
                    {
                        userIdentifier = credential.Name;
                    }
                }
            }

            ContextHelper.ApiContext.CurrentCredential = credential;

            if (!runtimeRoute.OperationParameters.IsTokenRequired)
            {
                return null;
            }

            //Check permissions
            if (credential != null)
            {
                var userPermissions = ContextHelper.ApiContext.CurrentPermissionIdentifiers?.Permissions ?? new List<string>();
                return userPermissions.ValidateApiPermission(runtimeRoute.OperationParameters.Permissions, token, runtimeRoute.ApiMethod.GetFullName());
            }

            return new UnauthorizedTokenException(new { token });
        }
    }
}