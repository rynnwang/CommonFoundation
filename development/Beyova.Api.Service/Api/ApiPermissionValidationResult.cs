namespace Beyova.Api
{
    /// <summary>
    ///
    /// </summary>
    public class ApiPermissionValidationResult
    {
        /// <summary>
        /// Gets or sets the permission identifier.
        /// </summary>
        /// <value>
        /// The permission identifier.
        /// </value>
        public string PermissionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the declared API permission rule.
        /// </summary>
        /// <value>
        /// The declared API permission rule.
        /// </value>
        public ApiPermissionAttribute DeclaredApiPermissionRule { get; set; }
    }
}