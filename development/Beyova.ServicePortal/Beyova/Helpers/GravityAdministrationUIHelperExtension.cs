using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beyova.ServicePortal
{
    public static class GravityAdministrationUIHelperExtension
    {
        const string controllerName = "GravityAdministration";

        public static string CreateAdminUserPanelUrl(this UrlHelper helper, Guid? productKey)
        {
            return helper.Action("AdminUserInfo", controllerName, new { productKey = productKey });
        }

        public static string CreateAdminRolePanelUrl(this UrlHelper helper, Guid? productKey)
        {
            return helper.Action("AdminRole", controllerName, new { productKey = productKey });
        }

        public static string CreateAdminPermissionPanelUrl(this UrlHelper helper, Guid? productKey)
        {
            return helper.Action("AdminPermission", controllerName, new { productKey = productKey });
        }

        public static string CreateAdminUserRolePanelUrl(this UrlHelper helper, Guid? productKey, Guid? userKey)
        {
            return helper.Action("AdminUserRole", controllerName, new { productKey = productKey, userKey = userKey });
        }

        public static string CreateAdminRolePermissionPanelUrl(this UrlHelper helper, Guid? productKey, Guid? roleKey)
        {
            return helper.Action("AdminRolePermission", controllerName, new { productKey = productKey, roleKey = roleKey });
        }
    }
}