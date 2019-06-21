using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Beyova.Http;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// class HttpApiMessageContextContainer
    /// </summary>
    public sealed class HttpApiMessageContextContainer : HttpApiContextContainer<HttpRequestMessage, HttpResponseMessage>
    {
        #region Abstract Properties

        /// <summary>
        /// Gets the request all header keys.
        /// </summary>
        /// <value>
        /// The request all header keys.
        /// </value>
        public override IEnumerable<string> RequestAllHeaderKeys
        {
            get
            {
                return HttpMessageExtension.AllKeys(Request?.Headers);
            }
        }

        /// <summary>
        /// Gets the raw URL.
        /// </summary>
        /// <value>
        /// The raw URL.
        /// </value>
        public override string RawUrl
        {
            get { return Request?.RequestUri?.ToString(); }
        }

        /// <summary>
        /// Gets the user agent.
        /// </summary>
        /// <value>
        /// The user agent.
        /// </value>
        public override string UserAgent { get { return Request?.Headers?.UserAgent?.ToString(); } }

        /// <summary>
        /// Gets the user languages.
        /// </summary>
        /// <value>
        /// The user languages.
        /// </value>
        public override IEnumerable<string> UserLanguages { get { return Request?.Headers?.AcceptLanguage?.ToEnumerable(); } }

        /// <summary>
        /// Gets the network protocol.
        /// </summary>
        /// <value>
        /// The network protocol.
        /// </value>
        public override string NetworkProtocol { get { return Request?.RequestUri?.Scheme; } }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <value>
        /// The query string.
        /// </value>
        public override NameValueCollection QueryString { get { return Request?.RequestUri.ToQueryString(); } }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        /// <value>
        /// The request headers.
        /// </value>
        public override NameValueCollection RequestHeaders { get { return Request?.Headers.ToNameValueCollection(); } }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public override Uri Url { get { return Request?.RequestUri; } }

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public override string HttpMethod { get { return Request?.Method?.ToString(); } }

        /// <summary>
        /// Gets or sets the response status code.
        /// </summary>
        /// <value>
        /// The response status code.
        /// </value>
        public override HttpStatusCode ResponseStatusCode
        {
            get
            {
                return Response.StatusCode;
            }
            set
            {
                Response.StatusCode = value;
            }
        }

        /// <summary>
        /// Gets the request body stream.
        /// </summary>
        /// <value>
        /// The request body stream.
        /// </value>
        public override Stream RequestBodyStream
        {
            get { return Request?.Content?.ReadAsStreamAsync().Result; }
        }

        #endregion Abstract Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpApiMessageContextContainer"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="options">The options.</param>
        public HttpApiMessageContextContainer(HttpRequestMessage request, HttpResponseMessage response, HttpContextOptions<HttpRequestMessage> options)
            : base(request, response, options)
        {
        }

        /// <summary>
        /// Tries the get header.
        /// </summary>
        /// <param name="headerKey">The header key.</param>
        /// <returns></returns>
        public override string TryGetRequestHeader(string headerKey)
        {
            return Request.Headers.GetValue(headerKey);
        }

        /// <summary>
        /// Sets the response header.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        /// <param name="value">The value.</param>
        public override void SetResponseHeader(string headerName, string value)
        {
            Response.SafeSetHttpHeader(headerName, value);
        }

        /// <summary>
        /// Removes the response header.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        public override void RemoveResponseHeader(string headerName)
        {
            if (!string.IsNullOrWhiteSpace(headerName) && (Response?.Headers?.AllKeys().Contains(headerName, StringComparer.Ordinal) ?? false))
            {
                Response.Headers.Remove(headerName);
            }
        }

        /// <summary>
        /// Writes the response body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseBody(byte[] bytes, string contentType)
        {
            if (bytes != null)
            {
                WriteResponseBody(bytes.ToStream(), contentType);
            }
        }

        /// <summary>
        /// Writes the response body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseBody(Stream stream, string contentType)
        {
            if (Response != null && stream != null)
            {
                Response.Content = new StreamContent(stream);
                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    Response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                }
                Response.Content.Headers.ContentEncoding.Add(HttpConstants.HttpValues.GZip);
            }
        }

        /// <summary>
        /// Writes the response gzip body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseGzipBody(byte[] bytes, string contentType)
        {
            Response.WriteResponseGzipBody(bytes, contentType);
        }

        /// <summary>
        /// Writes the response gzip body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseGzipBody(Stream stream, string contentType)
        {
            WriteResponseGzipBody(stream.ReadStreamToBytes(true), contentType);
        }

        /// <summary>
        /// Reads the request body.
        /// </summary>
        /// <returns></returns>
        public override byte[] ReadRequestBody()
        {
            return Request.Content.ReadAsByteArrayAsync().Result;
        }

        /// <summary>
        /// Writes the response deflate body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseDeflateBody(byte[] bytes, string contentType)
        {
            Response.WriteResponseDeflateBody(bytes, contentType);
        }

        /// <summary>
        /// Writes the response deflate body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseDeflateBody(Stream stream, string contentType)
        {
            WriteResponseDeflateBody(stream.ReadStreamToBytes(true), contentType);
        }

        /// <summary>
        /// Gets the cookie values.
        /// </summary>
        /// <param name="cookieKey">The cookie key.</param>
        /// <returns></returns>
        public override IEnumerable<string> GetCookieValues(string cookieKey)
        {
            List<string> result = new List<string>();
            if (!string.IsNullOrWhiteSpace(cookieKey))
            {
                var cookieString = Request.Headers.GetValue(HttpConstants.HttpHeader.Cookie);
                var cookieMatrix = HttpExtension.ConvertCookieStringToMatrix(cookieString);
                cookieMatrix.TryGetValue(cookieKey, out result);
            }

            return result?.Select(x => x.ToUrlDecodedText());
        }
    }
}