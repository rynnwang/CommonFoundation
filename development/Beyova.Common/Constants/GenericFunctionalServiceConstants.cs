namespace Beyova
{
    /// <summary>
    /// Class GenericFunctionalServiceConstants
    /// </summary>
    public static class GenericFunctionalServiceConstants
    {
        /// <summary>
        /// Class Permission.
        /// </summary>
        public class Permission
        {
            /// <summary>
            /// The remote configuration administrator
            /// </summary>

            /// <summary>
            /// The authentication administrator
            /// </summary>
            public const string AuthenticationAdministrator = "AuthenticationAdministrator";

            /// <summary>
            /// The administration owner
            /// </summary>
            public const string AdministrationOwner = "AdministrationOwner";

            /// <summary>
            /// The administrator
            /// </summary>
            public const string Administrator = "Administrator";

            /// <summary>
            /// Class AdminSystem.
            /// </summary>
            public static class AdminSystem
            {
                /// <summary>
                /// The admin user administration. Used for operate user, user-role mapping.
                /// </summary>
                public const string AdminUserAdministration = "AdminUserAdministration";

                /// <summary>
                /// The admin role permission administration. Used for operate role &amp; permission, role-permission mapping.
                /// </summary>
                public const string AdminRolePermissionAdministration = "AdminRolePermissionAdministration";

                /// <summary>
                /// The admin user role view
                /// </summary>
                public const string AdminUserRoleView = "AdminUserRoleView";
            }
        }

        /// <summary>
        /// Class ActionName.
        /// </summary>
        public class ActionName
        {
            /// <summary>
            /// The snapshot
            /// </summary>
            public const string Snapshot = "Snapshot";

            /// <summary>
            /// The create
            /// </summary>
            public const string Create = "Create";

            /// <summary>
            /// The commit
            /// </summary>
            public const string Commit = "Commit";

            /// <summary>
            /// The query
            /// </summary>
            public const string Query = "Query";

            /// <summary>
            /// The search
            /// </summary>
            public const string Search = "Search";
        }

        /// <summary>
        /// Class ResourceName.
        /// </summary>
        public class ResourceName
        {
            /// <summary>
            /// The product
            /// </summary>
            public const string Product = "Product";

            /// <summary>
            /// The transaction
            /// </summary>
            public const string Transaction = "Transaction";

            /// <summary>
            /// The configuration
            /// </summary>
            public const string Configuration = "Configuration";

            /// <summary>
            /// The central authentication
            /// </summary>
            public const string CentralAuthentication = "CentralAuthentication";

            /// <summary>
            /// The admin user information
            /// </summary>
            public const string AdminUserInfo = "AdminUserInfo";

            /// <summary>
            /// The admin role
            /// </summary>
            public const string AdminRole = "AdminRole";

            /// <summary>
            /// The admin permission
            /// </summary>
            public const string AdminPermission = "AdminPermission";

            /// <summary>
            /// The admin privilege
            /// </summary>
            public const string AdminPrivilege = "AdminPrivilege";

            /// <summary>
            /// The history
            /// </summary>
            public const string History = "History";

            /// <summary>
            /// The EULA
            /// </summary>
            public const string EULA = "EULA";

            /// <summary>
            /// The trade
            /// </summary>
            public const string Trade = "Trade";

            /// <summary>
            /// The finance audit
            /// </summary>
            public const string FinanceAudit = "FinanceAudit";
        }

        /// <summary>
        ///
        /// </summary>
        public class ServiceName
        {
            /// <summary>
            /// The user preference service
            /// </summary>
            public const string UserPreferenceService = "UserPreferenceService";
        }
    }
}