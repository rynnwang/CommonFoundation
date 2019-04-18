using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Beyova.Api.RestApi;
using Beyova.Diagnostic;

namespace Beyova.Api
{
    /// <summary>
    /// Class ApiRouteIdentifier.
    /// </summary>
    public class ApiRouteIdentifier : IApiContractOptions, ICloneable
    {
        /// <summary>
        /// Gets or sets the realm.
        /// </summary>
        /// <value>
        /// The realm.
        /// </value>
        public string Realm { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        /// <value>
        /// The resource.
        /// </value>
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public string HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the parameterized identifiers.
        /// </summary>
        /// <value>
        /// The parameterized identifiers.
        /// </value>
        public List<string> ParameterizedIdentifiers { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRouteIdentifier"/> class.
        /// </summary>
        public ApiRouteIdentifier()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRouteIdentifier"/> class.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="version">The version.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="action">The action.</param>
        internal ApiRouteIdentifier(string realm, string version, string resource, string httpMethod, string action) : this()
        {
            Realm = realm;
            Version = version;
            Resource = resource;
            HttpMethod = httpMethod;
            Action = action;
        }

        /// <summary>
        /// Sets the parameterized identifier.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        public void SetParameterizedIdentifier(NameValueCollection queryString)
        {
            ParameterizedIdentifiers = queryString != null ? new List<string>(queryString.AllKeys.Distinct(StringComparer.OrdinalIgnoreCase)) : null;
            ParameterizedIdentifiers.Sort();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return
                ParameterizedIdentifiers.HasItem() ?
                string.Format("{0}?{1}", ToUniqueDeclaration(true), ParameterizedIdentifiers.Join(seperator: "&")) :
                ToUniqueDeclaration(true);
        }

        /// <summary>
        /// Converts to unique declaration. Format: <c>{method}:[{realm}]/{version}/{resource}/[{action}]</c>
        /// </summary>
        /// <returns></returns>
        public string ToUniqueDeclaration(bool includeMethod = false)
        {
            return ToPath(includeMethod, null);
        }

        /// <summary>
        /// Converts to apiuniqueidentifier.
        /// </summary>
        /// <returns></returns>
        public ApiUniqueIdentifier ToApiUniqueIdentifier()
        {
            return new ApiUniqueIdentifier
            {
                HttpMethod = HttpMethod,
                Path = ToRoutePath(false)
            };
        }

        /// <summary>
        /// Converts to path.
        /// </summary>
        /// <param name="includeMethod">if set to <c>true</c> [include method].</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        private string ToPath(bool includeMethod, string prefix)
        {
            StringBuilder builder = new StringBuilder(256);
            if (includeMethod)
            {
                builder.Append(HttpMethod).Append(":");
            }
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                builder.Append(prefix);
            }
            if (!string.IsNullOrWhiteSpace(Realm))
            {
                builder.Append(Realm).Append("/");
            }
            builder.Append(Version).Append("/").Append(Resource).Append("/");
            if (!string.IsNullOrWhiteSpace(Action))
            {
                builder.Append(Action).Append("/");
            }
            return builder.ToString();
        }

        /// <summary>
        /// Converts to routepath. Format: <c>{method}:/api/[{realm}]/{version}/{resource}/[{action}]</c>
        /// </summary>
        /// <returns></returns>
        public string ToRoutePath(bool includeMethod = false)
        {
            return ToPath(includeMethod, "/api/");
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
            return obj is ApiRouteIdentifier && string.Equals(ToUniqueDeclaration(true), ((ApiRouteIdentifier)obj).ToUniqueDeclaration(true), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return ToUniqueDeclaration(true).ToLowerInvariant().GetHashCode();
        }

        /// <summary>
        /// Froms the API objects.
        /// </summary>
        /// <param name="apiContract">The API contract.</param>
        /// <param name="apiOperation">The API operation.</param>
        /// <returns></returns>
        public static ApiRouteIdentifier FromApiObjects(ApiContractAttribute apiContract, ApiOperationAttribute apiOperation)
        {
            var result = new ApiRouteIdentifier() { };

            if (apiContract != null)
            {
                result.Realm = apiContract.Realm;
                result.Version = apiContract.Version;
            }

            if (apiOperation != null)
            {
                result.Resource = apiOperation.ResourceName;
                result.Action = apiOperation.Action;
                result.HttpMethod = apiOperation.HttpMethod;
            }

            return result;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}