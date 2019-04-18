using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyova.Diagnostic;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class RestApiExtension.
    /// </summary>
    public static class RestApiExtension
    {
        #region Route Url

        /// <summary>
        /// The module based URL regex format
        /// </summary>
        private const string moduleBasedUrlRegexFormat = @"(([^\/\?]+)/)?({0})/([0-9a-zA-Z\-_\.]+)/([^\/\?]+)(/([^\/\?]+))?(/(([^\/\?]+)))?(/)?";

        /// <summary>
        /// Gets the module based URL regex.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns></returns>
        internal static string GetModuleBasedUrlRegex(string moduleName)
        {
            return string.IsNullOrWhiteSpace(moduleName) ? string.Empty : string.Format(moduleBasedUrlRegexFormat, ConvertToModuleRegex(moduleName));
        }

        /// <summary>
        /// Converts to module regex.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns></returns>
        private static string ConvertToModuleRegex(string moduleName)
        {
            if (!string.IsNullOrWhiteSpace(moduleName))
            {
                StringBuilder builder = new StringBuilder(moduleName.Length * 4);
                foreach (char c in moduleName)
                {
                    if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || c == '_')
                    {
                        builder.AppendFormat("[{0}{1}]", char.ToUpperInvariant(c), char.ToLowerInvariant(c));
                    }
                }

                return builder.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// The API URL regex
        /// </summary>
        internal static readonly string apiUrlRegex = GetModuleBasedUrlRegex("api");

        #endregion Route Url

        /// <summary>
        /// To the dictionary.
        /// </summary>
        /// <param name="methodPermissionAttributes">The method permission attributes.</param>
        /// <returns>Dictionary&lt;System.String, ApiPermission&gt;.</returns>
        public static Dictionary<string, ApiPermissionAttribute> ToDictionary(this IEnumerable<ApiPermissionAttribute> methodPermissionAttributes)
        {
            Dictionary<string, ApiPermissionAttribute> result = new Dictionary<string, ApiPermissionAttribute>();

            if (methodPermissionAttributes != null)
            {
                foreach (var one in methodPermissionAttributes)
                {
                    result.Merge(one.PermissionIdentifier, one);
                }
            }

            return result;
        }

        /// <summary>
        /// Validates the API permission. Return the permission which cause validation failure.
        /// </summary>
        /// <param name="userPermissions">The user permissions.</param>
        /// <param name="methodPermissions">The method permissions.</param>
        /// <returns>System.Nullable&lt;KeyValuePair&lt;System.String, ApiPermission&gt;&gt;.</returns>
        public static ApiPermissionValidationResult ValidateApiPermission(this IList<string> userPermissions, IDictionary<string, ApiPermissionAttribute> methodPermissions)
        {
            if (methodPermissions == null)
            {
                return null;
            }

            userPermissions = userPermissions ?? new List<string>();

            // Check deny first
            foreach (var one in (from item in methodPermissions where item.Value.Permission == ApiPermission.Denied select item.Key))
            {
                if (userPermissions.Contains(one))
                {
                    return new ApiPermissionValidationResult
                    {
                        PermissionIdentifier = one,
                        DeclaredApiPermissionRule = methodPermissions[one]
                    };
                }
            }

            // Check required permissions
            foreach (var one in (from item in methodPermissions where item.Value.Permission == ApiPermission.Required select item.Key))
            {
                if (!userPermissions.Contains(one))
                {
                    return new ApiPermissionValidationResult
                    {
                        PermissionIdentifier = one,
                        DeclaredApiPermissionRule = methodPermissions[one]
                    };
                }
            }

            return null;
        }

        /// <summary>
        /// Validates the API permission.
        /// </summary>
        /// <param name="userPermissions">The user permissions.</param>
        /// <param name="methodPermissions">The method permissions.</param>
        /// <param name="token">The token.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>BaseException.</returns>
        public static BaseException ValidateApiPermission(this IList<string> userPermissions, IDictionary<string, ApiPermissionAttribute> methodPermissions, string token, string methodName)
        {
            const string defaultMinor = "ApiPermissionConstraint";

            BaseException result = null;
            var permissionValidationResult = userPermissions.ValidateApiPermission(methodPermissions);

            if (permissionValidationResult != null)
            {
                var data = new
                {
                    userPermissions,
                    methodPermissions,
                    result = new
                    {
                        permissionIdentifier = permissionValidationResult.PermissionIdentifier,
                        apiPermission = permissionValidationResult.DeclaredApiPermissionRule.Permission
                    }
                };

                if (permissionValidationResult.DeclaredApiPermissionRule?.ExceptionBehavior != null)
                {
                    var reason = permissionValidationResult.DeclaredApiPermissionRule.ExceptionBehavior.Minor.SafeToString(defaultMinor);

                    switch (permissionValidationResult.DeclaredApiPermissionRule.ExceptionBehavior.Major)
                    {
                        case ExceptionCode.MajorCode.UnauthorizedOperation:
                            result = new UnauthorizedOperationException(reason, data: data);
                            break;

                        case ExceptionCode.MajorCode.OperationForbidden:
                            result = new OperationForbiddenException(reason, data: data);
                            break;

                        default:
                            break;
                    }
                }

                if (result == null)
                {
                    result = new UnauthorizedOperationException(minorCode: defaultMinor, data: data);
                }
            }

            return result;
        }
    }
}