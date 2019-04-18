using System;
using System.Collections.Generic;
using Beyova.Http;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class RestApiClientBase.
    /// </summary>
    public abstract class RestApiClientBase
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token
        {
            get
            {
                return Endpoint?.Token;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [accept GZIP].
        /// </summary>
        /// <value><c>true</c> if [accept g zip]; otherwise, <c>false</c>.</value>
        public bool AcceptGZip { get; protected set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        public ApiEndpoint Endpoint { get; protected set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        public int? Timeout { get; protected set; }

        /// <summary>
        /// The base client version
        /// </summary>
        public const int BaseClientVersion = 4;

        /// <summary>
        /// The client generated version
        /// </summary>
        protected abstract int ClientGeneratedVersion { get; }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>
        /// The user agent.
        /// </value>
        public string UserAgent { get; protected set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClientBase" /> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="acceptGZip">if set to <c>true</c> [accept g zip].</param>
        /// <param name="timeout">The timeout.</param>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="System.NotSupportedException"></exception>
        public RestApiClientBase(ApiEndpoint endpoint, bool acceptGZip = false, int? timeout = null)
        {
            //Try detecting base client version and generated version.
            if (ClientGeneratedVersion != BaseClientVersion)
            {
                throw new NotSupportedException(string.Format("ClientGeneratedVersion [{0}] doesnot match BaseClientVersion [{1}].", ClientGeneratedVersion, BaseClientVersion));
            }

            Endpoint = endpoint ?? new ApiEndpoint();
            AcceptGZip = acceptGZip;
            Timeout = timeout;

            var actualType = GetType();

            var version = (actualType.Assembly.GetCustomAttribute<BeyovaComponentAttribute>()?.UnderlyingObject?.Version).SafeToString(actualType.Assembly.GetName().Version.ToString());

            UserAgent = string.Format("{0}/{1} BeyovaCommon/{2} .NET/{3}", actualType.Name, version, EnvironmentCore.CommonComponentInfo?.Version, Environment.Version.ToString());
        }

        #endregion Constructor

        #region Util

        /// <summary>
        /// Creates the HTTP request raw.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="key">The key.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        protected HttpRequestRaw CreateHttpRequestRaw(string realm, string version, string httpMethod, string resourceName, string resourceAction, string key, Dictionary<string, string> queryString = null)
        {
            var result = new HttpRequestRaw
            {
                Uri = new Uri(GetRequestUri(Endpoint, realm, version, resourceName, resourceAction, key, queryString)),
                Method = httpMethod,
                ProtocolVersion = "1.1",
                Headers = new System.Collections.Specialized.NameValueCollection { { HttpConstants.HttpHeader.UserAgent, UserAgent } }
            };

            FillAdditionalData(result);
            return result;
        }

        /// <summary>
        /// Fills the additional data.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        protected void FillAdditionalData(HttpRequestRaw httpRequest)
        {
            if (httpRequest != null)
            {
                httpRequest.Headers.AddIfNotNullOrEmpty(HttpConstants.HttpHeader.TOKEN, Token);

                var currentApiContext = ContextHelper.ApiContext;

                httpRequest.Headers.AddIfNotNullOrEmpty(HttpConstants.HttpHeader.ORIGINAL, currentApiContext.IpAddress);
                httpRequest.Headers.AddIfNotNullOrEmpty(HttpConstants.HttpHeader.TRACEID, ApiTraceContext.TraceId);
                httpRequest.Headers.AddIfNotNullOrEmpty(HttpConstants.HttpHeader.TRACESEQUENCE, ApiTraceContext.TraceSequence.ToString());
                httpRequest.Headers.AddIfNotNullOrEmpty(HttpConstants.HttpHeader.OPERATOR, currentApiContext.IndicatedOperator.SafeToString(currentApiContext.CurrentCredential?.Key.ToString()));

                var userAgent = currentApiContext.UserAgent.SafeToString();
                if (!string.IsNullOrWhiteSpace(userAgent))
                {
                    httpRequest.Headers.AddIfNotNullOrEmpty(HttpConstants.HttpHeader.UserAgent, userAgent);
                }
            }
        }

        /// <summary>
        /// Gets the request endpoint.
        /// </summary>
        /// <param name="apiEndpoint">The API endpoint.</param>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <returns>System.String.</returns>
        protected internal static string GetRequestEndpoint(UriEndpoint apiEndpoint, string realm, string version)
        {
            var pathPrefix = string.IsNullOrWhiteSpace(realm) ? "/api/" : string.Format("/{0}/api/", realm.Trim());
            return string.Format("{0}{1}{2}/", (apiEndpoint ?? new UriEndpoint()).GetBaseUri(), pathPrefix, version);
        }

        /// <summary>
        /// Gets the request URI.
        /// </summary>
        /// <param name="apiEndpoint">The API endpoint.</param>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="key">The key.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        protected internal virtual string GetRequestUri(ApiEndpoint apiEndpoint, string realm, string version, string resourceName, string resourceAction, string key, Dictionary<string, string> queryString = null)
        {
            var url = string.Format("{0}{1}/{2}", GetRequestEndpoint(Endpoint, realm, version), resourceName, resourceAction).TrimEnd('/') + "/";
            if (!string.IsNullOrWhiteSpace(key))
            {
                url += (key.ToUrlEncodedText() + "/");
            }
            if (queryString.HasItem())
            {
                url += ("?" + queryString.ToKeyValuePairString(encodeKeyValue: true));
            }

            return url;
        }

        #endregion Util
    }
}