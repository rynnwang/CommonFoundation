using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyova.ExceptionSystem;

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
        internal readonly static string apiUrlRegex = GetModuleBasedUrlRegex("api");

        #endregion Route Url

        /// <summary>
        /// To the dictionary.
        /// </summary>
        /// <param name="methodPermissionAttributes">The method permission attributes.</param>
        /// <returns>Dictionary&lt;System.String, ApiPermission&gt;.</returns>
        public static Dictionary<string, ApiPermission> ToDictionary(this IEnumerable<ApiPermissionAttribute> methodPermissionAttributes)
        {
            Dictionary<string, ApiPermission> result = new Dictionary<string, ApiPermission>();

            if (methodPermissionAttributes != null)
            {
                foreach (var one in methodPermissionAttributes)
                {
                    result.Merge(one.PermissionIdentifier, one.Permission);
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
        public static KeyValuePair<string, ApiPermission>? ValidateApiPermission(this IList<string> userPermissions, IDictionary<string, ApiPermission> methodPermissions)
        {
            if (methodPermissions == null)
            {
                return null;
            }

            userPermissions = userPermissions ?? new List<string>();

            // Check deny first
            foreach (var one in (from item in methodPermissions where item.Value == ApiPermission.Denied select item.Key))
            {
                if (userPermissions.Contains(one))
                {
                    return new KeyValuePair<string, ApiPermission>(one, ApiPermission.Denied);
                }
            }

            // Check required permissions
            foreach (var one in (from item in methodPermissions where item.Value == ApiPermission.Required select item.Key))
            {
                if (!userPermissions.Contains(one))
                {
                    return new KeyValuePair<string, ApiPermission>(one, ApiPermission.Required);
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
        public static BaseException ValidateApiPermission(this IList<string> userPermissions, IDictionary<string, ApiPermission> methodPermissions, string token, string methodName)
        {
            var permissionValidationResult = userPermissions.ValidateApiPermission(methodPermissions);

            return (permissionValidationResult != null) ?
                new UnauthorizedOperationException(
                    minorCode: "ApiPermissionConstraint",
                     data: new
                     {
                         userPermissions,
                         methodPermissions,
                         result = permissionValidationResult
                     })
                : null;
        }
    }
}