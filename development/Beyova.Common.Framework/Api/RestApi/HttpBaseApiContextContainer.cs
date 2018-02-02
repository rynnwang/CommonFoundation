using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Web;
using Beyova.Http;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class HttpBaseApiContextContainer : HttpApiContextContainer<HttpRequestBase, HttpResponseBase>
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
                return this.Request?.Headers?.AllKeys;
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
            get { return this.Request?.RawUrl; }
        }

        /// <summary>
        /// Gets the user agent.
        /// </summary>
        /// <value>
        /// The user agent.
        /// </value>
        public override string UserAgent { get { return this.Request?.UserAgent; } }

        /// <summary>
        /// Gets the user languages.
        /// </summary>
        /// <value>
        /// The user languages.
        /// </value>
        public override IEnumerable<string> UserLanguages { get { return this.Request?.UserLanguages; } }

        /// <summary>
        /// Gets the network protocol.
        /// </summary>
        /// <value>
        /// The network protocol.
        /// </value>
        public override string NetworkProtocol { get { return this.Request?.Url.Scheme; } }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <value>
        /// The query string.
        /// </value>
        public override NameValueCollection QueryString { get { return this.Request?.QueryString; } }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        /// <value>
        /// The request headers.
        /// </value>
        public override NameValueCollection RequestHeaders { get { return this.Request?.Headers; } }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public override Uri Url { get { return this.Request?.Url; } }

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public override string HttpMethod { get { return this.Request?.HttpMethod; } }

        /// <summary>
        /// Gets the request body stream.
        /// </summary>
        /// <value>
        /// The request body stream.
        /// </value>
        public override Stream RequestBodyStream
        {
            get { return this.Request?.InputStream; }
        }

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
                return (HttpStatusCode)this.Response.StatusCode;
            }
            set
            {
                this.Response.StatusCode = (int)value;
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpApiContextContainer" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="options">The options.</param>
        public HttpBaseApiContextContainer(HttpRequestBase request, HttpResponseBase response, HttpContextOptions<HttpRequestBase> options = null)
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
            return this.Request.TryGetHeader(headerKey);
        }

        /// <summary>
        /// Sets the response header.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        /// <param name="value">The value.</param>
        public override void SetResponseHeader(string headerName, string value)
        {
            this.Response.SafeSetHttpHeader(headerName, value);
        }

        /// <summary>
        /// Removes the response header.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        public override void RemoveResponseHeader(string headerName)
        {
            if (!string.IsNullOrWhiteSpace(headerName) && (this.Response?.Headers?.AllKeys?.Contains(headerName, false) ?? false))
            {
                this.Response.Headers.Remove(headerName);
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
            if (this.Response != null && stream != null)
            {
                stream.CopyStream(this.Response.OutputStream);

                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    this.Response.ContentType = contentType;
                }
            }
        }

        /// <summary>
        /// Writes the response gzip body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseGzipBody(byte[] bytes, string contentType)
        {
            if (this.Response != null)
            {
                this.Response.Filter = new System.IO.Compression.GZipStream(this.Response.Filter, System.IO.Compression.CompressionMode.Compress);
                this.Response.Headers.Remove(HttpConstants.HttpHeader.ContentEncoding);
                this.Response.AppendHeader(HttpConstants.HttpHeader.ContentEncoding, HttpConstants.HttpValues.GZip);
            }
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                this.Response.SafeSetHttpHeader(HttpConstants.HttpHeader.ContentType, contentType);
            }
            this.Response.OutputStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the response deflate body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseDeflateBody(byte[] bytes, string contentType)
        {
            if (this.Response != null)
            {
                this.Response.Filter = new System.IO.Compression.DeflateStream(this.Response.Filter, System.IO.Compression.CompressionMode.Compress);
                this.Response.Headers.Remove(HttpConstants.HttpHeader.ContentEncoding);
                this.Response.AppendHeader(HttpConstants.HttpHeader.ContentEncoding, HttpConstants.HttpValues.Deflate);
            }
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                this.Response.SafeSetHttpHeader(HttpConstants.HttpHeader.ContentType, contentType);
            }
            this.Response.OutputStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the response gzip body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseGzipBody(Stream stream, string contentType)
        {
            WriteResponseGzipBody(stream.ReadStreamToBytes(), contentType);
        }

        /// <summary>
        /// Writes the response deflate body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseDeflateBody(Stream stream, string contentType)
        {
            WriteResponseDeflateBody(stream.ReadStreamToBytes(), contentType);
        }

        /// <summary>
        /// Reads the request body.
        /// </summary>
        /// <returns></returns>
        public override byte[] ReadRequestBody()
        {
            return this.Request?.InputStream.ReadStreamToBytes(true);
        }
    }
}
