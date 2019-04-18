using System;
using Beyova.Diagnostic;

namespace Beyova.Api
{
    /// <summary>
    /// Class ApiPermissionAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class ApiPermissionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the permission identifier.
        /// </summary>
        /// <value>The permission identifier.</value>
        public string PermissionIdentifier { get; protected set; }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>The permission.</value>
        public ApiPermission Permission { get; protected set; }

        /// <summary>
        /// Gets or sets the exception behavior.
        /// </summary>
        /// <value>
        /// The exception behavior.
        /// </value>
        public ExceptionCode ExceptionBehavior { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiPermissionAttribute" /> class.
        /// </summary>
        /// <param name="permissionIdentifier">The permission identifier.</param>
        /// <param name="permission">The permission.</param>
        public ApiPermissionAttribute(string permissionIdentifier, ApiPermission permission = ApiPermission.Required)
        {
            PermissionIdentifier = permissionIdentifier;
            Permission = permission;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiPermissionAttribute"/> class.
        /// </summary>
        /// <param name="permissionIdentifier">The permission identifier.</param>
        /// <param name="exceptionBehavior">The exception behavior.</param>
        /// <param name="permission">The permission.</param>
        public ApiPermissionAttribute(string permissionIdentifier, ExceptionCode exceptionBehavior, ApiPermission permission = ApiPermission.Required)
            : this(permissionIdentifier, permission)
        {
            ExceptionBehavior = exceptionBehavior;
        }
    }
}