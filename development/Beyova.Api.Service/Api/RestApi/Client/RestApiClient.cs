using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using Beyova.ExceptionSystem;
using Beyova.Http;
using Newtonsoft.Json.Linq;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class RestApiClient.
    /// </summary>
    public abstract class RestApiClient : RestApiClientBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClient" /> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="acceptGZip">if set to <c>true</c> [accept g zip].</param>
        /// <param name="timeout">The timeout.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        public RestApiClient(ApiEndpoint endpoint, bool acceptGZip = false, int? timeout = null) : base(endpoint, acceptGZip, timeout)
        {
        }

        #endregion Constructor

        #region Common usage

        /// <summary>
        /// Invokes as void.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="key">The key.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="bodyJson">The body json.</param>
        /// <param name="methodNameForTrace">The method name for trace.</param>
        public void InvokeAsVoid(string realm, string version, string httpMethod, string resourceName, string resourceAction, string key = null, Dictionary<string, string> queryString = null, string bodyJson = null, [CallerMemberName] string methodNameForTrace = null)
        {
            InvokeAsJToken(realm, version, httpMethod, resourceName, resourceAction, key, queryString, bodyJson, this.Timeout, methodNameForTrace);
        }

        /// <summary>
        /// Invokes the specified HTTP method.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="key">The key.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="bodyJson">The body json.</param>
        /// <param name="methodNameForTrace">The method name for trace.</param>
        /// <returns>JToken.</returns>
        public JToken Invoke(string realm, string version, string httpMethod, string resourceName, string resourceAction, string key = null, Dictionary<string, string> queryString = null, string bodyJson = null, [CallerMemberName] string methodNameForTrace = null)
        {
            return InvokeAsJToken(realm, version, httpMethod, resourceName, resourceAction, key, queryString, bodyJson, this.Timeout, methodNameForTrace);
        }

        #endregion Common usage

        #region Programming Intelligence usage

        /// <summary>
        /// Invokes the using query string.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="methodNameForTrace">The method name for trace.</param>
        /// <returns>JToken.</returns>
        protected JToken InvokeUsingQueryString(string realm, string version, string httpMethod, string resourceName, string resourceAction, string parameter = null, [CallerMemberName] string methodNameForTrace = null)
        {
            return InvokeAsJToken(realm, version, httpMethod, resourceName, resourceAction, key: parameter, timeout: this.Timeout, methodNameForTrace: methodNameForTrace);
        }

        /// <summary>
        /// Invokes the using combined query string.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="methodNameForTrace">The method name for trace.</param>
        /// <returns>JToken.</returns>
        protected JToken InvokeUsingCombinedQueryString(string realm, string version, string httpMethod, string resourceName, string resourceAction, Dictionary<string, string> parameters, [CallerMemberName] string methodNameForTrace = null)
        {
            return InvokeAsJToken(realm, version, httpMethod, resourceName, resourceAction, queryString: parameters, timeout: this.Timeout, methodNameForTrace: methodNameForTrace);
        }

        /// <summary>
        /// Invokes the using body.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="methodNameForTrace">The method name for trace.</param>
        /// <returns>JToken.</returns>
        protected JToken InvokeUsingBody(string realm, string version, string httpMethod, string resourceName, string resourceAction, object parameter, [CallerMemberName] string methodNameForTrace = null)
        {
            return InvokeAsJToken(realm, version, httpMethod, resourceName, resourceAction, bodyJson: parameter.ToJson(), timeout: this.Timeout, methodNameForTrace: methodNameForTrace);
        }

        /// <summary>
        /// Invokes as j token.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="key">The key.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="bodyJson">The body json.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="methodNameForTrace">The method name for trace.</param>
        /// <returns>
        /// JToken.
        /// </returns>
        protected JToken InvokeAsJToken(string realm, string version, string httpMethod, string resourceName, string resourceAction, string key = null, Dictionary<string, string> queryString = null, string bodyJson = null, int? timeout = null, [CallerMemberName] string methodNameForTrace = null)
        {
            BaseException exception = null;

            try
            {
                ApiTraceContext.Enter("RestApiClient", methodNameForTrace);

                var httpRequestRaw = CreateHttpRequestRaw(realm, version, httpMethod, resourceName, resourceAction, key, queryString);
                if (httpMethod.IsInString(StringComparison.OrdinalIgnoreCase, HttpConstants.HttpMethod.Post, HttpConstants.HttpMethod.Put))
                {
                    httpRequestRaw.Body = Encoding.UTF8.GetBytes(bodyJson.SafeToString());
                }

                ApiTraceContext.WriteHttpRequestRaw(httpRequestRaw);
                var httpRequest = httpRequestRaw.ToHttpWebRequest();
                if (timeout.HasValue)
                {
                    httpRequest.Timeout = timeout.Value;
                }

                var response = httpRequest.ReadResponseAsText(Encoding.UTF8);

                if (response.HttpStatusCode == HttpStatusCode.NoContent)
                {
                    return null;
                }

                ApiTraceContext.WriteHttpResponseRaw(response);
                return response.Body;
            }
            catch (HttpOperationException httpEx)
            {
                ApiTraceContext.WriteHttpResponseRaw(new HttpActionResult<string> { Body = httpEx.ExceptionReference.ResponseText });

                ExceptionInfo externalExceptionInfo = JsonExtension.TryConvertJsonToObject<ExceptionInfo>(httpEx.ExceptionReference.ResponseText);

                exception = httpEx.Handle(new { httpMethod, resourceName, resourceAction, key, queryString, externalExceptionInfo });
                //Reset key to new one so that in log system, this exception can be identified correctly.
                exception.Key = Guid.NewGuid();
                throw exception;
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { httpMethod, resourceName, resourceAction, key, queryString });
                throw exception;
            }
            finally
            {
                ApiTraceContext.Exit(exception?.Key);
            }
        }

        #endregion Programming Intelligence usage
    }
}