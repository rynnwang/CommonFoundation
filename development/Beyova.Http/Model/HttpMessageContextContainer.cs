using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace Beyova.Http
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpMessageContextContainer : HttpContextContainer<HttpRequestMessage, HttpResponseMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessageContextContainer" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="options">The options.</param>
        public HttpMessageContextContainer(HttpRequestMessage request, HttpResponseMessage response, HttpContextOptions<HttpRequestMessage> options)
            : base(request, response, options)
        {
        }

        /// <summary>
        /// Gets the request all header keys.
        /// </summary>
        /// <value>
        /// The request all header keys.
        /// </value>
        public override IEnumerable<string> RequestAllHeaderKeys
        {
            get { return this.Request.Headers.AllKeys(); }
        }

        /// <summary>
        /// Gets the raw URL.
        /// </summary>
        /// <value>
        /// The raw URL.
        /// </value>
        public override string RawUrl
        {
            get { return this.Request.RequestUri?.ToString(); }
        }

        /// <summary>
        /// Gets the user agent.
        /// </summary>
        /// <value>
        /// The user agent.
        /// </value>
        public override string UserAgent
        {
            get { return this.Request.Headers?.UserAgent?.ToString(); }
        }

        /// <summary>
        /// Gets the user languages.
        /// </summary>
        /// <value>
        /// The user languages.
        /// </value>
        public override IEnumerable<string> UserLanguages
        {
            get
            {
                return this.Request.Headers?.AcceptLanguage.ToEnumerable();
            }
        }

        /// <summary>
        /// Gets the network protocol.
        /// </summary>
        /// <value>
        /// The network protocol.
        /// </value>
        public override string NetworkProtocol
        {
            get { return this.Request.RequestUri?.Scheme; }
        }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <value>
        /// The query string.
        /// </value>
        public override NameValueCollection QueryString
        {
            get { return this.Request?.RequestUri.ToQueryString(); }
        }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        /// <value>
        /// The request headers.
        /// </value>
        public override NameValueCollection RequestHeaders
        {
            get { return this.Request.Headers.ToNameValueCollection(); }
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public override Uri Url
        {
            get { return this.Request.RequestUri; }
        }

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public override string HttpMethod
        {
            get { return this.Request.Method?.ToString(); }
        }

        /// <summary>
        /// Gets the request body stream.
        /// </summary>
        /// <value>
        /// The request body stream.
        /// </value>
        public override Stream RequestBodyStream
        {
            get { return this.Request.Content.ReadAsStreamAsync().Result; }
        }

        /// <summary>
        /// Gets or sets the response status code.
        /// </summary>
        /// <value>
        /// The response status code.
        /// </value>
        public override HttpStatusCode ResponseStatusCode
        {
            get { return this.Response.StatusCode; }
            set { this.Response.StatusCode = value; }
        }

        /// <summary>
        /// Reads the request body.
        /// </summary>
        /// <returns></returns>
        public override byte[] ReadRequestBody()
        {
            return this.Request?.Content.ReadAsByteArrayAsync().Result;
        }

        /// <summary>
        /// Removes the response header.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        public override void RemoveResponseHeader(string headerName)
        {
            if (!string.IsNullOrWhiteSpace(headerName))
            {
                this.Response?.Headers.Remove(headerName);
            }
        }

        /// <summary>
        /// Sets the response header.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        /// <param name="value">The value.</param>
        public override void SetResponseHeader(string headerName, string value)
        {
            if (!string.IsNullOrWhiteSpace(headerName))
            {
                this.Response?.Headers.TryAddWithoutValidation(headerName, value);
            }
        }

        /// <summary>
        /// Tries the get header.
        /// </summary>
        /// <param name="headerKey">The header key.</param>
        /// <returns></returns>
        public override string TryGetRequestHeader(string headerKey)
        {
            return (this.Request != null && !string.IsNullOrWhiteSpace(headerKey)) ? this.Request.Headers.GetValue(headerKey) : null;
        }

        /// <summary>
        /// Writes the response body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseBody(byte[] bytes, string contentType)
        {
            if (this.Response != null && bytes != null)
            {
                this.Response.Content = new ByteArrayContent(bytes);
                this.Response.Headers.SafeSetHttpHeader(HttpConstants.HttpHeader.ContentType, contentType, true);
            }
        }

        /// <summary>
        /// Writes the response body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseBody(Stream stream, string contentType)
        {
            if (this.Response != null && stream != null)
            {
                this.Response.Content = new StreamContent(stream);
                this.Response.Headers.SafeSetHttpHeader(HttpConstants.HttpHeader.ContentType, contentType, true);
            }
        }
    }
}
