using System;
using System.Collections.Generic;
using System.Reflection;
using Beyova.ApiTracking;
using Beyova.Cache;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class RuntimeRoute.
    /// </summary>
    public class RuntimeRoute
    {
        /// <summary>
        /// Gets or sets the API method.
        /// </summary>
        /// <value>
        /// The API method.
        /// </value>
        public MethodInfo ApiMethod { get; protected set; }

        /// <summary>
        /// Gets or sets the API instance.
        /// </summary>
        /// <value>
        /// The API instance.
        /// </value>
        public object ApiInstance { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is void.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is void; otherwise, <c>false</c>.
        /// </value>
        public bool? IsVoid { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is action used.
        /// </summary>
        /// <value><c>true</c> if this instance is action used; otherwise, <c>false</c>.</value>
        public bool IsActionUsed { get; protected set; }

        /// <summary>
        /// Gets or sets the setting.
        /// </summary>
        /// <value>The setting.</value>
        public RestApiSettings Setting { get; protected set; }

        /// <summary>
        /// Gets or sets the operation parameters.
        /// </summary>
        /// <value>The operation parameters.</value>
        internal RuntimeApiOperationParameters OperationParameters { get; private set; }

        /// <summary>
        /// Gets or sets the type of the instance.
        /// </summary>
        /// <value>The type of the instance.</value>
        public Type InstanceType { get; protected set; }

        /// <summary>
        /// Gets the API route identifier.
        /// </summary>
        /// <value>
        /// The API route identifier.
        /// </value>
        internal ApiRouteIdentifier ApiRouteIdentifier { get; private set; }

        /// <summary>
        /// Gets the omit API tracking.
        /// </summary>
        /// <value>
        /// The omit API tracking.
        /// </value>
        internal OmitApiTrackingAttribute OmitApiTracking { get; private set; }

        #region Cache

        /// <summary>
        /// Gets the API cache attribute.
        /// </summary>
        /// <value>
        /// The API cache attribute.
        /// </value>
        internal ApiCacheAttribute ApiCacheAttribute { get; private set; }

        /// <summary>
        /// Gets or sets the API cache container.
        /// </summary>
        /// <value>
        /// The API cache container.
        /// </value>
        internal IApiCacheContainer ApiCacheContainer { get; private set; }

        #endregion Cache

        /// <summary>
        /// Prevents a default instance of the <see cref="RuntimeRoute"/> class from being created.
        /// </summary>
        private RuntimeRoute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeRoute" /> class.
        /// </summary>
        /// <param name="routeIdentifier">The route key.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="isActionUsed">if set to <c>true</c> [is action used].</param>
        /// <param name="isTokenRequired">if set to <c>true</c> [is token required].</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="isDataSensitive">if set to <c>true</c> [is data sensitive].</param>
        /// <param name="setting">The setting.</param>
        /// <param name="apiCacheAttribute">The API cache attribute.</param>
        /// <param name="omitApiTracking">The omit API tracking.</param>
        /// <param name="permissions">The permissions.</param>
        /// <param name="headerKeys">The header keys.</param>
        public RuntimeRoute(ApiRouteIdentifier routeIdentifier, MethodInfo methodInfo, Type instanceType, object instance, bool isActionUsed, bool isTokenRequired, string moduleName, string contentType, bool isDataSensitive, RestApiSettings setting, ApiCacheAttribute apiCacheAttribute, OmitApiTrackingAttribute omitApiTracking, IDictionary<string, ApiPermission> permissions = null, List<string> headerKeys = null) : this()
        {
            this.ApiMethod = methodInfo;
            this.ApiInstance = instance;
            this.IsActionUsed = isActionUsed;
            this.InstanceType = instanceType;
            this.Setting = setting;
            this.OmitApiTracking = omitApiTracking;

            this.OperationParameters = new RuntimeApiOperationParameters
            {
                ContentType = contentType,
                IsTokenRequired = isTokenRequired,
                IsDataSensitive = isDataSensitive,
                CustomizedHeaderKeys = headerKeys,
                Permissions = permissions,
                ModuleName = moduleName
            };

            this.ApiRouteIdentifier = routeIdentifier;
            this.IsVoid = this.ApiMethod?.ReturnType?.IsVoid();

            if (apiCacheAttribute != null)
            {
                this.ApiCacheAttribute = apiCacheAttribute;
                this.ApiCacheContainer = apiCacheAttribute.CacheContainer ?? new ApiCacheContainer(routeIdentifier.ToString(), apiCacheAttribute.CacheParameter);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.ApiMethod == null ? (InstanceType == null ? string.Empty : InstanceType.FullName) : ApiMethod.GetFullName();
        }
    }
}