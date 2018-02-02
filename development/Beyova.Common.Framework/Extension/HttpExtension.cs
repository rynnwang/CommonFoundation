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
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Beyova.Api.RestApi;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Extension class for http operations.
    /// </summary>
    static partial class HttpExtension
    {
        #region Uri and Credential      

        /// <summary>
        /// To the full raw URL.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns>System.String.</returns>
        public static string ToFullRawUrl(this HttpRequest httpRequest)
        {
            return httpRequest == null ? string.Empty : string.Format("{0}: {1}", httpRequest.HttpMethod, httpRequest.RawUrl);
        }

        #endregion Uri and Credential

        #region Fill Data On HttpWebRequest async

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
        public static async Task FillFileDataAsync(this HttpWebRequest httpWebRequest, NameValueCollection postData, Dictionary<string, byte[]> fileCollection, string paramName)
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

                    using (var requestStream = await httpWebRequest.GetRequestStreamAsync())
                    {
                        var task = requestStream.WriteAsync(tempBuffer, 0, tempBuffer.Length);
                        await task;

                        if (task.Exception == null)
                        {
                            await requestStream.FlushAsync();
                        }
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
        /// Fills the file data on HTTP web request.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="fileFullName">Full name of the file.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <exception cref="OperationFailureException">FillFileData</exception>
        public static async Task FillFileDataAsync(this HttpWebRequest httpWebRequest, NameValueCollection postData, string fileFullName, string paramName)
        {
            if (httpWebRequest != null && !string.IsNullOrWhiteSpace(fileFullName))
            {
                try
                {
                    var fileData = File.ReadAllBytes(fileFullName);
                    var fileName = Path.GetFileName(fileFullName);

                    var fileCollection = new Dictionary<string, byte[]> { { fileName, fileData } };

                    await FillFileDataAsync(httpWebRequest, postData, fileCollection, paramName);
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
        public static async Task FillDataAsync(this HttpWebRequest httpWebRequest, string method, Dictionary<string, string> dataMappings, Encoding encoding = null)
        {
            if (httpWebRequest != null)
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

                var data = (encoding ?? Encoding.ASCII).GetBytes(stringBuilder.ToString());

                httpWebRequest.Method = method;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = data.Length;
                using (var stream = await httpWebRequest.GetRequestStreamAsync())
                {
                    var task = stream.WriteAsync(data, 0, data.Length);
                    await task;

                    if (task.Exception == null)
                    {
                        await stream.FlushAsync();
                    }
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
        private static async Task InternalFillDataAsync(this HttpWebRequest httpWebRequest, string method, byte[] data, string contentType = "application/json")
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
                using (var dataStream = await httpWebRequest.GetRequestStreamAsync())
                {
                    var task = dataStream.WriteAsync(data, 0, data.Length);
                    await task;
                    if (task.Exception == null)
                    {
                        await dataStream.FlushAsync();
                    }
                }
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
        /// <returns>Task.</returns>
        private static async Task InternalFillDataAsync(this HttpWebRequest httpWebRequest, string method, string data, Encoding encodingToByte, string contentType = "application/json")
        {
            byte[] byteArray = null;

            if (!string.IsNullOrWhiteSpace(data))
            {
                byteArray = (encodingToByte ?? Encoding.UTF8).GetBytes(data);
            }

            await InternalFillDataAsync(httpWebRequest, method, byteArray, contentType);
        }

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        public static async Task FillDataAsync(this HttpWebRequest httpWebRequest, byte[] data, string contentType = "application/json")
        {
            await InternalFillDataAsync(httpWebRequest, null, data, contentType);
        }

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="contentType">Type of the content.</param>
        public static async Task FillDataAsync(this HttpWebRequest httpWebRequest, string data, Encoding encoding = null, string contentType = null)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                await InternalFillDataAsync(httpWebRequest, null, (encoding ?? Encoding.UTF8).GetBytes(data), contentType);
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
        public static async Task FillDataAsync(this HttpWebRequest httpWebRequest, string method, string data, Encoding encodingToByte, string contentType = "application/json")
        {
            byte[] byteArray = null;

            if (!string.IsNullOrWhiteSpace(data))
            {
                byteArray = (encodingToByte ?? Encoding.UTF8).GetBytes(data);
            }

            await InternalFillDataAsync(httpWebRequest, method, byteArray, contentType);
        }

        #endregion Fill Data On HttpWebRequest async

        #region Response Write

        /// <summary>
        /// Writes the content.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="closeStream">if set to <c>true</c> [close stream].</param>
        public static void WriteAllContent(this HttpListenerResponse httpResponse, string content, Encoding encoding = null, string contentType = "text/html", bool closeStream = true)
        {
            try
            {
                httpResponse.ContentType = contentType;

                var buffer = (encoding ?? Encoding.UTF8).GetBytes(content.SafeToString());
                httpResponse.ContentLength64 = buffer.Length;
                var output = httpResponse.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                if (closeStream)
                {
                    output.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Writes the content.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="closeStream">if set to <c>true</c> [close stream].</param>
        /// <exception cref="InvalidObjectException">WriteContent</exception>
        public static void WriteContent(this HttpResponseBase httpResponse, string content, Encoding encoding = null, bool closeStream = true)
        {
            try
            {
                var buffer = (encoding ?? Encoding.UTF8).GetBytes(content.SafeToString());
                var output = httpResponse.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                if (closeStream)
                {
                    output.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Writes the content.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="closeStream">if set to <c>true</c> [close stream].</param>
        /// <exception cref="InvalidObjectException">WriteContent</exception>
        public static void WriteContent(this HttpResponse httpResponse, string content, Encoding encoding = null, bool closeStream = true)
        {
            try
            {
                byte[] buffer = (encoding ?? Encoding.UTF8).GetBytes(content.SafeToString());
                Stream output = httpResponse.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                if (closeStream)
                {
                    output.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #endregion Response Write

        /// <summary>
        /// Expires all cookie.
        /// </summary>
        /// <param name="cookies">The cookies.</param>
        public static void ExpireAll(this CookieCollection cookies)
        {
            if (cookies != null)
            {
                foreach (Cookie one in cookies)
                {
                    one.Expired = true;
                    one.Expires = DateTime.UtcNow.AddDays(-1);
                }
            }
        }

        /// <summary>
        /// Automatics the cookie raw string.
        /// </summary>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <returns>System.String.</returns>
        public static string ToCookieRawString(this CookieCollection cookieCollection)
        {
            var builder = new StringBuilder();

            if (cookieCollection != null)
            {
                foreach (Cookie cookie in cookieCollection)
                {
                    builder.AppendFormat("{0}={1}; ", cookie.Name, cookie.Value);
                }
            }

            return builder.ToString();
        }



        /// <summary>
        /// Supply binary download via HttpResponse
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="physicalPath">The physical path.</param>
        /// <param name="originalFullFileName">Name of the original full file.</param>
        /// <exception cref="OperationFailureException">SupplyBinaryDownload</exception>
        public static void SupplyBinaryDownload(this HttpResponse response, string physicalPath, string originalFullFileName)
        {
            if (response != null && !string.IsNullOrWhiteSpace(physicalPath) && File.Exists(physicalPath))
            {
                var fs = new FileStream(physicalPath, FileMode.Open);

                try
                {
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    response.ContentType = HttpConstants.ContentType.BinaryDefault;
                    response.AddHeader(HttpConstants.HttpHeader.ContentDisposition, "attachment; filename=" + originalFullFileName.SafeToString("download").ToUrlEncodedText());
                    response.BinaryWrite(bytes);
                    response.Flush();
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { physicalPath, originalFullFileName });
                }
                finally
                {
                    fs.Close();
                    fs.Dispose();
                    response.End();
                }
            }
        }

        /// <summary>
        /// Supplies the binary download.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="physicalPath">The physical path.</param>
        /// <param name="originalFullFileName">Name of the original full file.</param>
        public static void SupplyBinaryDownload(this HttpContext context, string physicalPath, string originalFullFileName)
        {
            if (context != null)
            {
                SupplyBinaryDownload(context.Response, physicalPath, originalFullFileName);
            }
        }

        /// <summary>
        /// Parses to key value pair collection.
        /// </summary>
        /// <param name="keyValuePairString">The key value pair string.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>System.Collections.Generic.List&lt;System.Collections.Generic.KeyValuePair&lt;System.String, System.String&gt;&gt;.</returns>
        [Obsolete("Use ParseToDictonary instead")]
        public static List<KeyValuePair<string, string>> ParseToKeyValuePairCollection(this string keyValuePairString, char separator = '&')
        {
            var result = new List<KeyValuePair<string, string>>();

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
                                    result.Add(new KeyValuePair<string, string>(key, value.SafeToString()));
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
        /// Tries the get header.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="headerKey">The header key.</param>
        /// <returns>System.String.</returns>
        public static string TryGetHeader(this HttpRequest httpRequest, string headerKey)
        {
            return httpRequest?.Headers?.Get(headerKey).SafeToString();
        }

        /// <summary>
        /// Tries the get header.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="headerKey">The header key.</param>
        /// <returns>System.String.</returns>
        public static string TryGetHeader(this HttpRequestBase httpRequest, string headerKey)
        {
            return httpRequest?.Headers?.Get(headerKey).SafeToString();
        }

        #region Http proxy

        /// <summary>
        /// Copies the HTTP request to HTTP web request.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="destinationHost">The destination host.</param>
        /// <param name="rewriteDelegate">The rewrite delegate.</param>
        /// <returns>
        /// HttpWebRequest.
        /// </returns>
        public static HttpWebRequest CopyHttpRequestToHttpWebRequest(this HttpRequest httpRequest, string destinationHost, Func<NameValueCollection, NameValueCollection, Exception> rewriteDelegate = null)
        {
            if (httpRequest != null && !string.IsNullOrWhiteSpace(destinationHost))
            {
                var newUrl = string.Format("{0}{1}", destinationHost.TrimEnd('/'), httpRequest.Url.PathAndQuery);

                var destinationRequest = newUrl.CreateHttpWebRequest(httpRequest.HttpMethod);
                destinationRequest.Headers.Set(HttpConstants.HttpHeader.ORIGINAL, httpRequest.UserHostAddress);

                FillHttpRequestToHttpWebRequest(httpRequest, destinationRequest, rewriteDelegate);

                return destinationRequest;
            }

            return null;
        }

        /// <summary>
        /// The ignored headers
        /// </summary>
        public readonly static string[] ignoredHeaders = new string[] { HttpConstants.HttpHeader.TransferEncoding, HttpConstants.HttpHeader.AccessControlAllowHeaders, HttpConstants.HttpHeader.AccessControlAllowMethods, HttpConstants.HttpHeader.AccessControlAllowOrigin };

        /// <summary>
        /// Transports the HTTP response.
        /// </summary>
        /// <param name="sourceResponse">The source response.</param>
        /// <param name="destinationResponse">The destination response.</param>
        public static void TransportHttpResponse(this HttpWebResponse sourceResponse, HttpResponse destinationResponse)
        {
            if (sourceResponse != null && destinationResponse != null)
            {
                foreach (var key in sourceResponse.Headers.AllKeys)
                {
                    if (!key.IsInString(ignoredHeaders))
                    {
                        destinationResponse.SafeSetHttpHeader(key, sourceResponse.Headers.Get(key));
                    }
                }
                destinationResponse.StatusCode = (int)(sourceResponse.StatusCode);
                destinationResponse.StatusDescription = sourceResponse.StatusDescription;

                using (var sourceStream = sourceResponse.GetResponseStream())
                {
                    sourceStream.CopyTo(destinationResponse.OutputStream);
                    destinationResponse.Flush();
                }
            }
        }

        /// <summary>
        /// Transports the HTTP response.
        /// </summary>
        /// <param name="sourceResponse">The source response.</param>
        /// <param name="destinationResponse">The destination response.</param>
        public static void TransportHttpResponse(this WebResponse sourceResponse, HttpResponse destinationResponse)
        {
            if (sourceResponse != null && destinationResponse != null)
            {
                TransportHttpResponse((HttpWebResponse)sourceResponse, destinationResponse);
            }
        }

        /// <summary>
        /// Fills the HTTP request to HTTP web request.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="request">The request.</param>
        /// <param name="rewriteDelegate">The rewrite delegate.</param>
        public static void FillHttpRequestToHttpWebRequest(this HttpRequest httpRequest, HttpWebRequest request, Func<NameValueCollection, NameValueCollection, Exception> rewriteDelegate = null)
        {
            if (httpRequest != null && request != null)
            {
                //Copy header
                request.Headers.Clear();
                foreach (var key in httpRequest.Headers.AllKeys)
                {
                    request.SafeSetHttpHeader(key, httpRequest.Headers.Get(key));
                }

                if (rewriteDelegate != null)
                {
                    var exception = rewriteDelegate(request.Headers, httpRequest.Headers);
                    if (exception != null)
                    {
                        throw exception.Handle();
                    }
                }

                //Copy body, for PUT and POST only.
                if (httpRequest.HttpMethod == HttpConstants.HttpMethod.Put ||
                    httpRequest.HttpMethod == HttpConstants.HttpMethod.Post)
                {
                    var bytes = httpRequest.InputStream.ToBytes();
                    request.FillData(httpRequest.HttpMethod, bytes, httpRequest.ContentType);
                }
            }
        }

        /// <summary>
        /// Safes the set HTTP header.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="headerKey">The header key.</param>
        /// <param name="value">The value.</param>
        internal static void SafeSetHttpHeader(this HttpResponse httpResponse, string headerKey, object value)
        {
            if (httpResponse != null && !string.IsNullOrWhiteSpace(headerKey))
            {
                switch (headerKey.ToLowerInvariant())
                {
                    case "content-type":
                        httpResponse.ContentType = value.SafeToString();
                        break;

                    case "host":
                    //case "connection":
                    case "close":
                    case "content-length":
                    case "proxy-connection":
                    case "range":
                        //do nothing
                        break;

                    default:
                        httpResponse.Headers[headerKey] = value.SafeToString();
                        break;
                }
            }
        }

        /// <summary>
        /// Safes the set HTTP header.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="headerKey">The header key.</param>
        /// <param name="value">The value.</param>
        internal static void SafeSetHttpHeader(this HttpResponseBase httpResponse, string headerKey, object value)
        {
            if (httpResponse != null && !string.IsNullOrWhiteSpace(headerKey))
            {
                switch (headerKey.ToLowerInvariant())
                {
                    case "content-type":
                        httpResponse.ContentType = value.SafeToString();
                        break;

                    case "host":
                    //case "connection":
                    case "close":
                    case "content-length":
                    case "proxy-connection":
                    case "range":
                        //do nothing
                        break;

                    default:
                        httpResponse.Headers[headerKey] = value.SafeToString();
                        break;
                }
            }
        }

        #endregion Http proxy






    }
}