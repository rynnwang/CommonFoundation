using System;
using System.Net;
using System.Web;
using Beyova.Api.RestApi;
using Beyova.Diagnostic;
using Beyova.Http;

namespace Beyova
{
    /// <summary>
    /// Class ContextHelper.
    /// </summary>
    public static partial class ContextHelper
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
        /// <param name="currentUri">The current URI.</param>
        /// <param name="basicAuthentication">The basic authentication.</param>
        /// <param name="apiUniqueIdentifier">The API unique identifier.</param>
        internal static void ConsistContext(string token, string settingName, string ipAddress, string userAgent, string cultureCode, Uri currentUri, HttpCredential basicAuthentication, ApiUniqueIdentifier apiUniqueIdentifier)
        {
            ConsistContext(token, RestApiSettingPool.GetRestApiSettingByName(settingName, true), ipAddress, userAgent, cultureCode, currentUri, basicAuthentication, apiUniqueIdentifier);
        }

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        internal static void ConsistContext<TRequest, TResponse>(HttpContextContainer<TRequest, TResponse> context, RestApiSettings settings)
        {
            ConsistContext(
                GetValueCompatibly(context, context, HttpConstants.HttpHeader.TOKEN),
                settings,
                context.ClientIpAddress,
                context.UserAgent,
                context.QueryString.Get(HttpConstants.QueryString.Language).SafeToString(context?.GetCookieValue(HttpConstants.QueryString.Language)).SafeToString(context.UserLanguages.SafeFirstOrDefault()).EnsureCultureCode(),
                context.Url,
                HttpExtension.GetBasicAuthentication(context.TryGetRequestHeader(HttpConstants.HttpHeader.Authorization).DecodeBase64()),
                new ApiUniqueIdentifier
                {
                    HttpMethod = context.HttpMethod,
                    Path = context.Url.AbsolutePath
                }
                );
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
            return !string.IsNullOrWhiteSpace(key) ? (context?.TryGetRequestHeader(key).SafeToString(cookieActions?.GetCookieValue(key))) : null;
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
                    httpRequest.QueryString.Get(HttpConstants.QueryString.Language).SafeToString(httpRequest.Cookies.Get(HttpConstants.QueryString.Language)?.Value).SafeToString(httpRequest.UserLanguages.SafeFirstOrDefault()).EnsureCultureCode(),
                    httpRequest.Url,
                    HttpExtension.GetBasicAuthentication(httpRequest.Headers.Get(HttpConstants.HttpHeader.Authorization).DecodeBase64()),
                    new ApiUniqueIdentifier
                    {
                        HttpMethod = httpRequest.HttpMethod,
                        Path = httpRequest.Url.AbsolutePath
                    }
                    );
            }
        }

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="settings">The settings.</param>
        internal static void ConsistContext(HttpRequestBase httpRequest, RestApiSettings settings)
        {
            if (httpRequest != null && settings != null)
            {
                ConsistContext(
                    httpRequest.Headers.Get(HttpConstants.HttpHeader.TOKEN) ?? httpRequest.Cookies.TryGetValue(HttpConstants.HttpHeader.TOKEN),
                    settings,
                    httpRequest.UserHostAddress,
                    httpRequest.UserAgent,
                    httpRequest.QueryString.Get(HttpConstants.QueryString.Language).SafeToString(httpRequest.Cookies.Get(HttpConstants.QueryString.Language)?.Value).SafeToString(httpRequest.UserLanguages.SafeFirstOrDefault()).EnsureCultureCode(),
                    httpRequest.Url,
                    HttpExtension.GetBasicAuthentication(httpRequest.Headers.Get(HttpConstants.HttpHeader.Authorization).DecodeBase64()),
                    new ApiUniqueIdentifier
                    {
                        HttpMethod = httpRequest.HttpMethod,
                        Path = httpRequest.Url.AbsolutePath
                    }
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
                    httpRequest.QueryString.Get(HttpConstants.QueryString.Language).SafeToString(httpRequest.Cookies.Get(HttpConstants.QueryString.Language)?.Value).SafeToString(httpRequest.UserLanguages.SafeFirstOrDefault()).EnsureCultureCode(),
                    httpRequest.Url,
                    HttpExtension.GetBasicAuthentication(httpRequest.Headers.Get(HttpConstants.HttpHeader.Authorization).DecodeBase64()),
                    new ApiUniqueIdentifier
                    {
                        HttpMethod = httpRequest.HttpMethod,
                        Path = httpRequest.Url.AbsolutePath
                    }
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
                    httpRequest.QueryString.Get(HttpConstants.QueryString.Language).SafeToString(httpRequest.UserLanguages.SafeFirstOrDefault()).EnsureCultureCode(),
                    httpRequest.Url,
                    HttpExtension.GetBasicAuthentication(httpRequest.Headers.Get(HttpConstants.HttpHeader.Authorization).DecodeBase64()),
                    new ApiUniqueIdentifier
                    {
                        HttpMethod = httpRequest.HttpMethod,
                        Path = httpRequest.Url.AbsolutePath
                    }
                    );
            }
        }

        #endregion ConsistContext
    }
}