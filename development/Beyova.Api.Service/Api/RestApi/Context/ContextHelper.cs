﻿using Beyova.Api.RestApi;
using Beyova.Diagnostic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Beyova
{
    /// <summary>
    /// Class ContextHelper.
    /// </summary>
    [FunctionInjectionHostTypeMap(typeof(Framework))]
    public static partial class ContextHelper
    {
        /// <summary>
        /// The thread key_ API context
        /// </summary>
        internal const string threadKey_ApiContext = "ApiContext";

        /// <summary>
        /// The thread key_ trace context
        /// </summary>
        internal const string threadKey_TraceContext = "TraceContext";

        /// <summary>
        /// Initializes the <see cref="ContextHelper"/> class.
        /// </summary>
        static ContextHelper()
        {
            FunctionInjectionController.AutoDiscoverFunctionInjection();
            //FunctionInjectionController.ApplyInjection(typeof(Framework), nameof(Framework.GetCurrentOperatorCredential), GetCurrentCredential);
            //FunctionInjectionController.ApplyInjection(typeof(Framework), nameof(Framework.CurrentCultureInfo), GetCurrentCredential);
            //Framework.ApplyInjection("GetCurrentOperatorCredential", GetCurrentCredential);
            //Framework.ApplyInjection("CurrentCultureInfo", GetCurrentCultureInfo);
        }

        /// <summary>
        /// Gets the current operator key.
        /// </summary>
        /// <returns>Guid.</returns>
        public static Guid GetCurrentOperatorKey()
        {
            var credential = ApiContext.CurrentCredential;
            credential.CheckNullObject(nameof(credential));
            credential.Key.CheckNullObject("credential.Key");

            return credential.Key.Value;
        }

        /// <summary>
        /// Gets the current user information.
        /// </summary>
        /// <value>The current user information.</value>
        public static IUserEssential CurrentUserInfo
        {
            get { return ApiContext.CurrentUserInfo; }
        }

        /// <summary>
        /// Gets the current culture code.
        /// Order: Query String[language] -> IUserInfo.Language -> CultureInfo.Current.
        /// </summary>
        /// <value>The current culture code.</value>
        public static CultureInfo CurrentCultureInfo
        {
            get
            {
                var code = (ApiContext.CurrentUserInfo?.CultureCode).SafeToString(ApiContext.CultureCode);
                return code.AsCultureInfo() ?? CultureInfo.CurrentUICulture ?? CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        /// Gets the current culture information.
        /// </summary>
        /// <returns></returns>
        [FunctionInjectionMap(nameof(Framework.GetCurrentCultureInfo))]
        public static CultureInfo GetCurrentCultureInfo()
        {
            return CurrentCultureInfo;
        }

        /// <summary>
        /// Gets the current credential.
        /// </summary>
        /// <value>The current credential.</value>
        public static BaseCredential CurrentCredential
        {
            get
            {
                return (ApiContext.CurrentCredential as BaseCredential) ?? ToBaseCredential(ApiContext.CurrentCredential);
            }
        }

        /// <summary>
        /// Gets the current raw URL.
        /// </summary>
        /// <returns></returns>
        [FunctionInjectionMap(nameof(Framework.GetCurrentRawUrl))]
        public static string GetCurrentRawUrl()
        {
            return CurrentRawUrl;
        }

        /// <summary>
        /// Gets the current raw URL.
        /// </summary>
        /// <value>
        /// The current raw URL.
        /// </value>
        public static string CurrentRawUrl
        {
            get
            {
                return ApiContext.CurrentUri?.ToString();
            }
        }

        /// <summary>
        /// Gets the current credential.
        /// </summary>
        /// <returns></returns>
        [FunctionInjectionMap(nameof(Framework.GetCurrentOperatorCredential))]
        public static BaseCredential GetCurrentCredential()
        {
            return CurrentCredential;
        }

        /// <summary>
        /// Gets the current credential.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetCurrentCredential<T>()
            where T : class, ICredential
        {
            return ApiContext.CurrentCredential as T;
        }

        /// <summary>
        /// Gets the current permissions.
        /// </summary>
        /// <value>The current permissions.</value>
        public static HashSet<string> CurrentPermissions
        {
            get { return (ApiContext.CurrentPermissionIdentifiers?.Permissions) ?? new System.Collections.Generic.HashSet<string>(); }
        }

        /// <summary>
        /// Determines whether the specified permission has permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns>
        ///   <c>true</c> if the specified permission has permission; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasPermission(string permission)
        {
            return !string.IsNullOrWhiteSpace(permission) && CurrentPermissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether [has any of permission] [the specified permissions].
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        /// <returns>
        ///   <c>true</c> if [has any of permission] [the specified permissions]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAnyOfPermission(params string[] permissions)
        {
            return permissions.HasItem() && CurrentPermissions.MatchAny(permissions);
        }

        /// <summary>
        /// Requireses the permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <exception cref="UnauthorizedOperationException">MissingPermission</exception>
        public static void RequiresPermission(string permission, FriendlyHint hint = null, [CallerMemberName] string memberName = null)
        {
            if (!HasPermission(permission))
            {
                throw new UnauthorizedOperationException("MissingPermission", data: new { requires = permission }, hint: hint, operationName: memberName);
            }
        }

        /// <summary>
        /// Gets the current full identifier.
        /// </summary>
        /// <value>The current full identifier.</value>
        public static string CurrentFullIdentifier
        {
            get
            {
                var credential = CurrentCredential;
                return string.Format("{0}[{1},TOKEN: {2}]", credential.Name, credential.Key, Token);
            }
        }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public static string IpAddress
        {
            get { return ApiContext.IpAddress; }
        }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public static string UserAgent
        {
            get { return ApiContext.UserAgent; }
        }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public static string Token
        {
            get { return ApiContext.Token; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is user.
        /// </summary>
        /// <value><c>true</c> if this instance is user; otherwise, <c>false</c>.</value>
        public static bool IsUser
        {
            get { return ApiContext.CurrentUserInfo != null; }
        }

        /// <summary>
        /// Gets or sets the API context.
        /// </summary>
        /// <value>The API context.</value>
        public static ApiContext ApiContext
        {
            get
            {
                var result = threadKey_ApiContext.GetThreadData() as ApiContext;
                if (result == null)
                {
                    result = new ApiContext();
                    threadKey_ApiContext.SetThreadData(result);
                }

                return result;
            }
        }

        #region ConsistContext

        /// <summary>
        /// Consists the context.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="setting">The setting.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <param name="cultureCode">The culture code.</param>
        /// <param name="currentUri">The current URI.</param>
        /// <param name="basicAuthentication">The basic authentication.</param>
        /// <param name="apiUniqueIdentifier">The API unique identifier.</param>
        internal static void ConsistContext(string token, RestApiSettings setting, string ipAddress, string userAgent, string cultureCode, Uri currentUri, HttpCredential basicAuthentication, ApiUniqueIdentifier apiUniqueIdentifier)
        {
            IRestApiEventHandlers restApiEventHandlers = setting?.EventHandlers;

            var apiContext = ContextHelper.ApiContext;

            apiContext.UserAgent = userAgent;
            apiContext.IpAddress = ipAddress;
            apiContext.CultureCode = cultureCode;
            apiContext.CurrentUri = currentUri;
            apiContext.HttpAuthorization = basicAuthentication;
            apiContext.UniqueIdentifier = apiUniqueIdentifier;

            if (restApiEventHandlers != null && !string.IsNullOrWhiteSpace(token))
            {
                apiContext.CurrentCredential = restApiEventHandlers.GetCredentialByToken(token);
                apiContext.Token = token;
            }
            else
            {
                apiContext.CurrentCredential = null;
                apiContext.Token = null;
            }
        }

        #endregion ConsistContext

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public static void Clear()
        {
            threadKey_ApiContext.SetThreadData(null);
            threadKey_TraceContext.SetThreadData(null);
        }

        /// <summary>
        /// Reinitializes this instance.
        /// </summary>
        public static void Reinitialize()
        {
            threadKey_ApiContext.SetThreadData(new ApiContext());
        }

        /// <summary>
        /// To the base credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>Beyova.BaseCredential.</returns>
        private static BaseCredential ToBaseCredential(ICredential credential)
        {
            return credential == null ? null : new BaseCredential { Key = credential.Key, Name = credential.Name };
        }
    }
}