using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Web;
using Beyova.Api.RestApi;
using Beyova.Http;

namespace Beyova
{
    /// <summary>
    /// Class ContextHelper.
    /// </summary>
    static partial class ContextHelper
    {
        #region ConsistContext

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <param name="cultureCode">The culture code.</param>
        internal static void ConsistContext(string token, string settingName, string ipAddress, string userAgent, string cultureCode)
        {
            ConsistContext(token, ApiHandlerBase.GetRestApiSettingByName(settingName, true), ipAddress, userAgent, cultureCode);
        }

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="cookieActions">The cookie actions.</param>
        internal static void ConsistContext<TRequest, TResponse>(HttpContextContainer<TRequest, TResponse> context, string settingName = null, IHttpRequestCookieActions cookieActions = null)
        {
            ConsistContext(
                GetValueCompatibly(context, cookieActions, HttpConstants.HttpHeader.TOKEN),
                settingName,
                context.ClientIpAddress,
                context.UserAgent,
                 context.QueryString.Get(HttpConstants.QueryString.Language).SafeToString(cookieActions?.GetValue(HttpConstants.QueryString.Language)).SafeToString(context.UserLanguages.SafeFirstOrDefault()).EnsureCultureCode());
        }

        /// <summary>
        /// Gets the value compatibly.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="cookieActions">The cookie actions.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static string GetValueCompatibly<TRequest, TResponse>(HttpContextContainer<TRequest, TResponse> context, IHttpRequestCookieActions cookieActions, string key)
        {
            return !string.IsNullOrWhiteSpace(key) ? (context?.TryGetRequestHeader(key) ?? (cookieActions?.GetValue(key))) : null;
        }

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="settingName">Name of the setting.</param>
        internal static void ConsistContext(HttpRequestBase httpRequest, string settingName = null)
        {
            if (httpRequest != null)
            {
                ConsistContext(
                    httpRequest.Headers.Get(HttpConstants.HttpHeader.TOKEN) ?? httpRequest.Cookies.TryGetValue(HttpConstants.HttpHeader.TOKEN),
                    settingName,
                    httpRequest.UserHostAddress,
                    httpRequest.UserAgent,
                    httpRequest.QueryString.Get(HttpConstants.QueryString.Language).SafeToString(httpRequest.Cookies.Get(HttpConstants.QueryString.Language)?.Value).SafeToString(httpRequest.UserLanguages.SafeFirstOrDefault()).EnsureCultureCode()
                    );
            }
        }

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="settingName">Name of the setting.</param>
        internal static void ConsistContext(HttpRequest httpRequest, string settingName = null)
        {
            if (httpRequest != null)
            {
                ConsistContext(httpRequest.Headers.Get(HttpConstants.HttpHeader.TOKEN) ?? httpRequest.Cookies.TryGetValue(HttpConstants.HttpHeader.TOKEN),
                    settingName,
                    httpRequest.UserHostAddress,
                    httpRequest.UserAgent,
                    httpRequest.QueryString.Get(HttpConstants.QueryString.Language).SafeToString(httpRequest.Cookies.Get(HttpConstants.QueryString.Language)?.Value).SafeToString(httpRequest.UserLanguages.SafeFirstOrDefault()).EnsureCultureCode()
                    );
            }
        }

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="settingName">Name of the setting.</param>
        internal static void ConsistContext(HttpListenerRequest httpRequest, string settingName)
        {
            if (httpRequest != null)
            {
                ConsistContext(httpRequest.Headers.Get(HttpConstants.HttpHeader.TOKEN),
                    settingName,
                    httpRequest.UserHostAddress,
                    httpRequest.UserAgent,
                    httpRequest.QueryString.Get(HttpConstants.QueryString.Language).SafeToString(httpRequest.UserLanguages.SafeFirstOrDefault()).EnsureCultureCode()
                    );
            }
        }

        #endregion ConsistContext      
    }
}