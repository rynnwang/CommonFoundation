﻿using System;
using Beyova.Diagnostic;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class ApiContractAttribute. It is used to define interface which used as REST API.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ApiContractAttribute : Attribute, IApiContractOptions
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; protected set; }

        /// <summary>
        /// Gets or sets the name.
        /// This name is used for as service identifier in <see cref="ApiEventLog"/> model.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the realm.
        /// </summary>
        /// <value>The realm.</value>
        public string Realm { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContractAttribute" /> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="name">The name.</param>
        public ApiContractAttribute(string version, string name = null) : this(version, name, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContractAttribute" /> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="name">The name.</param>
        /// <param name="realm">The realm.</param>
        public ApiContractAttribute(string version, string name, string realm)
        {
            Version = version;
            Name = name;
            Realm = realm;
        }
    }
}