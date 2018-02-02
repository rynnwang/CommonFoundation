using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using Beyova.ApiTracking;
using Beyova.Cache;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class ApiHandlerBase.
    /// </summary>
    public abstract class ApiHandlerBase<TRequest, TResponse>
    {
        #region Protected static fields

        /// <summary>
        /// The built-in feature version keyword
        /// </summary>
        protected const string BuiltInFeatureVersionKeyword = "builtin";

        /// <summary>
        /// The default setting name
        /// </summary>
        protected const string defaultSettingName = "default";

        /// <summary>
        /// The json converters
        /// </summary>
        protected static readonly HashSet<JsonConverter> jsonConverters = new HashSet<JsonConverter>();

        /// <summary>
        /// The json converters
        /// </summary>
        internal static JsonConverter[] JsonConverters = null;

        /// <summary>
        /// The settings container
        /// </summary>
        internal static Dictionary<string, RestApiSettings> settingsContainer = new Dictionary<string, RestApiSettings>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The default rest API settings
        /// </summary>
        protected static RestApiSettings defaultRestApiSettings = null;

        /// <summary>
        /// Gets the default rest API settings.
        /// </summary>
        /// <value>The default rest API settings.</value>
        public static RestApiSettings DefaultRestApiSettings { get { return defaultRestApiSettings; } }

        #endregion Protected static fields

        #region Property

        /// <summary>
        /// Gets or sets the default settings.
        /// </summary>
        /// <value>The default rest settings.</value>
        public RestApiSettings DefaultSettings { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow options].
        /// </summary>
        /// <value><c>true</c> if [allow options]; otherwise, <c>false</c>.</value>
        public bool AllowOptions { get; protected set; }

        #endregion Property

        /// <summary>
        /// Specifies the global json serialization converters.
        /// </summary>
        /// <param name="converters">The converters.</param>
        public static void SpecifyGlobalJsonSerializationConverters(params JsonConverter[] converters)
        {
            if (converters != null && converters.Any())
            {
                jsonConverters.AddRange(converters);
                JsonConverters = jsonConverters.ToArray();
            }
        }

        /// <summary>
        /// Initializes static members of the <see cref="ApiHandlerBase" /> class.
        /// </summary>
        static ApiHandlerBase()
        {
            jsonConverters.Add(JsonExtension.IsoDateTimeConverter);
            JsonConverters = jsonConverters.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiHandlerBase" /> class.
        /// </summary>
        /// <param name="defaultApiSettings">The default API settings.</param>
        /// <param name="allowOptions">if set to <c>true</c> [allow options].</param>
        protected ApiHandlerBase(RestApiSettings defaultApiSettings, bool allowOptions = false)
        {
            // Ensure it is never null. Default values should be safe.
            DefaultSettings = defaultApiSettings ?? new RestApiSettings
            {
                TokenHeaderKey = HttpConstants.HttpHeader.TOKEN,
                ClientIdentifierHeaderKey = HttpConstants.HttpHeader.CLIENTIDENTIFIER,
                EnableContentCompression = true
            };

            DefaultSettings.Name = DefaultSettings.Name.SafeToString(defaultSettingName);
            DefaultSettings.ApiTracking = DefaultSettings.ApiTracking ?? Framework.ApiTracking;

            settingsContainer.Merge(DefaultSettings.Name, DefaultSettings, false);

            this.AllowOptions = allowOptions;

            if (defaultRestApiSettings == null)
            {
                defaultRestApiSettings = defaultApiSettings ?? new RestApiSettings();
            }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessHttpApiContextContainer(HttpApiContextContainer<TRequest, TResponse> context)
        {
            context.EntryStamp = DateTime.UtcNow;
            context.SetResponseHeader(HttpConstants.HttpHeader.SERVERENTRYTIME, context.EntryStamp.ToFullDateTimeTzString());

            var acceptEncoding = context.TryGetRequestHeader(HttpConstants.HttpHeader.AcceptEncoding).SafeToLower();

            try
            {
                //First of all, clean thread info for context.
                ContextHelper.Reinitialize();

                context.TraceId = context.TryGetRequestHeader(HttpConstants.HttpHeader.TRACEID);
                context.TraceSequence = context.TryGetRequestHeader(HttpConstants.HttpHeader.TRACESEQUENCE).ToNullableInt32();

                Prepare(context.Request);

                if (context.HttpMethod.Equals(HttpConstants.HttpMethod.Options, StringComparison.OrdinalIgnoreCase))
                {
                    if (this.AllowOptions)
                    {
                        //Return directly. IIS would append following headers by default, according to what exactly web.config have.
                        //Access-Control-Allow-Origin: *
                        //Access-Control-Allow-Headers: Content-Type
                        //Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS

                        context.SetResponseHeader(HttpConstants.HttpHeader.SERVEREXITTIME, DateTime.UtcNow.ToFullDateTimeTzString());

                        return;
                    }
                }

                // Authentication has already done inside ProcessRoute.
                var runtimeContext = ProcessRoute(context);
                runtimeContext.CheckNullObject(nameof(runtimeContext));
                context.RuntimeContext = runtimeContext;

                context.Settings = runtimeContext.Settings ?? DefaultSettings;

                // Fill basic context info.

                var userAgentHeaderKey = context.Settings?.OriginalUserAgentHeaderKey;
                var apiContext = ContextHelper.ApiContext;
                apiContext.UserAgent = string.IsNullOrWhiteSpace(userAgentHeaderKey) ? context.UserAgent : context.TryGetRequestHeader(userAgentHeaderKey).SafeToString(context.UserAgent);
                apiContext.IpAddress = context.TryGetRequestHeader(context.Settings?.OriginalIpAddressHeaderKey.SafeToString(HttpConstants.HttpHeader.ORIGINAL)).SafeToString(context.ClientIpAddress);
                apiContext.CurrentUri = context.Url;
                var httpAuthorizationValue = context.TryGetRequestHeader(HttpConstants.HttpHeader.Authorization).DecodeBase64();
                if (!string.IsNullOrWhiteSpace(httpAuthorizationValue))
                {
                    apiContext.HttpAuthorization = HttpExtension.GetBasicAuthentication(httpAuthorizationValue);
                }

                apiContext.CultureCode = context.QueryString.Get(HttpConstants.QueryString.Language).SafeToString(context.UserLanguages.SafeFirstOrDefault()).EnsureCultureCode();
                if (runtimeContext.OperationParameters?.EntitySynchronizationMode != null)
                {
                    apiContext.LastSynchronizedStamp = context.TryGetRequestHeader(runtimeContext.OperationParameters.EntitySynchronizationMode.IfModifiedSinceKey).ObjectToDateTime();
                }

                // Fill finished.
                if (string.IsNullOrWhiteSpace(runtimeContext.Realm) && runtimeContext.Version.Equals(ApiConstants.BuiltInFeatureVersionKeyword, StringComparison.OrdinalIgnoreCase))
                {
                    string contentType;
                    var buildInResult = ProcessBuiltInFeature(context, runtimeContext, context.IsLocal, out contentType);
                    PackageResponse(context, buildInResult, new RuntimeApiOperationParameters { ContentType = contentType }, null, acceptEncoding, runtimeContext.IsVoid ?? false, context.Settings);
                }
                else
                {
                    //Initialize additional header keys
                    if ((runtimeContext.OperationParameters?.CustomizedHeaderKeys).HasItem())
                    {
                        var currentApiContext = ContextHelper.ApiContext;

                        foreach (var one in runtimeContext.OperationParameters.CustomizedHeaderKeys)
                        {
                            currentApiContext.CustomizedHeaders.Merge(one, context.TryGetRequestHeader(one));
                        }
                    }

                    InitializeContext(context.Request, runtimeContext);

                    byte[] body = context.ReadRequestBody();
                    context.CollectTrackingInfo(body.LongLength);

                    if (runtimeContext.Exception != null)
                    {
                        throw runtimeContext.Exception;
                    }

                    ApiTraceContext.Enter(runtimeContext, setNameAsMajor: true);
                    string jsonBody = null;

                    try
                    {
                        if (runtimeContext.ApiCacheStatus == ApiCacheStatus.UseCache)
                        {
                            jsonBody = runtimeContext.CachedResponseBody;
                        }
                        else
                        {
                            var invokeResult = Invoke(runtimeContext.ApiInstance, runtimeContext.ApiMethod, context, runtimeContext.EntityKey, out jsonBody);

                            if (runtimeContext.ApiCacheStatus == ApiCacheStatus.UpdateCache)
                            {
                                runtimeContext.ApiCacheContainer.Update(runtimeContext.ApiCacheIdentity, jsonBody);
                            }

                            PackageResponse(context, invokeResult, runtimeContext.OperationParameters, null, acceptEncoding, runtimeContext.IsVoid ?? false, settings: context.Settings);
                        }
                    }
                    catch (Exception invokeEx)
                    {
                        context.BaseException = invokeEx.Handle(new { Url = context.RawUrl, Method = context.HttpMethod });
                        throw context.BaseException;
                    }
                    finally
                    {
                        if (context.ApiEvent != null && !string.IsNullOrWhiteSpace(jsonBody) && !(runtimeContext.OperationParameters?.IsDataSensitive ?? false))
                        {
                            context.ApiEvent.Content = jsonBody.Length > 50 ? ((jsonBody.Substring(0, 40) + "..." + jsonBody.Substring(jsonBody.Length - 6, 6))) : jsonBody;
                        }

                        ApiTraceContext.Exit(context.BaseException?.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                var apiTracking = context.Settings?.ApiTracking;
                context.BaseException = (ex as BaseException) ?? ex.Handle(new { Uri = context.Url.SafeToString() });

                (apiTracking ?? Framework.ApiTracking)?.LogException(context.BaseException.ToExceptionInfo(eventKey: context.ApiEvent?.Key.SafeToString()));

                if (context.ApiEvent != null)
                {
                    context.ApiEvent.ExceptionKey = context.BaseException.Key;
                }

                PackageResponse(context, null, null, context.BaseException, acceptEncoding, settings: context.Settings);
            }
            finally
            {
                if (context.Settings?.ApiTracking != null)
                {
                    var exitStamp = DateTime.UtcNow;
                    if (context.ApiEvent != null)
                    {
                        try
                        {
                            context.ApiEvent.ExitStamp = exitStamp;
                            context.Settings.ApiTracking.LogApiEvent(context.ApiEvent);
                        }
                        catch { }
                    }

                    if (ApiTraceContext.Root != null)
                    {
                        try
                        {
                            ApiTraceContext.Exit(context.BaseException?.Key, exitStamp);
                            context.Settings.ApiTracking.LogApiTraceLog(ApiTraceContext.GetCurrentTraceLog(true));
                        }
                        catch { }
                    }
                }

                Dispose();
            }
        }

        /// <summary>
        /// Processes the built in feature.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="runtimeContext">The runtime context.</param>
        /// <param name="isLocalhost">if set to <c>true</c> [is localhost].</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        protected virtual object ProcessBuiltInFeature(HttpApiContextContainer<TRequest, TResponse> context, RuntimeContext runtimeContext, bool isLocalhost, out string contentType)
        {
            object result = null;
            contentType = HttpConstants.ContentType.Json;

            switch (runtimeContext?.ResourceName.SafeToLower())
            {
                case "server":
                    result = Framework.AboutService();
                    break;

                case "secured-key":
                    result = Framework.AboutService();
                    break;

                case "machine":
                    result = SystemManagementExtension.GetMachineHealth();
                    break;

                case "cache":
                    result = CacheRealm.GetSummary();
                    break;

                case "clearmemorycache":
                    if (isLocalhost)
                    {
                        CacheRealm.ClearAll();
                        result = "Done.";
                    }
                    else
                    {
                        result = "This API is available at localhost machine.";
                    }
                    break;

                case "gravity":
                    result = Gravity.GravityShell.Host?.Info;
                    break;

                case "i18n":
                    result = GlobalCultureResourceCollection.Instance?.AvailableCultureInfo ?? new Collection<CultureInfo>();
                    break;

                case "mirror":
                    var apiContext = ContextHelper.ApiContext;
                    var headers = new Dictionary<string, string>();

                    foreach (var key in context.RequestAllHeaderKeys)
                    {
                        headers.Add(key, context.TryGetRequestHeader(key));
                    }

                    result = new
                    {
                        RawUrl = context.RawUrl,
                        HttpMethod = context.HttpMethod,
                        Headers = headers,
                        UserAgent = apiContext.UserAgent,
                        IpAddress = apiContext.IpAddress,
                        CultureCode = apiContext.CultureCode
                    };
                    break;

                case "assemblyhash":
                    result = EnvironmentCore.GetAssemblyHash();
                    break;

                case "dll":
                    var dllName = context.QueryString.Get("name");
                    if (!string.IsNullOrWhiteSpace(dllName) && context.HttpMethod.MeaningfulEquals(HttpConstants.HttpMethod.Post, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var dllPath = Path.Combine(EnvironmentCore.ApplicationBaseDirectory, dllName + ".dll");
                            if (File.Exists(dllPath))
                            {
                                result = File.ReadAllBytes(dllPath);
                                contentType = HttpConstants.ContentType.BinaryDefault;
                            }
                        }
                        catch { }
                    }
                    break;

                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        protected virtual void Dispose()
        {
            ThreadExtension.Clear();
        }

        #region Protected virtual methods

        /// <summary>
        /// Prepares the specified request.
        /// <remarks>
        /// This method would be called before <c>ProcessRoute</c>. It can be used to help you to do some preparation, such as get something from headers or cookie for later actions.
        /// ou can save them in Thread data so that you can get them later in <c>ProcessRoute</c>, <c>Invoke</c>, <c>PackageOutput</c> ,etc.
        /// If any exception is throw from this method, the process flow would be interrupted.
        /// </remarks></summary>
        /// <param name="request">The request.</param>
        protected virtual void Prepare(TRequest request)
        {
        }

        /// <summary>
        /// Initializes the context.
        /// <remarks>
        /// This method would be called after <c>ProcessRoute</c> and before <c>Invoke</c>. It can be used to help you to do some context initialization, such as get something from database for later actions.
        /// ou can save them in Thread data so that you can get them later in <c>Invoke</c>, <c>PackageOutput</c> ,etc.
        /// If any exception is throw from this method, the process flow would be interrupted.
        /// </remarks>
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="runtimeContext">The runtime context.</param>
        protected virtual void InitializeContext(TRequest request, RuntimeContext runtimeContext)
        {
            //Do nothing here.
        }

        /// <summary>
        /// Invokes the specified method information.
        /// <remarks>
        /// Invoke action would regard to method parameter to use difference logic. Following steps show the IF-ELSE case. When it is hit, other would not go through.
        /// <list type="number"><item>
        /// If input parameter count is 0, invoke without parameter object.
        /// </item><item>
        /// If input parameter count is 1 and key is not empty or null, invoke using key.
        /// </item><item>
        /// If input parameter count is 1 and key is empty or null, invoke using key, try to get JSON object from request body and convert to object for invoke.
        /// </item><item>
        /// If input parameter count more than 1, try read JSON data to match parameters by name (ignore case) in root level, then invoke.
        /// </item></list></remarks></summary>
        protected virtual object Invoke(object instance, MethodInfo methodInfo, HttpApiContextContainer<TRequest, TResponse> context, string key, out string jsonBody)
        {
            return InternalInvoke(instance, methodInfo, context.ReadRequestBody(), context.Url, key, out jsonBody);
        }

        /// <summary>
        /// Internals the invoke.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="bodyData">The body data.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="key">The key.</param>
        /// <param name="jsonBody">The json body.</param>
        /// <returns>System.Object.</returns>
        internal static object InternalInvoke(object instance, MethodInfo methodInfo, byte[] bodyData, Uri uri, string key, out string jsonBody)
        {
            var inputParameters = methodInfo.GetParameters();
            jsonBody = null;

            if (!string.IsNullOrWhiteSpace(key) && key.Contains('%'))
            {
                key = key.ToUrlDecodedText();
            }

            if (inputParameters.Length == 0)
            {
                return methodInfo.Invoke(instance, null);
            }
            else if (inputParameters.Length == 1)
            {
                if (!string.IsNullOrWhiteSpace(key) && (inputParameters[0].ParameterType == typeof(string) || inputParameters[0].ParameterType.IsValueType))
                {
                    return methodInfo.Invoke(instance, new object[] { ReflectionExtension.ConvertToObjectByType(inputParameters[0].ParameterType, key) });
                }
                else
                {
                    var json = jsonBody = (bodyData == null ? null : Encoding.UTF8.GetString(bodyData));
                    return methodInfo.Invoke(instance, new object[] { DeserializeJsonObject(json, inputParameters[0].ParameterType) });
                }
            }
            else
            {
                // Now inputParameters.Length must > 1.
                object[] parameters = new object[inputParameters.Length];

                // Query string would override values in body. So try body first.
                var json = jsonBody = (bodyData == null ? null : Encoding.UTF8.GetString(bodyData));
                var jsonObject = string.IsNullOrWhiteSpace(json) ? null : JObject.Parse(json);

                if (jsonObject != null)
                {
                    for (int i = 0; i < inputParameters.Length; i++)
                    {
                        var jTokenObject = jsonObject.GetProperty(inputParameters[i].Name);
                        parameters[i] = jTokenObject == null ? null : jTokenObject.ToObject(inputParameters[i].ParameterType);
                    }
                }

                // Now use Query string to set values
                var queryString = HttpUtility.ParseQueryString(uri.Query);

                if (queryString.Count > 0)
                {
                    for (var i = 0; i < inputParameters.Length; i++)
                    {
                        parameters[i] = ReflectionExtension.ConvertToObjectByType(inputParameters[i].ParameterType,
                            queryString.Get(inputParameters[i].Name));
                    }
                }

                // Use parameter to override agian at last.
                if (parameters[0] == null && !string.IsNullOrWhiteSpace(key))
                {
                    parameters[0] = ReflectionExtension.ConvertToObjectByType(inputParameters[0].ParameterType, key);
                }

                return methodInfo.Invoke(instance, parameters);
            }
        }

        /// <summary>
        /// Processes the route.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected abstract RuntimeContext ProcessRoute(HttpApiContextContainer<TRequest, TResponse> context);

        #endregion Protected virtual methods

        #region PackageResponse

        /// <summary>
        /// Packages the response.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The data.</param>
        /// <param name="operationParameters">The operation parameters.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="acceptEncoding">The accept encoding.</param>
        /// <param name="noBody">if set to <c>true</c> [no body].</param>
        /// <param name="settings">The settings.</param>
        public static void PackageResponse(HttpApiContextContainer<TRequest, TResponse> context, object data, RuntimeApiOperationParameters operationParameters, BaseException ex = null, string acceptEncoding = null, bool noBody = false, RestApiSettings settings = null)
        {
            if (context != null)
            {
                if (settings == null)
                {
                    settings = defaultRestApiSettings;
                }

                var objectToReturn = ex != null ? (settings.OmitExceptionDetail ? ex.ToSimpleExceptionInfo() : ex.ToExceptionInfo()) : data;

                context.SetResponseHeader(HttpConstants.HttpHeader.SERVERNAME, EnvironmentCore.ServerName);
                context.SetResponseHeader(HttpConstants.HttpHeader.TRACEID, ApiTraceContext.TraceId);
                int httpStatusCode = (int)(ex == null ? (noBody ? HttpStatusCode.NoContent : HttpStatusCode.OK) : ex.Code.ToHttpStatusCode());

                if (ex == null && operationParameters?.EntitySynchronizationMode != null)
                {
                    DateTime? lastModifiedStamp = null;
                    data = operationParameters.EntitySynchronizationMode.RebuildOutputObject(ContextHelper.ApiContext.LastSynchronizedStamp, data, ref httpStatusCode, ref noBody, out lastModifiedStamp);

                    if (lastModifiedStamp.HasValue)
                    {
                        context.SetResponseHeader(operationParameters.EntitySynchronizationMode.LastModifiedKey, lastModifiedStamp.Value.ToFullDateTimeTzString());
                    }
                }

                context.ResponseStatusCode = (HttpStatusCode)httpStatusCode;

                if (!noBody)
                {
                    var contentType = (ex != null ? HttpConstants.ContentType.Json : operationParameters?.ContentType).SafeToString(HttpConstants.ContentType.Json);

                    bool isStreamBased = (objectToReturn != null && contentType.StartsWith("application/", StringComparison.OrdinalIgnoreCase) && objectToReturn.GetType() == typeof(byte[]));

                    if (isStreamBased)
                    {
                        // return as bytes;
                        context.WriteResponseBody(((byte[])objectToReturn).ToStream(), contentType);
                    }
                    else if (settings.EnableContentCompression)
                    {
                        acceptEncoding = acceptEncoding.SafeToString().ToLower();
                        var responseBytes = Framework.DefaultTextEncoding.GetBytes(contentType.Equals(HttpConstants.ContentType.Json, StringComparison.OrdinalIgnoreCase) ? objectToReturn.ToJson(true, JsonConverters) : objectToReturn.SafeToString());

                        if (acceptEncoding.Contains(HttpConstants.HttpValues.GZip))
                        {
                            context.WriteResponseGzipBody(responseBytes, contentType);
                        }
                        else if (acceptEncoding.Contains(HttpConstants.HttpValues.Deflate))
                        {
                            context.WriteResponseDeflateBody(responseBytes, contentType);
                        }
                        else
                        {
                            //return  as string;
                            context.WriteResponseBody(responseBytes, contentType);
                        }
                    }
                }

                context.SetResponseHeader(HttpConstants.HttpHeader.SERVEREXITTIME, DateTime.UtcNow.ToFullDateTimeTzString());
            }
        }

        #endregion PackageResponse

        /// <summary>
        /// Adds the setting.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="setting">The setting.</param>
        /// <param name="overrideIfExists">The override if exists.</param>
        /// <returns>System.Boolean.</returns>
        public static bool AddSetting(string name, RestApiSettings setting, bool overrideIfExists = false)
        {
            if (setting != null)
            {
                return settingsContainer.Merge(name.SafeToString(), setting, overrideIfExists);
            }

            return false;
        }

        /// <summary>
        /// Deserializes the json object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        protected static object DeserializeJsonObject(string value, Type type)
        {
            try
            {
                if (type == typeof(object))
                {
                    type = typeof(JToken);
                }
                return JsonConvert.DeserializeObject(value, type, JsonConverters);
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex, new { type = type.Name, value });
            }
        }

        /// <summary>
        /// Gets the name of the rest API setting by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="useDefaultIfNotFound">The use default if not found.</param>
        /// <returns>Beyova.RestApi.RestApiSettings.</returns>
        public static RestApiSettings GetRestApiSettingByName(string name, bool useDefaultIfNotFound = true)
        {
            RestApiSettings setting;
            return settingsContainer.TryGetValue(name.SafeToString(), out setting) ? setting : (useDefaultIfNotFound ? settingsContainer[defaultSettingName] : null);
        }
    }
}