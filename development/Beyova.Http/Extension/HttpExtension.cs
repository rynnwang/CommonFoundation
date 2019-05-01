using Beyova.Diagnostic;
using Beyova.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace Beyova
{
    public static partial class HttpExtension
    {
        #region Ensure protocol

        /// <summary>
        /// The regex protocol
        /// </summary>
        private static Regex regexProtocol = new Regex(@"^(?<protocol>([a-zA-Z]+:\/\/))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Ensures the URL protocol.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="protocol">The protocol.</param>
        /// <returns></returns>
        public static string EnsureUrlProtocol(this string url, string protocol = HttpConstants.HttpProtocols.Http)
        {
            if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(protocol))
            {
                var match = regexProtocol.Match(url, 0);
                return string.Format("{0}://{1}", protocol, match.Success ? url.Substring(match.Length) : url);
            }

            return url;
        }

        #endregion Ensure protocol

        #region CreateHttpWebRequest

        /// <summary>
        /// Omits the remote certificate validation callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="certificate">The certificate.</param>
        /// <param name="chain">The chain.</param>
        /// <param name="sslPolicyErrors">The SSL policy errors.</param>
        /// <returns></returns>
        private static bool OmitRemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Creates the HTTP web request.
        /// </summary>
        /// <param name="destinationUrl">The destination URL.</param>
        /// <param name="httpMethod">Type of the method.</param>
        /// <param name="referrer">The referrer.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <param name="cookieContainer">The cookie container.</param>
        /// <param name="accept">The accept.</param>
        /// <param name="acceptGZip">The accept g zip.</param>
        /// <param name="omitServerCertificateValidation">The omit server certificate validation.</param>
        /// <returns>
        /// HttpWebRequest.
        /// </returns>
        public static HttpWebRequest CreateHttpWebRequest(this string destinationUrl, string httpMethod = HttpConstants.HttpMethod.Get, string referrer = null, string userAgent = null, CookieContainer cookieContainer = null, string accept = null, bool acceptGZip = true, bool omitServerCertificateValidation = false)
        {
            try
            {
                destinationUrl.CheckEmptyString(nameof(destinationUrl));

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(destinationUrl);
                httpWebRequest.KeepAlive = false;
                httpWebRequest.Accept = accept.SafeToString("*/*");

                if (omitServerCertificateValidation)
                {
                    httpWebRequest.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(OmitRemoteCertificateValidationCallback);
                }

                if (!string.IsNullOrWhiteSpace(referrer))
                {
                    httpWebRequest.Referer = referrer;
                }

                if (acceptGZip)
                {
                    httpWebRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.AcceptEncoding, HttpConstants.HttpValues.GZip);
                }

                if (!string.IsNullOrWhiteSpace(userAgent))
                {
                    httpWebRequest.UserAgent = userAgent;
                }

                httpWebRequest.CookieContainer = cookieContainer ?? new CookieContainer();
                httpWebRequest.Method = httpMethod.SafeToString(HttpConstants.HttpMethod.Get);

                return httpWebRequest;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { destinationUrl, httpMethod, referrer, userAgent });
            }
        }

        /// <summary>
        /// Creates the HTTP web request.
        /// </summary>
        /// <param name="uriObject">The URI object.</param>
        /// <param name="method">The method.</param>
        /// <returns>HttpWebRequest.</returns>
        public static HttpWebRequest CreateHttpWebRequest(this Uri uriObject, string method = HttpConstants.HttpMethod.Get)
        {
            if (uriObject != null)
            {
                return CreateHttpWebRequest(uriObject.ToString(), method);
            }

            return null;
        }

        #endregion CreateHttpWebRequest

        #region HttpRawMessage

        /// <summary>
        /// To the raw.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns>System.String.</returns>
        public static HttpRequestRaw ToHttpRawMessage(this HttpWebRequest httpRequest)
        {
            return httpRequest == null ? null : new HttpRequestRaw
            {
                Body = httpRequest.GetRequestStream().ReadStreamToBytes(),
                Headers = httpRequest.Headers,
                Method = httpRequest.Method,
                ProtocolVersion = httpRequest.ProtocolVersion.ToString(),
                Uri = httpRequest.RequestUri
            };
        }

        /// <summary>
        /// The header regex
        /// </summary>
        private static Regex headerRegex = new Regex(@"^(?<Key>([^\s\t\r\n]+)):(\s)*(?<Value>([^\t\r\n])+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// The destination URL regex
        /// </summary>
        private static Regex destinationUrlRegex = new Regex(@"(?<HttpMethod>([^\s\t\r\n]+))(\s)+(?<Url>([^\s\t\r\n]+))(\s)+(?<Protocol>(([^\s\t\r\n])+))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// To the HTTP raw message.
        /// </summary>
        /// <param name="raw">The raw.</param>
        /// <returns></returns>
        public static HttpRequestRaw ToHttpRawMessage(this string raw)
        {
            //SAMPLE:
            //GET https://www.telerik.com/UpdateCheck.aspx?isBeta=False HTTP/1.1
            //User-Agent: Fiddler/4.6.0.2 (.NET 4.0.30319.42000; WinNT 10.0.10240.0; zh-CN; 4xAMD64)
            //Pragma: no-cache
            //Host: www.telerik.com
            //Accept-Language: zh-CN
            //Referer: http://fiddler2.com/client/4.6.0.2
            //Accept-Encoding: gzip, deflate
            //Connection: close

            try
            {
                raw.CheckEmptyString(nameof(raw));
                raw.TrimStart();
                HttpRequestRaw result = null;

                var lines = raw.Split(StringConstants.NewLineCharacters);
                if (lines.Length < 1)
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(lines), lines);
                }

                //1st line must be destination info.
                string destinationString = lines[0];
                var destinationDetail = destinationUrlRegex.Match(destinationString);
                if (destinationDetail != null && destinationDetail.Success)
                {
                    result = new HttpRequestRaw
                    {
                        Method = destinationDetail.Result("${HttpMethod}").ToUpperInvariant(),
                        Uri = new Uri(destinationDetail.Result("${Url}")),
                        ProtocolVersion = destinationDetail.Result("${Protocol}")
                    };
                }
                else
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(destinationString), destinationString);
                }

                // skip blanklines between destination line and header lines
                int currentLineIndex = 1;
                for (; currentLineIndex < lines.Length;)
                {
                    if (!string.IsNullOrWhiteSpace(lines[currentLineIndex]))
                    {
                        break;
                    }
                    else
                    {
                        currentLineIndex++;
                    }
                }

                // Process headers
                for (; currentLineIndex < lines.Length; currentLineIndex++)
                {
                    if (string.IsNullOrWhiteSpace(lines[currentLineIndex]))
                    {
                        currentLineIndex++;
                        break;
                    }
                    else
                    {
                        var match = headerRegex.Match(lines[currentLineIndex]);

                        if (match.Success)
                        {
                            result.Headers.Merge(match.Result("${Key}"), match.Result("${Value}"));
                        }
                    }
                }

                // Process body
                StringBuilder builder = new StringBuilder(raw.Length);
                for (; currentLineIndex < lines.Length; currentLineIndex++)
                {
                    builder.AppendLine(lines[currentLineIndex]);
                }

                result.Body = Encoding.UTF8.GetBytes(builder.ToString());

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { raw });
            }
        }

        /// <summary>
        /// To the HTTP raw message.
        /// </summary>
        /// <param name="rawMessage">The raw message.</param>
        /// <returns></returns>
        public static HttpRequestMessage ToHttpRequestMessage(this HttpRequestRaw rawMessage)
        {
            if (rawMessage != null)
            {
                var request = new HttpRequestMessage(rawMessage.Method.ToHttpMethod(), rawMessage.Uri.SafeToString())
                {
                    Version = new Version(rawMessage.ProtocolVersion.SafeToString("1.1"))
                };

                foreach (var key in rawMessage.Headers.AllKeys)
                {
                    request.Headers.TryAddWithoutValidation(key, rawMessage.Headers.Get(key));
                }

                if (rawMessage.Body.HasItem())
                {
                    request.Content = new ByteArrayContent(rawMessage.Body);
                }

                return request;
            }

            return null;
        }

        /// <summary>
        /// To the HTTP web request.
        /// </summary>
        /// <param name="rawMessage">The raw message.</param>
        /// <returns></returns>
        public static HttpWebRequest ToHttpWebRequest(this HttpRequestRaw rawMessage)
        {
            if (rawMessage != null)
            {
                var request = (HttpWebRequest)HttpWebRequest.Create(rawMessage.Uri);
                request.Method = rawMessage.Method;
                request.ProtocolVersion = string.IsNullOrWhiteSpace(rawMessage.ProtocolVersion) ? null : new Version(rawMessage.ProtocolVersion);
                request.Headers.AddRange(rawMessage.Headers);

                if (rawMessage.Body.HasItem())
                {
                    request.ContentLength = rawMessage.Body.LongLength;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(rawMessage.Body, 0, rawMessage.Body.Length);
                    }
                }

                return request;
            }

            return null;
        }

        /// <summary>
        /// To the HTTP raw message.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static HttpRequestRaw ToHttpRawMessage(this HttpRequestMessage request)
        {
            if (request != null)
            {
                var raw = new HttpRequestRaw
                {
                    Method = request.Method.SafeToString(),
                    ProtocolVersion = request.Version.SafeToString(),
                    Uri = request.RequestUri,
                    Headers = request.Headers.ToNameValueCollection(),
                    Body = request.Content.ReadAsByteArrayAsync().Result
                };
            }

            return null;
        }

        #endregion HttpRawMessage

        #region Basic http Authorization

        /// <summary>
        /// The basic authorization prefix
        /// </summary>
        private const string basicAuthorizationPrefix = "Basic ";

        /// <summary>
        /// The basic authorization separator
        /// </summary>
        private static readonly char[] basicAuthorizationSeparator = new char[] { ':' };

        /// <summary>
        /// Gets the basic authentication.
        /// </summary>
        /// <param name="basicAuthorizationValue">The basic authorization value. E.g.: Basic iiwjke=</param>
        /// <returns></returns>
        public static HttpCredential GetBasicAuthentication(string basicAuthorizationValue)
        {
            return GetBasicAuthentication<HttpCredential>(basicAuthorizationValue);
        }

        /// <summary>
        /// Gets the basic authentication.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="basicAuthorizationValue">The basic authorization value.</param>
        /// <returns></returns>
        public static T GetBasicAuthentication<T>(string basicAuthorizationValue)
            where T : ICommonCredential, new()
        {
            T result = default(T);
            if (!string.IsNullOrWhiteSpace(basicAuthorizationValue))
            {
                if (basicAuthorizationValue.StartsWith(basicAuthorizationPrefix))
                {
                    basicAuthorizationValue = basicAuthorizationValue.Substring(basicAuthorizationPrefix.Length).DecodeBase64();
                    result = basicAuthorizationValue.ParseDomainCredentialToHttpCredential<T>();
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the basic authentication.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public static void SetBasicAuthentication(this NameValueCollection headers, string userName, string password)
        {
            if (headers != null)
            {
                headers.Set(HttpRequestHeader.Authorization.ToString(), basicAuthorizationPrefix + string.Format(HttpConstants.UserPasswordFormat, userName, password).EncodeBase64());
            }
        }

        #endregion Basic http Authorization

        /// <summary>
        /// Parses to dictonary.
        /// </summary>
        /// <param name="keyValuePairString">The key value pair string.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="keyEqualityComparer">The key equality comparer.</param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseToDictonary(this string keyValuePairString, char separator = '&', IEqualityComparer<string> keyEqualityComparer = null)
        {
            var result = new Dictionary<string, string>(keyEqualityComparer ?? StringComparer.OrdinalIgnoreCase);

            if (!string.IsNullOrWhiteSpace(keyValuePairString))
            {
                try
                {
                    var pairs = keyValuePairString.Split(separator);
                    foreach (var one in pairs)
                    {
                        if (!string.IsNullOrWhiteSpace(one))
                        {
                            var keyValuePair = one.Split(new char[] { '=' }, 2);

                            if (keyValuePair.Length == 2)
                            {
                                var key = keyValuePair[0];
                                var value = keyValuePair[1];

                                if (!string.IsNullOrWhiteSpace(key))
                                {
                                    result.Add(key, value.SafeToString());
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { keyValuePairString, separator = separator.ToString() });
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the HTTP status code to exception code.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="webExceptionStatus">The web exception status.</param>
        /// <returns></returns>
        public static ExceptionCode ConvertHttpStatusCodeToExceptionCode(this HttpStatusCode? httpStatusCode, WebExceptionStatus? webExceptionStatus)
        {
            return httpStatusCode.HasValue ?
                ConvertHttpStatusCodeToExceptionCode(httpStatusCode.Value, webExceptionStatus)
                : new ExceptionCode
                {
                    Major = ExceptionCode.MajorCode.HttpBlockError,
                    Minor = webExceptionStatus.HasValue ?
                        string.Format("{0}={1}", nameof(WebExceptionStatus), webExceptionStatus.Value)
                        : null
                };
        }

        /// <summary>
        /// Converts the HTTP status code to exception code.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="webExceptionStatus">The web exception status.</param>
        /// <returns>ExceptionCode.</returns>
        public static ExceptionCode ConvertHttpStatusCodeToExceptionCode(this HttpStatusCode httpStatusCode, WebExceptionStatus? webExceptionStatus)
        {
            ExceptionCode result = new ExceptionCode { Minor = webExceptionStatus == WebExceptionStatus.Success ? string.Empty : webExceptionStatus.ToString() };

            var statusCodeString = ((int)httpStatusCode).ToString();
            if (!(statusCodeString.StartsWith("4") || statusCodeString.StartsWith("5")))
            {
                return null;
            }

            switch (httpStatusCode)
            {
                case HttpStatusCode.BadRequest://400
                    result.Major = ExceptionCode.MajorCode.NullOrInvalidValue;
                    break;

                case HttpStatusCode.Unauthorized://401
                    result.Major = ExceptionCode.MajorCode.UnauthorizedOperation;
                    break;

                case HttpStatusCode.PaymentRequired://402
                    result.Major = ExceptionCode.MajorCode.CreditNotAfford;
                    break;

                case HttpStatusCode.Forbidden://403
                    result.Major = ExceptionCode.MajorCode.OperationForbidden;
                    break;

                case HttpStatusCode.NotFound: //404
                    result.Major = ExceptionCode.MajorCode.ResourceNotFound;
                    break;

                case HttpStatusCode.Conflict: //409
                    result.Major = ExceptionCode.MajorCode.DataConflict;
                    break;

                case HttpStatusCode.InternalServerError: //500
                    result.Major = ExceptionCode.MajorCode.OperationFailure;
                    break;

                case HttpStatusCode.NotImplemented: //501
                    result.Major = ExceptionCode.MajorCode.NotImplemented;
                    break;

                case HttpStatusCode.ServiceUnavailable: //503
                    result.Major = ExceptionCode.MajorCode.ServiceUnavailable;
                    break;

                case HttpStatusCode.HttpVersionNotSupported: //505
                    result.Major = ExceptionCode.MajorCode.Unsupported;
                    break;

                default:
                    result.Major = ExceptionCode.MajorCode.HttpBlockError;
                    break;
            }

            return result;
        }

        /// <summary>
        /// To the query string.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static NameValueCollection ToQueryString(this Uri uri)
        {
            return ParseToNameValueCollection(uri?.Query, '&', EncodingOrSecurityExtension.ToUrlDecodedText);
        }

        /// <summary>
        /// Updates the query string.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="updateActions">The update actions.</param>
        /// <returns></returns>
        public static Uri UpdateQueryString(this Uri uri, Action<Dictionary<string, string>> updateActions)
        {
            if (uri != null && updateActions != null)
            {
                var parameters = uri.Query.ParseToDictonary();
                updateActions(parameters);

                return new Uri(string.Format("{0}?{1}", uri.ToString().SubStringBeforeFirstMatch('?'), parameters.ToKeyValuePairString(encodeKeyValue: true)));
            }

            return uri;
        }

        /// <summary>
        /// Parses to key value pair collection.
        /// <remarks>
        /// Define separator = '&amp;',
        /// Parse string like a=1&amp;b=2&amp;c=3 into name value collection.
        /// Define separator = ';',
        /// Parse string like a=1;b=2;c=3 into name value collection.
        /// </remarks>
        /// </summary>
        /// <param name="keyValuePairString">The key value pair string.</param>
        /// <param name="separator">The separator. Default is '&amp;'.</param>
        /// <param name="caster">The caster.</param>
        /// <returns>
        /// System.Collections.Specialized.NameValueCollection.
        /// </returns>
        public static NameValueCollection ParseToNameValueCollection(this string keyValuePairString, char separator = '&', Func<string, string> caster = null)
        {
            var result = new NameValueCollection();

            if (!string.IsNullOrWhiteSpace(keyValuePairString))
            {
                try
                {
                    var pairs = keyValuePairString.Split(separator);
                    foreach (var one in pairs)
                    {
                        if (!string.IsNullOrWhiteSpace(one))
                        {
                            var keyValuePair = one.Split(new char[] { '=' }, 2);

                            if (keyValuePair.Length == 2)
                            {
                                var key = keyValuePair[0];
                                var value = keyValuePair[1];

                                if (!string.IsNullOrWhiteSpace(key))
                                {
                                    result.Set(key, caster == null ? value : caster(value));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { keyValuePairString, separator = separator.ToString() });
                }
            }

            return result;
        }

        /// <summary>
        /// Fills the headers.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="headerCollection">The header collection.</param>
        internal static void FillHeaders(this StringBuilder builder, NameValueCollection headerCollection)
        {
            if (builder != null && headerCollection != null)
            {
                foreach (string key in headerCollection.Keys)
                {
                    builder.AppendLineWithFormat("{0}: {1}", key, headerCollection.Get(key));
                }
            }
        }

        /// <summary>
        /// To the raw.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal static string ToRaw<TRequest, TResponse>(this HttpContextContainer<TRequest, TResponse> context)
        {
            StringBuilder builder = new StringBuilder(512);

            if (context != null)
            {
                //Write destination
                builder.Append(context.HttpMethod);
                builder.Append(StringConstants.WhiteSpace);
                builder.Append(context.Url.PathAndQuery);
                builder.Append(StringConstants.WhiteSpace);
                builder.Append(context.NetworkProtocol);
                builder.AppendLine();
                builder.AppendLine();

                //Write headers
                FillHeaders(builder, context.RequestHeaders);

                builder.AppendLine();

                if (context.HttpMethod.IsInString(HttpConstants.HttpMethod.Post, HttpConstants.HttpMethod.Put))
                {
                    var bytes = context.RequestBodyStream.ReadStreamToBytes();
                    builder.AppendLine(Encoding.UTF8.GetString(bytes));
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        #region Fill Data On HttpWebRequest

        /// <summary>
        /// Fills the file data.
        /// <remarks>
        /// Reference: http://stackoverflow.com/questions/566462/upload-files-with-httpwebrequest-multipart-form-data
        /// </remarks>
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="fileCollection">The file collection.
        /// Key: file name. e.g.: sample.txt
        /// Value: file data in byte array.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public static void FillFileData(this HttpWebRequest httpWebRequest, NameValueCollection postData, Dictionary<string, byte[]> fileCollection, string paramName)
        {
            try
            {
                var boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

                httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                httpWebRequest.Method = "POST";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Credentials = CredentialCache.DefaultCredentials;

                using (var stream = new MemoryStream())
                {
                    var boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                    var formDataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

                    if (postData != null)
                    {
                        foreach (string key in postData.Keys)
                        {
                            var formItem = string.Format(formDataTemplate, key, postData[key]);
                            var formItemBytes = Encoding.UTF8.GetBytes(formItem);
                            stream.Write(formItemBytes, 0, formItemBytes.Length);
                        }
                    }

                    stream.Write(boundaryBytes, 0, boundaryBytes.Length);

                    const string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\n\r\n";

                    if (fileCollection != null)
                    {
                        foreach (var key in fileCollection.Keys)
                        {
                            var header = string.Format(headerTemplate, paramName, key);
                            var headerBytes = Encoding.UTF8.GetBytes(header);
                            stream.Write(headerBytes, 0, headerBytes.Length);

                            stream.Write(fileCollection[key], 0, fileCollection[key].Length);

                            stream.Write(boundaryBytes, 0, boundaryBytes.Length);
                        }
                    }

                    httpWebRequest.ContentLength = stream.Length;
                    stream.Position = 0;
                    var tempBuffer = new byte[stream.Length];
                    stream.Read(tempBuffer, 0, tempBuffer.Length);

                    using (var requestStream = httpWebRequest.GetRequestStream())
                    {
                        requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                    }

                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(fileCollection);
            }
        }

        /// <summary>
        /// Fills the file data.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="fileCollection">The file collection.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public static void FillFileData(this HttpRequestMessage httpRequest, NameValueCollection postData, Dictionary<string, byte[]> fileCollection, string paramName)
        {
            try
            {
                var boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

                httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.ContentType, "multipart/form-data; boundary=" + boundary);
                httpRequest.Method = HttpMethod.Post;

                using (var stream = new MemoryStream())
                {
                    httpRequest.Content = new StreamContent(stream);

                    var boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                    var formDataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

                    if (postData != null)
                    {
                        foreach (string key in postData.Keys)
                        {
                            var formItem = string.Format(formDataTemplate, key, postData[key]);
                            var formItemBytes = Encoding.UTF8.GetBytes(formItem);
                            stream.Write(formItemBytes, 0, formItemBytes.Length);
                        }
                    }

                    stream.Write(boundaryBytes, 0, boundaryBytes.Length);

                    const string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\n\r\n";

                    if (fileCollection != null)
                    {
                        foreach (var key in fileCollection.Keys)
                        {
                            var header = string.Format(headerTemplate, paramName, key);
                            var headerBytes = Encoding.UTF8.GetBytes(header);
                            stream.Write(headerBytes, 0, headerBytes.Length);

                            stream.Write(fileCollection[key], 0, fileCollection[key].Length);

                            stream.Write(boundaryBytes, 0, boundaryBytes.Length);
                        }
                    }

                    stream.Position = 0;
                    var tempBuffer = new byte[stream.Length];
                    stream.Read(tempBuffer, 0, tempBuffer.Length);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(fileCollection);
            }
        }

        /// <summary>
        /// Fills the file data on HTTP web request.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="fileFullName">Full name of the file.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <exception cref="OperationFailureException">FillFileData</exception>
        public static void FillFileData(this HttpWebRequest httpWebRequest, NameValueCollection postData, string fileFullName, string paramName)
        {
            if (httpWebRequest != null && !string.IsNullOrWhiteSpace(fileFullName))
            {
                try
                {
                    var fileData = File.ReadAllBytes(fileFullName);
                    var fileName = Path.GetFileName(fileFullName);

                    var fileCollection = new Dictionary<string, byte[]> { { fileName, fileData } };

                    FillFileData(httpWebRequest, postData, fileCollection, paramName);
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { fileFullName, paramName });
                }
            }
        }

        /// <summary>
        /// Fills post data on HttpWebRequest.
        /// </summary>
        /// <param name="httpWebRequest">The HttpWebRequest instance.</param>
        /// <param name="method">The method.</param>
        /// <param name="dataMappings">The data mappings.</param>
        /// <param name="encoding">The encoding.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, string method, Dictionary<string, string> dataMappings, Encoding encoding = null)
        {
            if (httpWebRequest != null)
            {
                httpWebRequest.Method = method;
                FillData(httpWebRequest, dataMappings, encoding);
            }
        }

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="dataMappings">The data mappings.</param>
        /// <param name="encoding">The encoding.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, Dictionary<string, string> dataMappings, Encoding encoding = null)
        {
            if (httpWebRequest != null)
            {
                var data = FormDataToBytes(dataMappings, encoding);

                httpWebRequest.ContentType = HttpConstants.ContentType.FormSubmit;
                httpWebRequest.ContentLength = data.Length;
                using (var stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
        }

        /// <summary>
        /// Internals the fill data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        private static void InternalFillData(this HttpWebRequest httpWebRequest, string method, byte[] data, string contentType)
        {
            if (httpWebRequest != null && data != null)
            {
                if (!string.IsNullOrWhiteSpace(method))
                {
                    httpWebRequest.Method = method;
                }

                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    httpWebRequest.ContentType = contentType;
                }

                httpWebRequest.ContentLength = data.Length;
                var dataStream = httpWebRequest.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();
            }
        }

        /// <summary>
        /// Internals the fill data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        private static void InternalFillData(this HttpWebRequest httpWebRequest, string method, Stream stream, string contentType)
        {
            if (httpWebRequest != null && stream != null)
            {
                if (!string.IsNullOrWhiteSpace(method))
                {
                    httpWebRequest.Method = method;
                }

                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    httpWebRequest.ContentType = contentType;
                }
                httpWebRequest.ContentLength = stream.Length;
                var dataStream = httpWebRequest.GetRequestStream();
                stream.CopyTo(dataStream);
                dataStream.Close();
            }
        }

        /// <summary>
        /// Internals the fill data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="data">The data.</param>
        /// <param name="encodingToByte">The encoding to byte.</param>
        /// <param name="contentType">Type of the content.</param>
        private static void InternalFillData(this HttpWebRequest httpWebRequest, string method, string data, Encoding encodingToByte, string contentType = HttpConstants.ContentType.Json)
        {
            byte[] byteArray = null;

            // DO NOT use String.IsNullOrWhiteSpace or String.IsNullOrEmpty. string.Empty is allowed to filled here.
            if (data != null)
            {
                byteArray = (encodingToByte ?? Encoding.UTF8).GetBytes(data);
            }

            InternalFillData(httpWebRequest, method, byteArray, contentType);
        }

        /// <summary>
        /// Fills the data on HTTP web request.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, string method, byte[] data, string contentType = HttpConstants.ContentType.Json)
        {
            InternalFillData(httpWebRequest, method, data, contentType);
        }

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, byte[] data, string contentType = HttpConstants.ContentType.Json)
        {
            InternalFillData(httpWebRequest, null, data, contentType);
        }

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="method">The method.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, Stream stream, string contentType = HttpConstants.ContentType.Json, string method = null)
        {
            InternalFillData(httpWebRequest, method, stream, contentType);
        }

        /// <summary>
        /// Fills the data on HTTP web request.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, string method, string data, string contentType = HttpConstants.ContentType.Json)
        {
            InternalFillData(httpWebRequest, method, data, null, contentType);
        }

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="contentType">Type of the content.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, string data, Encoding encoding = null, string contentType = null)
        {
            //data can be empty string or spaces, so only check null here.
            if (data != null)
            {
                InternalFillData(httpWebRequest, null, (encoding ?? Encoding.UTF8).GetBytes(data), contentType);
            }
        }

        /// <summary>
        /// Fills the data on HTTP web request.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="data">The data.</param>
        /// <param name="encodingToByte">The encoding to byte.</param>
        /// <param name="contentType">Type of the content.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, string method, string data, Encoding encodingToByte, string contentType = HttpConstants.ContentType.Json)
        {
            byte[] byteArray = null;

            if (!string.IsNullOrWhiteSpace(data))
            {
                byteArray = (encodingToByte ?? Encoding.UTF8).GetBytes(data);
            }

            InternalFillData(httpWebRequest, method, byteArray, contentType);
        }

        #endregion Fill Data On HttpWebRequest

        #region Read response

        /// <summary>
        /// Reads the response.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <returns></returns>
        public static HttpStatusCode ReadResponse(this HttpWebRequest httpWebRequest)
        {
            httpWebRequest.AllowAutoRedirect = false;
            var result = ReadResponseAsT<string>(httpWebRequest, null);
            return result.HttpStatusCode;
        }


        /// <summary>
        /// Reads the response as t.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="responseDelegate">The response delegate. If not specified, return default(T).</param>
        /// <returns></returns>
        /// <exception cref="Beyova.Diagnostic.HttpOperationException"></exception>
        private static HttpActionResult<T> ReadResponseAsT<T>(this HttpWebRequest httpWebRequest, Func<HttpWebResponse, T> responseDelegate)
        {
            WebResponse response = null;
            HttpWebResponse webResponse = null;

            try
            {
                httpWebRequest.CheckNullObject(nameof(httpWebRequest));

                if (httpWebRequest.ContentLength < 0
                    && httpWebRequest.Method.IsInValues(HttpConstants.HttpMethod.Post, HttpConstants.HttpMethod.Put))
                {
                    httpWebRequest.ContentLength = 0;
                }

                response = httpWebRequest.GetResponse();

                webResponse = response as HttpWebResponse;
                webResponse.CheckNullObject(nameof(webResponse));

                return new HttpActionResult<T>
                {
                    Headers = webResponse.Headers,
                    HttpStatusCode = webResponse.StatusCode,
                    Cookies = webResponse.Cookies,
                    Body = responseDelegate == null ? default(T) : responseDelegate(webResponse)
                };
            }
            catch (WebException webEx)
            {
                webResponse = webEx.Response as HttpWebResponse;
                string responseText = webResponse?.ReadAsText();

                throw new HttpOperationException(httpWebRequest.RequestUri.ToString(),
                    httpWebRequest.Method,
                    webEx.Message,
                    responseText,
                    webResponse?.StatusCode,
                    webEx.Status,
                    webResponse?.Headers?.Get(HttpConstants.HttpHeader.SERVERNAME));
            }
            catch (Exception ex)
            {
                throw new HttpOperationException(httpWebRequest.RequestUri.ToString(),
                    httpWebRequest.Method,
                    webResponse.StatusCode,
                    ex as BaseException,
                    webResponse.Headers?.Get(HttpConstants.HttpHeader.SERVERNAME));
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        /// <summary>
        /// Reads the response as redirection.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <returns></returns>
        public static HttpActionResult<string> ReadResponseAsRedirection(this HttpWebRequest httpWebRequest)
        {
            httpWebRequest.AllowAutoRedirect = false;
            return ReadResponseAsT<string>(httpWebRequest, (webResponse) =>
            {
                return (webResponse.StatusCode == HttpStatusCode.Redirect) ? HttpExtension.ReadAsText(webResponse, Encoding.UTF8, false) : null;
            });
        }

        /// <summary>
        /// Reads the response as text.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        /// <exception cref="OperationFailureException">ReadResponseAsText</exception>
        public static HttpActionResult<string> ReadResponseAsText(this HttpWebRequest httpWebRequest, Encoding encoding = null)
        {
            return ReadResponseAsT<string>(httpWebRequest, (webResponse) => { return webResponse.ReadAsText(encoding, false); });
        }

        /// <summary>
        /// Reads the response as object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <returns></returns>
        public static HttpActionResult<T> ReadResponseAsObject<T>(this HttpWebRequest httpWebRequest)
        {
            return ReadResponseAsT<T>(httpWebRequest, (webResponse) =>
            {
                return webResponse.ReadAsText(Framework.DefaultTextEncoding, true).TryConvertJsonToObject<T>();
            });
        }

        /// <summary>
        /// Reads the response as bytes.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <returns>
        /// System.Byte[].
        /// </returns>
        public static HttpActionResult<byte[]> ReadResponseAsBytes(this HttpWebRequest httpWebRequest)
        {
            return ReadResponseAsT<byte[]>(httpWebRequest, (webResponse) => { return InternalReadAsBytes(webResponse, true); });
        }

        #region WebResponse Extension

        /// <summary>
        /// Determines whether [is g zip] [the specified web response].
        /// </summary>
        /// <param name="webResponse">The web response.</param>
        /// <returns>System.Boolean.</returns>
        public static bool IsGZip(this WebResponse webResponse)
        {
            var contentEncoding = webResponse?.Headers?.Get(HttpConstants.HttpHeader.ContentEncoding);
            return contentEncoding != null && contentEncoding.Trim().Equals(HttpConstants.HttpValues.GZip, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Reads as text. This method would try to detect GZip and decode it correctly.
        /// </summary>
        /// <param name="webResponse">The web response.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="closeResponse">The close response.</param>
        /// <returns>System.String.</returns>
        public static string ReadAsText(this WebResponse webResponse, Encoding encoding = null, bool closeResponse = true)
        {
            try
            {
                return webResponse.IsGZip() ?
                    webResponse.InternalReadAsGZipText(encoding ?? Framework.DefaultTextEncoding, closeResponse)
                    : webResponse.InternalReadAsText(encoding, closeResponse);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { encoding = encoding?.EncodingName, closeResponse });
            }
        }

        /// <summary>
        /// Reads as text.
        /// </summary>
        /// <param name="webResponse">The web response.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="closeResponse">if set to <c>true</c> [close response].</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">ReadAsText</exception>
        private static string InternalReadAsText(this WebResponse webResponse, Encoding encoding, bool closeResponse)
        {
            string result = string.Empty;

            if (webResponse != null)
            {
                try
                {
                    using (var responseStream = webResponse.GetResponseStream())
                    {
                        var streamReader = new StreamReader(responseStream, (encoding ?? Encoding.UTF8), true);
                        result = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { encoding = encoding?.EncodingName, closeResponse });
                }
                finally
                {
                    if (closeResponse)
                    {
                        webResponse.Close();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Reads as GZip text.
        /// </summary>
        /// <param name="webResponse">The web response.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="closeResponse">if set to <c>true</c> [close response].</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">ReadAsGZipText</exception>
        private static string InternalReadAsGZipText(this WebResponse webResponse, Encoding encoding, bool closeResponse)
        {
            if (webResponse != null)
            {
                try
                {
                    using (var responseStream = webResponse.GetResponseStream())
                    {
                        using (var gZipStream = new GZipStream(responseStream, CompressionMode.Decompress))
                        {
                            return (encoding ?? Encoding.UTF8).GetString(gZipStream.ReadStreamToBytes(true));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { encoding = encoding?.EncodingName, closeResponse });
                }
                finally
                {
                    if (closeResponse)
                    {
                        webResponse.Close();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Reads as bytes.
        /// </summary>
        /// <param name="webResponse">The web response.</param>
        /// <param name="closeResponse">if set to <c>true</c> [close response].</param>
        /// <returns>System.Byte[].</returns>
        /// <exception cref="OperationFailureException">ReadAsBytes</exception>
        internal static byte[] InternalReadAsBytes(this WebResponse webResponse, bool closeResponse)
        {
            byte[] result = null;

            if (webResponse != null)
            {
                try
                {
                    using (Stream responseStream = webResponse.GetResponseStream())
                    {
                        result = responseStream.ReadStreamToBytes(true);
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { closeResponse });
                }
                finally
                {
                    if (closeResponse)
                    {
                        webResponse.Close();
                    }
                }
            }

            return result;
        }

        #endregion WebResponse Extension

        #endregion Read response

        /// <summary>
        /// Converts the cookie string to matrix.
        /// </summary>
        /// <param name="cookieString">The cookie string.</param>
        /// <returns></returns>
        public static MatrixList<string> ConvertCookieStringToMatrix(string cookieString)
        {
            MatrixList<string> result = new MatrixList<string>();

            if (!string.IsNullOrWhiteSpace(cookieString))
            {
                string[] cookieProperties = cookieString.Split(new char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var property in cookieProperties)
                {
                    string[] keyValue = property.Split(new char[] { '=' }, 2);
                    if (keyValue.Length >= 2)
                    {
                        result.Add(keyValue[0], keyValue[1]);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the cookie by string.
        /// </summary>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <param name="cookieString">The cookie string.</param>
        /// <param name="hostDomain">The host domain.</param>
        public static void SetCookieByString(this CookieCollection cookieCollection, string cookieString, string hostDomain)
        {
            if (cookieCollection != null && !string.IsNullOrWhiteSpace(cookieString))
            {
                string[] cookieProperties = cookieString.Split(new char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var property in cookieProperties)
                {
                    string[] keyValue = property.Split(new char[] { '=' }, 2);
                    if (keyValue.Length >= 2)
                    {
                        cookieCollection.Add(new Cookie(keyValue[0], keyValue[1], null, hostDomain));
                    }
                }
            }
        }

        /// <summary>
        /// Sets the cookie by string.
        /// </summary>
        /// <param name="cookieContainer">The cookie container.</param>
        /// <param name="cookieString">The cookie string.</param>
        /// <param name="hostDomain">The host domain.</param>
        public static void SetCookieByString(this CookieContainer cookieContainer, string cookieString, Uri hostDomain)
        {
            if (hostDomain != null && cookieContainer != null && !string.IsNullOrWhiteSpace(cookieString))
            {
                var cookieCollection = cookieContainer.GetCookies(hostDomain);
                cookieCollection.SetCookieByString(cookieString, hostDomain.Host);
            }
        }

        /// <summary>
        /// Determines whether [is mobile user agent].
        /// </summary>
        /// <param name="userAgent">The user agent.</param>
        /// <returns>
        ///   <c>true</c> if [is mobile user agent] [the specified user agent]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMobileUserAgent(this string userAgent)
        {
            return !string.IsNullOrWhiteSpace(userAgent) && (
                  userAgent.IndexOf("pad", StringComparison.InvariantCultureIgnoreCase) > -1
                      || userAgent.IndexOf("android", StringComparison.InvariantCultureIgnoreCase) > -1
                      || userAgent.IndexOf("phone", StringComparison.InvariantCultureIgnoreCase) > -1
                  );
        }

        /// <summary>
        /// Forms the data to bytes.
        /// </summary>
        /// <param name="dataMappings">The data mappings.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.Byte[].</returns>
        internal static byte[] FormDataToBytes(Dictionary<string, string> dataMappings, Encoding encoding = null)
        {
            var stringBuilder = new StringBuilder();
            if (dataMappings != null)
            {
                foreach (var key in dataMappings.Keys)
                {
                    var value = dataMappings[key] ?? string.Empty;
                    stringBuilder.Append(key + "=" + value.Trim() + "&");
                }
            }
            if (stringBuilder.Length > 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }

            return (encoding ?? Encoding.ASCII).GetBytes(stringBuilder.ToString());
        }

        /// <summary>
        /// Safes the set HTTP header.
        /// <remarks>This method would help you to set values for header, especially for those need to be set by property. But following items would be ignored.
        /// <list type="bullet"><item>host</item><item>connection</item><item>close</item><item>content-length</item><item>proxy-connection</item><item>range</item></list></remarks>
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="headerKey">The header key.</param>
        /// <param name="value">The value.</param>
        /// <param name="ignoreIfNullOrEmpty">if set to <c>true</c> [ignore if null or empty].</param>
        public static void SafeSetHttpHeader(this HttpWebRequest httpRequest, string headerKey, object value, bool ignoreIfNullOrEmpty = false)
        {
            if (httpRequest != null && !string.IsNullOrWhiteSpace(headerKey))
            {
                if (ignoreIfNullOrEmpty && (value == null || string.IsNullOrWhiteSpace(value.SafeToString())))
                {
                    return;
                }

                switch (headerKey.ToLowerInvariant())
                {
                    case "accept":
                        httpRequest.Accept = value.SafeToString();
                        break;

                    case "content-type":
                        httpRequest.ContentType = value.SafeToString();
                        break;

                    case "date":
                        httpRequest.Date = (DateTime)value;
                        break;

                    case "expect":
                        httpRequest.Expect = value.SafeToString();
                        break;

                    case "if-modified-since":
                        httpRequest.IfModifiedSince = (DateTime)value;
                        break;

                    case "referer":
                        httpRequest.Referer = value.SafeToString();
                        break;

                    case "transfer-encoding":
                        httpRequest.TransferEncoding = value.SafeToString();
                        break;

                    case "user-agent":
                        httpRequest.UserAgent = value.SafeToString();
                        break;

                    case "host":
                    case "connection":
                    case "close":
                    case "content-length":
                    case "proxy-connection":
                    case "range":
                        //do nothing
                        break;

                    default:
                        httpRequest.Headers[headerKey] = value.SafeToString();
                        break;
                }
            }
        }

        /// <summary>
        /// Tries the get header.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="headerKey">The header key.</param>
        /// <returns>System.String.</returns>
        public static string TryGetHeader(this HttpWebRequest httpRequest, string headerKey)
        {
            return httpRequest?.Headers?.Get(headerKey).SafeToString();
        }
    }
}