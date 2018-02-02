using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Beyova.Http
{
    /// <summary>
    /// Class HttpRequestRaw
    /// </summary>
    public class HttpRequestRaw
    {
        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the protocol version.
        /// </summary>
        /// <value>
        /// The protocol version.
        /// </value>
        public string ProtocolVersion { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public byte[] Body { get; set; }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public NameValueCollection Headers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestRaw"/> class.
        /// </summary>
        public HttpRequestRaw()
        {
            this.Headers = new NameValueCollection();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(512);

            //Write destination
            builder.Append(this.Method);
            builder.Append(StringConstants.WhiteSpace);
            builder.Append(this.Uri.PathAndQuery);
            builder.Append(StringConstants.WhiteSpace);
            builder.Append(this.ProtocolVersion);
            builder.AppendLine();
            builder.AppendLine();

            //Write headers
            if (Headers.HasItem())
            {
                foreach (string key in Headers.Keys)
                {
                    builder.AppendLineWithFormat("{0}: {1}", key, Headers.Get(key));
                }
            }

            builder.AppendLine();

            if (Method.IsInString(HttpConstants.HttpMethod.Post, HttpConstants.HttpMethod.Put))
            {
                builder.AppendLine(this.Body.ToUtf8String());
            }

            builder.AppendLine();

            return builder.ToString();
        }
    }
}
