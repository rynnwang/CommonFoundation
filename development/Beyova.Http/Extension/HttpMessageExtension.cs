using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Net;
using Beyova.ExceptionSystem;
using System.IO.Compression;
using System.IO;

namespace Beyova.Http
{
    /// <summary>
    /// class HttpMessageExtension
    /// </summary>
    public static class HttpMessageExtension
    {
        /// <summary>
        /// To the HTTP method.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <returns></returns>
        internal static HttpMethod ToHttpMethod(this string httpMethod)
        {
            return string.IsNullOrWhiteSpace(httpMethod) ? null : new HttpMethod(httpMethod.Trim());
        }

        /// <summary>
        /// Alls the keys.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IEnumerable<string> AllKeys(this HttpHeaders headers)
        {
            return headers?.Select(x => x.Key);
        }

        /// <summary>
        /// To the enumerable.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToEnumerable(this HttpHeaderValueCollection<StringWithQualityHeaderValue> collection)
        {
            return collection?.Select(x => x.ToString());
        }

        /// <summary>
        /// To the name value collection.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static NameValueCollection ToNameValueCollection(this HttpHeaders headers)
        {
            var result = new NameValueCollection();

            if (headers.HasItem())
            {
                foreach (var one in headers)
                {
                    result.Add(one.Key, one.Value.Join(seperator: StringConstants.SemicolonString));
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetValue(this HttpHeaders headers, string key)
        {
            return (headers != null && !string.IsNullOrWhiteSpace(key)) ? headers.GetValues(key).SafeFirstOrDefault() : null;
        }

        #region HttpClient

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetAsync(this HttpClient client, HttpRequestMessage request)
        {
            if (client != null && request != null)
            {
                return await client.GetAsync(request.RequestUri, HttpCompletionOption.ResponseContentRead);
            }

            return null;
        }

        /// <summary>
        /// Redirects the asynchronous.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static async Task<string> RedirectAsync(this HttpClient client, HttpRequestMessage request)
        {
            if (client != null && request != null)
            {
                return await client.GetAsync(request.RequestUri, HttpCompletionOption.ResponseHeadersRead).ContinueWith<string>(t =>
                {
                    using (t.Result)
                    {
                        if (t.Result.StatusCode == HttpStatusCode.Redirect)
                        {
                            return t.Result.Headers.GetValue(HttpConstants.HttpHeader.Location);
                        }

                        throw new HttpOperationException(request.RequestUri.ToString(), request.Method.SafeToString(), t.Result.StatusCode, null);
                    }
                });
            }

            return null;
        }

        /// <summary>
        /// Heads the asynchronous.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> HeadAsync(this HttpClient client, HttpRequestMessage request)
        {
            if (client != null && request != null)
            {
                request.Method = HttpMethod.Head;
                return await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            }

            return null;
        }

        /// <summary>
        /// Posts the asynchronous.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostAsync(this HttpClient client, HttpRequestMessage request)
        {
            if (client != null && request != null)
            {
                return await client.PostAsync(request.RequestUri, request.Content);
            }

            return null;
        }

        /// <summary>
        /// Puts the asynchronous.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PutAsync(this HttpClient client, HttpRequestMessage request)
        {
            if (client != null && request != null)
            {
                return await client.PutAsync(request.RequestUri, request.Content);
            }

            return null;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient client, HttpRequestMessage request)
        {
            if (client != null && request != null)
            {
                return await client.DeleteAsync(request.RequestUri);
            }

            return null;
        }

        /// <summary>
        /// Gets the response asynchronous.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetResponseAsync(this HttpClient client, HttpRequestMessage request)
        {
            if (client != null && request != null)
            {
                switch (request.Method.Method.ToUpperInvariant())
                {
                    case HttpConstants.HttpMethod.Get:
                        return await client.GetAsync(request);
                    case HttpConstants.HttpMethod.Post:
                        return await client.PostAsync(request);
                    case HttpConstants.HttpMethod.Put:
                        return await client.PutAsync(request);
                    case HttpConstants.HttpMethod.Delete:
                        return await client.DeleteAsync(request);
                    case HttpConstants.HttpMethod.Head:
                        return await client.HeadAsync(request);
                    default:
                        break;
                }
            }

            return null;
        }

        #endregion

        #region As Client

        private static HttpClient defaultSharedHttpClient = new HttpClient();

        /// <summary>
        /// Gets the response asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetResponseAsync(this HttpRequestMessage request, HttpClient client = null)
        {
            return await (client ?? defaultSharedHttpClient).GetResponseAsync(request);
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        public static HttpResponseMessage GetResponse(this HttpRequestMessage request, HttpClient client = null)
        {
            return GetResponseAsync(request, client).Result;
        }

        #endregion

        #region HttpResponseMessage

        /// <summary>
        /// Writes the response gzip body.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        public static void WriteResponseGzipBody(this HttpResponseMessage response, byte[] bytes, string contentType)
        {
            if (response != null && bytes != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var gzip = new GZipStream(ms, CompressionMode.Compress, true))
                    {
                        gzip.Write(bytes, 0, bytes.Length);
                    }
                    ms.Position = 0;
                    response.Content = new StreamContent(ms);
                    if (!string.IsNullOrWhiteSpace(contentType))
                    {
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    }
                    response.Content.Headers.ContentEncoding.Add(HttpConstants.HttpValues.GZip);
                }
            }
        }

        /// <summary>
        /// Writes the response deflate body.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        public static void WriteResponseDeflateBody(this HttpResponseMessage response, byte[] bytes, string contentType)
        {
            if (response != null && bytes != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var gzip = new DeflateStream(ms, CompressionMode.Compress, true))
                    {
                        gzip.Write(bytes, 0, bytes.Length);
                    }
                    ms.Position = 0;
                    response.Content = new StreamContent(ms);
                    if (!string.IsNullOrWhiteSpace(contentType))
                    {
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    }
                    response.Content.Headers.ContentEncoding.Add(HttpConstants.HttpValues.Deflate);
                }
            }
        }

        #endregion

        #region CreateHttpRequestMessage

        /// <summary>
        /// Creates the HTTP web request.
        /// </summary>
        /// <param name="destinationUrl">The destination URL.</param>
        /// <param name="httpMethod">Type of the method.</param>
        /// <param name="referrer">The referrer.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <param name="accept">The accept.</param>
        /// <param name="acceptGZip">The accept g zip.</param>
        /// <returns>
        /// HttpWebRequest.
        /// </returns>
        public static HttpRequestMessage CreateHttpRequestMessage(this string destinationUrl, string httpMethod = HttpConstants.HttpMethod.Get, string referrer = null, string userAgent = null, string accept = null, bool acceptGZip = true)
        {
            try
            {
                destinationUrl.CheckEmptyString(nameof(destinationUrl));

                var httpRequest = new HttpRequestMessage(httpMethod.ToHttpMethod(), destinationUrl);
                httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.AcceptEncoding, accept.SafeToString("*/*"));

                if (!string.IsNullOrWhiteSpace(referrer))
                {
                    httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.Referrer, referrer);
                }

                if (acceptGZip)
                {
                    httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.AcceptEncoding, HttpConstants.HttpValues.GZip);
                }

                if (!string.IsNullOrWhiteSpace(userAgent))
                {
                    httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.UserAgent, userAgent);
                }

                return httpRequest;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { destinationUrl, httpMethod, referrer, userAgent });
            }
        }

        #endregion CreateHttpRequestMessage

        #region FillData

        /// <summary>
        /// Internals the fill data.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        public static void FillData(this HttpRequestMessage httpRequest, byte[] data, string contentType)
        {
            if (httpRequest != null && data != null)
            {
                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    httpRequest.Headers.SafeSetHttpHeader(HttpConstants.HttpHeader.ContentType, contentType);
                }

                httpRequest.Content = new ByteArrayContent(data);
            }
        }

        /// <summary>
        /// Fills the json object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="jsonObject">The json object.</param>
        public static void FillJsonObject<T>(this HttpRequestMessage httpRequest, T jsonObject)
        {
            if (httpRequest != null && jsonObject != null)
            {
                httpRequest.Headers.SafeSetHttpHeader(HttpConstants.HttpHeader.ContentType, HttpConstants.ContentType.Json);

                httpRequest.Content = new ByteArrayContent(Framework.DefaultTextEncoding.GetBytes(jsonObject.ToJson(false)));
            }
        }

        #endregion

        #region Read response

        /// <summary>
        /// Reads the response as t.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="responseDelegate">The response delegate.</param>
        /// <returns></returns>
        private static async Task<HttpActionResult<T>> ReadResponseAsT<T>(this HttpRequestMessage httpRequest, Func<HttpResponseMessage, T> responseDelegate)
        {
            return await ReadResponseAsT<T>(httpRequest, null, responseDelegate);
        }

        /// <summary>
        /// Reads the response as t.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="responseDelegate">The response delegate.</param>
        /// <returns></returns>
        /// <exception cref="Beyova.ExceptionSystem.HttpOperationException">
        /// null - null
        /// or
        /// or
        /// </exception>
        private static async Task<HttpActionResult<T>> ReadResponseAsT<T>(this HttpRequestMessage httpRequest, HttpClient httpClient, Func<HttpResponseMessage, T> responseDelegate)
        {
            HttpActionResult<T> result = null;
            HttpResponseMessage httpResponse = null;
            string destinationMachine = null;

            try
            {
                httpRequest.CheckNullObject(nameof(httpRequest));
                httpResponse = await (httpClient ?? defaultSharedHttpClient).GetResponseAsync(httpRequest);
                destinationMachine = httpResponse.Headers?.GetValue(HttpConstants.HttpHeader.SERVERNAME);

                result = new HttpActionResult<T>
                {
                    Headers = httpResponse.Headers.ToNameValueCollection(),
                    HttpStatusCode = httpResponse.StatusCode,
                    Body = responseDelegate == null ? default(T) : responseDelegate(httpResponse)
                };

                return result;
            }
            catch (HttpRequestException httpEx)
            {
                throw new HttpOperationException(httpRequest.RequestUri.ToString(),
                        httpRequest.Method.ToString(),
                        httpEx.Message,
                        null,
                        result?.HttpStatusCode ?? HttpStatusCode.InternalServerError,
                        null,
                        destinationMachine);
            }
            catch (WebException webEx)
            {
                var webResponse = webEx.Response as HttpWebResponse;
                string responseText = null;
                if (webResponse != null)
                {
                    responseText = webResponse.ReadAsText();
                }

                throw new HttpOperationException(httpRequest.RequestUri.ToString(),
                       httpRequest.Method.ToString(),
                       webEx.Message,
                       responseText,
                        webResponse.StatusCode,
                       webEx.Status, destinationMachine);
            }
            catch (Exception ex)
            {
                throw new HttpOperationException(httpRequest.RequestUri.ToString(),
                    httpRequest.Method.ToString(),
                    result?.HttpStatusCode ?? HttpStatusCode.InternalServerError,
                    ex as BaseException, destinationMachine);
            }
            finally
            {
                if (httpResponse != null)
                {
                    httpResponse.Dispose();
                }
            }
        }

        /// <summary>
        /// Reads the response as redirection.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <returns></returns>
        public static async Task<string> ReadResponseAsRedirection(this HttpRequestMessage httpRequest, HttpClient httpClient = null)
        {
            return await httpClient.RedirectAsync(httpRequest);
        }

        /// <summary>
        /// Reads the response as text.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        /// <exception cref="OperationFailureException">ReadResponseAsText</exception>
        public static Task<HttpActionResult<string>> ReadResponseAsText(this HttpRequestMessage httpRequest, HttpClient httpClient = null, Encoding encoding = null)
        {
            return ReadResponseAsT<string>(
                httpRequest,
                httpClient,
                (response) =>
                {
                    return (encoding ?? Framework.DefaultTextEncoding).GetString(response.Content.ReadAsByteArrayAsync().Result);
                });
        }

        /// <summary>
        /// Reads the response as object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <returns></returns>
        public static async Task<HttpActionResult<T>> ReadResponseAsObject<T>(this HttpRequestMessage httpRequest, HttpClient httpClient = null)
        {
            return await ReadResponseAsT<T>(
                httpRequest,
                httpClient,
                (response) =>
                {
                    var bytes = response.Content.ReadAsByteArrayAsync().Result;
                    return bytes.HasItem() ? Framework.DefaultTextEncoding.GetString(bytes).TryConvertJsonToObject<T>() : default(T);
                });
        }

        /// <summary>
        /// Reads the response as bytes.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <returns></returns>
        public static async Task<HttpActionResult<byte[]>> ReadResponseAsBytes(this HttpRequestMessage httpRequest, HttpClient httpClient = null)
        {
            return await ReadResponseAsT<byte[]>(
               httpRequest,
               httpClient,
               (response) =>
               {
                   return response.Content.ReadAsByteArrayAsync().Result;
               });
        }

        #region HttpResponseMessage Extension

        /// <summary>
        /// Determines whether [is g zip] [the specified web response].
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <returns>
        /// System.Boolean.
        /// </returns>
        public static bool IsGZip(this HttpResponseMessage httpResponse)
        {
            var contentEncoding = httpResponse?.Headers?.GetValue(HttpConstants.HttpHeader.ContentEncoding);
            return contentEncoding != null && contentEncoding.Trim().Equals(HttpConstants.HttpValues.GZip, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Reads as text. This method would try to detect GZip and decode it correctly.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public static async Task<string> ReadAsText(this HttpResponseMessage httpResponse, Encoding encoding = null)
        {
            try
            {
                httpResponse.CheckNullObject(nameof(httpResponse));

                return httpResponse.IsGZip() ?
                     await httpResponse.InternalReadAsGZipText(encoding ?? Encoding.UTF8)
                    : await httpResponse.InternalReadAsText(encoding);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { encoding = encoding?.EncodingName });
            }
        }

        /// <summary>
        /// Reads as text.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        /// <exception cref="OperationFailureException">ReadAsText</exception>
        private static async Task<string> InternalReadAsText(this HttpResponseMessage httpResponse, Encoding encoding)
        {
            return await httpResponse.Content.ReadAsStreamAsync().ContinueWith<string>(t =>
            {
                try
                {
                    using (t.Result)
                    {
                        var streamReader = new StreamReader(t.Result, (encoding ?? Encoding.UTF8), true);
                        return streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { encoding = encoding?.EncodingName });
                }
            });
        }

        /// <summary>
        /// Reads as GZip text.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        /// <exception cref="OperationFailureException">ReadAsGZipText</exception>
        private static async Task<string> InternalReadAsGZipText(this HttpResponseMessage httpResponse, Encoding encoding)
        {
            return await httpResponse.Content.ReadAsStreamAsync().ContinueWith<string>(t =>
            {
                try
                {
                    using (t.Result)
                    {
                        using (var gZipStream = new GZipStream(t.Result, CompressionMode.Decompress))
                        {
                            return (encoding ?? Encoding.UTF8).GetString(gZipStream.ReadStreamToBytes(true));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { encoding = encoding?.EncodingName });
                }
            });
        }

        /// <summary>
        /// Reads as bytes.
        /// </summary>
        /// <param name="webResponse">The web response.</param>
        /// <param name="closeResponse">if set to <c>true</c> [close response].</param>
        /// <returns>System.Byte[].</returns>
        /// <exception cref="OperationFailureException">ReadAsBytes</exception>
        private static byte[] InternalReadAsBytes(this WebResponse webResponse, bool closeResponse)
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
        /// Safes the set HTTP header.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="headerKey">The header key.</param>
        /// <param name="value">The value.</param>
        /// <param name="ignoreIfNullOrEmpty">if set to <c>true</c> [ignore if null or empty].</param>
        internal static void SafeSetHttpHeader(this HttpHeaders headers, string headerKey, object value, bool ignoreIfNullOrEmpty = false)
        {
            if (headers != null && !string.IsNullOrWhiteSpace(headerKey))
            {
                if (ignoreIfNullOrEmpty && (value == null || string.IsNullOrWhiteSpace(value.SafeToString())))
                {
                    return;
                }

                var collectionValue = value as IEnumerable<string>;

                if (collectionValue.HasItem())
                {
                    headers.TryAddWithoutValidation(headerKey, collectionValue);
                }
                else
                {
                    headers.TryAddWithoutValidation(headerKey, value.SafeToString());
                }
            }
        }

        /// <summary>
        /// Safes the set HTTP header.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="headerKey">The header key.</param>
        /// <param name="value">The value.</param>
        /// <param name="ignoreIfNullOrEmpty">if set to <c>true</c> [ignore if null or empty].</param>
        public static void SafeSetHttpHeader(this HttpRequestMessage httpRequest, string headerKey, object value, bool ignoreIfNullOrEmpty = false)
        {
            SafeSetHttpHeader(httpRequest?.Headers, headerKey, value, ignoreIfNullOrEmpty);
        }

        /// <summary>
        /// Safes the set HTTP header.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="headerKey">The header key.</param>
        /// <param name="value">The value.</param>
        /// <param name="ignoreIfNullOrEmpty">if set to <c>true</c> [ignore if null or empty].</param>
        public static void SafeSetHttpHeader(this HttpResponseMessage httpResponse, string headerKey, object value, bool ignoreIfNullOrEmpty = false)
        {
            SafeSetHttpHeader(httpResponse?.Headers, headerKey, value, ignoreIfNullOrEmpty);
        }
    }
}
