﻿using System;

namespace Beyova
{
    /// <summary>
    /// Class UriEndpoint.
    /// </summary>
    public class UriEndpoint
    {
        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the path or path prefix. Example: "/path", "/path1/path2"
        /// </summary>
        /// <value>
        /// The path prefix.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int? Port { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance. Format: {Protocol}://{Host}:{Port?}{PathPrefix}/
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            var pathPrefix = (string.IsNullOrEmpty(Path) ? string.Empty : Path.Trim()).EnsureStartWith('/');
            return string.Format("{0}{1}", GetBaseUri(), pathPrefix);
        }

        /// <summary>
        /// Gets the base URI.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetBaseUri()
        {
            return this.Port.HasValue ?
                string.Format("{0}://{1}:{2}", Protocol.SafeToString(HttpConstants.HttpProtocols.Http), Host.SafeToString(HttpConstants.HttpValues.Localhost), Port.Value) :
                string.Format("{0}://{1}", Protocol.SafeToString(HttpConstants.HttpProtocols.Http), Host.SafeToString(HttpConstants.HttpValues.Localhost));
        }

        /// <summary>
        /// To the URI.
        /// </summary>
        /// <returns></returns>
        public virtual Uri ToUri()
        {
            return new Uri(this.ToString());
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="UriEndpoint"/> to <see cref="Uri"/>.
        /// </summary>
        /// <param name="uriEndpoint">The URI endpoint.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Uri(UriEndpoint uriEndpoint)
        {
            return uriEndpoint?.ToUri();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Uri"/> to <see cref="UriEndpoint"/>.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator UriEndpoint(Uri uri)
        {
            return uri == null ? null : new UriEndpoint
            {
                Host = uri.Host,
                Path = uri.AbsolutePath.TrimStart('/'),
                Port = uri.IsDefaultPort ? null : uri.Port as int?,
                Protocol = uri.Scheme
            };
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            UriEndpoint endpoint = obj as UriEndpoint;

            return endpoint != null
                && endpoint.Protocol.SafeEquals(this.Protocol, StringComparison.OrdinalIgnoreCase)
                && endpoint.Host.SafeEquals(this.Host, StringComparison.OrdinalIgnoreCase)
                && endpoint.Port.SafeEquals(this.Port)
                && endpoint.Path.MeaningfulEquals(this.Path, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Protocol.SafeGetHashCode() + this.Host.SafeGetHashCode() + this.Port.SafeGetHashCode() + this.Path.SafeGetHashCode();
        }
    }
}