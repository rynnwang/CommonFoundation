﻿using System;
using System.Collections.Generic;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class ApiContext.
    /// </summary>
    public class ApiContext
    {
        /// <summary>
        /// Gets or sets the last synchronized stamp.
        /// </summary>
        /// <value>The last synchronized stamp.</value>
        public DateTime? LastSynchronizedStamp { get; set; }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public string UserAgent { get; internal set; }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public string IpAddress { get; internal set; }

        /// <summary>
        /// Gets the current URI.
        /// </summary>
        /// <value>The current URI.</value>
        public Uri CurrentUri { get; internal set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; internal set; }

        /// <summary>
        /// Gets or sets the current credential.
        /// </summary>
        /// <value>The current credential.</value>
        public ICredential CurrentCredential { get; set; }

        /// <summary>
        /// Gets or sets the HTTP authorization.
        /// </summary>
        /// <value>
        /// The HTTP authorization.
        /// </value>
        public HttpCredential HttpAuthorization { get; set; }

        /// <summary>
        /// Gets or sets the secure encryption key.
        /// </summary>
        /// <value>
        /// The secure encryption key.
        /// </value>
        public string SecureEncryptionKey { get; internal set; }

        /// <summary>
        /// Gets the current permission identifiers.
        /// </summary>
        /// <value>The current permission identifiers.</value>
        public IPermissionIdentifiers CurrentPermissionIdentifiers
        {
            get { return CurrentCredential as IPermissionIdentifiers; }
        }

        /// <summary>
        /// Gets or sets the current user information.
        /// </summary>
        /// <value>The current user information.</value>
        public IUserEssential CurrentUserInfo
        {
            get { return CurrentCredential as IUserEssential; }
        }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public List<string> Permissions { get; protected set; }

        /// <summary>
        /// Gets or sets the culture code.
        /// </summary>
        /// <value>The culture code.</value>
        public string CultureCode { get; set; }

        /// <summary>
        /// Gets or sets the customized headers.
        /// </summary>
        /// <value>The customized headers.</value>
        public Dictionary<string, string> CustomizedHeaders { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext"/> class.
        /// </summary>
        internal ApiContext()
        {
            Permissions = new List<string>();
            CustomizedHeaders = new Dictionary<string, string>();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return (CurrentCredential?.Name).SafeToString();
        }
    }
}