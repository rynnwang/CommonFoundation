using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Beyova.Cache;
using Beyova.Diagnostic;

namespace Beyova.Api.RestApi
{
    /// <summary>
    ///
    /// </summary>
    internal static class RestApiRoutePool
    {
        /// <summary>
        /// The route operation locker
        /// </summary>
        internal static object routeOperationLocker = new object();

        /// <summary>
        /// The routes
        /// </summary>
        private static volatile Dictionary<ApiRouteIdentifier, RuntimeRoute> routes =
            new Dictionary<ApiRouteIdentifier, RuntimeRoute>(EqualityComparer<ApiRouteIdentifier>.Default);

        /// <summary>
        /// Gets the routes.
        /// </summary>
        /// <value>
        /// The routes.
        /// </value>
        internal static Dictionary<ApiRouteIdentifier, RuntimeRoute> Routes { get { return routes; } }

        /// <summary>
        /// Gets the runtime routes.
        /// </summary>
        /// <value>
        /// The runtime routes.
        /// </value>
        internal static List<RuntimeRoute> RuntimeRoutes
        {
            get
            {
                return routes.Values.ToList();
            }
        }

        /// <summary>
        /// The initialized types
        /// </summary>
        private static volatile HashSet<string> initializedTypes = new HashSet<string>();

        /// <summary>
        /// Adds the handler (instance and settings) into route.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="settings">The settings.</param>
        public static void Add(object instance, RestApiSettings settings = null)
        {
            InitializeRoute(instance, settings);
        }

        /// <summary>
        /// Initializes the routes.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="settings">The settings.</param>
        /// <exception cref="DataConflictException">Route</exception>
        private static void InitializeRoute(object instance, RestApiSettings settings = null)
        {
            lock (routeOperationLocker)
            {
                if (instance != null)
                {
                    var typeName = instance.GetType().FullName;
                    if (!initializedTypes.Contains(typeName))
                    {
                        #region Initialize routes

                        var doneInterfaceTypes = new List<string>();

                        foreach (var interfaceType in instance.GetType().GetInterfaces())
                        {
                            InitializeApiType(doneInterfaceTypes, routes, interfaceType, instance, settings);
                        }

                        #endregion Initialize routes

                        initializedTypes.Add(typeName);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the type of the API.
        /// </summary>
        /// <param name="doneInterfaceTypes">The done interface types.</param>
        /// <param name="routes">The routes.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="parentApiContractAttribute">The parent API class attribute.</param>
        /// <param name="parentApiModuleAttribute">The parent API module attribute.</param>
        /// <param name="omitApiTrackingAttribute">The omit API tracking attribute.</param>
        /// <param name="parentTokenRequiredAttribute">The parent token required attribute.</param>
        /// <exception cref="DataConflictException">routeKey</exception>
        private static void InitializeApiType(List<string> doneInterfaceTypes, Dictionary<ApiRouteIdentifier, RuntimeRoute> routes, Type interfaceType, object instance, RestApiSettings settings = null, ApiContractAttribute parentApiContractAttribute = null, ApiModuleAttribute parentApiModuleAttribute = null, OmitApiTrackingAttribute omitApiTrackingAttribute = null, TokenRequiredAttribute parentTokenRequiredAttribute = null)
        {
            if (routes != null && interfaceType != null && doneInterfaceTypes != null)
            {
                if (doneInterfaceTypes.Contains(interfaceType.FullName))
                {
                    return;
                }

                var apiContract = parentApiContractAttribute ?? interfaceType.GetCustomAttribute<ApiContractAttribute>(true);
                var omitApiTracking = omitApiTrackingAttribute ?? interfaceType.GetCustomAttribute<OmitApiTrackingAttribute>(true);
                var apiModule = parentApiModuleAttribute ?? interfaceType.GetCustomAttribute<ApiModuleAttribute>(true);
                var tokenRequiredAttribute = parentTokenRequiredAttribute ?? interfaceType.GetCustomAttribute<TokenRequiredAttribute>(true);
                var moduleName = apiModule?.ToString();

                if (apiContract != null && !string.IsNullOrWhiteSpace(apiContract.Version))
                {
                    if (apiContract.Version.SafeEquals(ApiConstants.BuiltInFeatureVersionKeyword, StringComparison.OrdinalIgnoreCase))
                    {
                        throw ExceptionFactory.CreateInvalidObjectException(nameof(apiContract.Version), reason: "<builtin> cannot be used as version due to it is used internally.");
                    }

                    foreach (var method in interfaceType.GetMethods())
                    {
                        var apiOperationAttribute = method.GetCustomAttribute<ApiOperationAttribute>(true);

                        #region Initialize based on ApiOperation

                        if (apiOperationAttribute != null)
                        {
                            var permissions = new Dictionary<string, ApiPermissionAttribute>();
                            var additionalHeaderKeys = new HashSet<string>();

                            var apiPermissionAttributes =
                                method.GetCustomAttributes<ApiPermissionAttribute>(true);

                            var apiCacheAttribute = method.GetCustomAttribute<ApiCacheAttribute>(true);

                            if (apiPermissionAttributes != null)
                            {
                                foreach (var one in apiPermissionAttributes)
                                {
                                    permissions.Merge(one.PermissionIdentifier, one);
                                }
                            }

                            var headerKeyAttributes = method.GetCustomAttributes<ApiHeaderAttribute>(true);
                            if (headerKeyAttributes != null)
                            {
                                foreach (var one in headerKeyAttributes)
                                {
                                    additionalHeaderKeys.Add(one.HeaderKey);
                                }
                            }

                            var routeKey = ApiRouteIdentifier.FromApiObjects(apiContract, apiOperationAttribute);

                            var tokenRequired =
                                method.GetCustomAttribute<TokenRequiredAttribute>(true) ??
                                tokenRequiredAttribute;

                            // If method can not support API cache, consider as no api cache.
                            if (apiCacheAttribute != null && (!apiOperationAttribute.HttpMethod.Equals(HttpConstants.HttpMethod.Get, StringComparison.OrdinalIgnoreCase) || !apiCacheAttribute.InitializeParameterNames(method)))
                            {
                                apiCacheAttribute = null;
                            }

                            var runtimeRoute = new RuntimeRoute(routeKey, method, interfaceType, instance,
                                   !string.IsNullOrWhiteSpace(apiOperationAttribute.Action),
                                   tokenRequired != null && tokenRequired.TokenRequired, moduleName, apiOperationAttribute.ContentType, settings, apiCacheAttribute, omitApiTracking ?? method.GetCustomAttribute<OmitApiTrackingAttribute>(true), permissions, additionalHeaderKeys.ToList());

                            if (routes.ContainsKey(routeKey))
                            {
                                throw new DataConflictException(nameof(routeKey), objectIdentity: routeKey?.ToString(), data: new
                                {
                                    existed = routes[routeKey].SafeToString(),
                                    newMethod = method.GetFullName(),
                                    newInterface = interfaceType.FullName
                                });
                            }

                            // EntitySynchronizationModeAttribute
                            var entitySynchronizationModeAttribute = method.GetCustomAttribute<EntitySynchronizationModeAttribute>(true);
                            if (entitySynchronizationModeAttribute != null)
                            {
                                if (EntitySynchronizationModeAttribute.IsReturnTypeMatched(method.ReturnType))
                                {
                                    runtimeRoute.OperationParameters.EntitySynchronizationMode = entitySynchronizationModeAttribute;
                                }
                            }

                            routes.Add(routeKey, runtimeRoute);
                        }

                        #endregion Initialize based on ApiOperation
                    }

                    foreach (var one in interfaceType.GetInterfaces())
                    {
                        InitializeApiType(doneInterfaceTypes, routes, one, instance, settings, apiContract, apiModule, omitApiTracking, tokenRequiredAttribute);
                    }

                    //Special NOTE:
                    // Move this add action in scope of if apiContract is valid.
                    // Reason: in complicated cases, when [A:Interface1] without ApiContract, but [Interface2: Interface] with defining ApiContract, and [B: A, Interface2], then correct contract definition might be missed.
                    doneInterfaceTypes.Add(interfaceType.FullName);
                }
            }
        }
    }
}