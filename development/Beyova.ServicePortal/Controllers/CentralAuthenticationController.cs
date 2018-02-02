using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beyova.CentralAuthentication.Server;
using Beyova.ExceptionSystem;
using Beyova.Gravity;
using Beyova.Web;

namespace Beyova.ServicePortal.Controllers
{
    /// <summary>
    /// Class CentralAuthenticationController. This class cannot be inherited.
    /// </summary>
    [ProductBasedAction("productKey")]
    public sealed class CentralAuthenticationController : BeyovaPortalController
    {
        const string moduleName = "GravityAdministration";

        /// <summary>
        /// The service core
        /// </summary>
        static GravityManagementServiceCore serviceCore = new GravityManagementServiceCore();

        /// <summary>
        /// The central authentication server
        /// </summary>
        static CentralAuthenticationServer centralAuthenticationServer = new CentralAuthenticationServer();

        /// <summary>
        /// Initializes a new instance of the <see cref="CentralAuthenticationController"/> class.
        /// </summary>
        public CentralAuthenticationController()
            : base(moduleName, Framework.ApiTracking, true)
        {
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return RedirectToAction("ProductInfo");
        }

        #region Product Info

        /// <summary>
        /// Products the information.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult ProductInfo()
        {
            return this.RenderAsPanelView<ProductInfo>();
        }

        /// <summary>
        /// Queries the product information.
        /// </summary>
        /// <returns>System.Web.Mvc.ActionResult.</returns>
        [HttpPost]
        public ActionResult QueryProductInfo()
        {
            try
            {
                var products = serviceCore.QueryProductInfo(new ProductCriteria { });
                return RenderAsListPartialView(products);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, null);
            }
        }

        /// <summary>
        /// Gets the product information.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult GetProductInfo(Guid? key)
        {
            try
            {
                key.CheckNullObject(nameof(key));
                var product = serviceCore.QueryProductInfo(new ProductCriteria { Key = key }).FirstOrDefault();
                return Json(product);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, key);
            }
        }

        /// <summary>
        /// Renews the product security information.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult RenewProductSecurityInfo(Guid? key)
        {
            try
            {
                key.CheckNullObject(nameof(key));
                serviceCore.RenewProductSecurityInfo(key);
                var product = serviceCore.QueryProductInfo(new ProductCriteria { Key = key }).FirstOrDefault();
                return Json(product);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, key);
            }
        }

        /// <summary>
        /// Creates the or update product.
        /// </summary>
        /// <param name="productInfo">The product information.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateOrUpdateProduct(ProductInfo productInfo)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                productInfo.CheckNullObject(nameof(productInfo));
                result = serviceCore.CreateOrUpdateProduct(productInfo);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(productInfo);
            }

            return this.ReturnAsJson(exception, productInfo, result);
        }

        //[HttpPost]
        //public ActionResult DeleteProductInfo(Guid? key)
        //{
        //    try
        //    {
        //        key.CheckNullObject(nameof(key));
        //        serviceCore.RenewProductSecurityInfo(key);
        //        return Json(product);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.HandleExceptionToPartialView(ex, key);
        //    }
        //}

        #endregion

        #region UserInfo

        [HttpGet]
        public ActionResult AdminUserInfo()
        {
            return this.RenderAsPanelView<AdminUserInfo>();
        }

        //[HttpGet]
        //public ActionResult ResetUserPassword(Guid? userKey)
        //{
        //    try
        //    {
        //        userKey.CheckNullObject(nameof(userKey));

        //        var user = centralAuthenticationServer.QueryAdminUser(new AdminUserCriteria { Key = userKey }).FirstOrDefault()?.Object;
        //        user.CheckNullObject(nameof(user));

        //        user.ValidateProductScope();

        //        centralAuthenticationServer.reset
        //        return RenderAsListPartialView(users);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.HandleExceptionToPartialView(ex, null);
        //    }

        //    return View(this.GetComponentViewPath("ResetUserPassword"))
        //}

        /// <summary>
        /// Queries the admin user information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryAdminUserInfo(AdminUserCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var users = centralAuthenticationServer.QueryAdminUser(criteria).GetUnderlyingObjects();
                return RenderAsListPartialView(users);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, null);
            }
        }

        /// <summary>
        /// Gets the admin user information.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAdminUserInfo(Guid? key)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                key.CheckNullObject(nameof(key));

                var user = centralAuthenticationServer.QueryAdminUser(new AdminUserCriteria { Key = key }).FirstOrDefault();
                if (user?.Object.ValidateProductScope() ?? false)
                {
                    result = user.Object;
                }
            }
            catch (Exception ex)
            {
                exception = ex.Handle(key);
            }

            return this.ReturnAsJson(exception, key, result);
        }

        /// <summary>
        /// Creates the or update admin user information.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateOrUpdateAdminUserInfo(AdminUserInfo userInfo)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                userInfo.CheckNullObject(nameof(userInfo));

                result = centralAuthenticationServer.CreateOrUpdateAdminUser(userInfo);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(userInfo);
            }

            return this.ReturnAsJson(exception, userInfo, result);
        }

        /// <summary>
        /// Deletes the admin user information.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAdminUserInfo(Guid? key)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                key.CheckNullObject(nameof(key));

                var user = centralAuthenticationServer.QueryAdminUser(new AdminUserCriteria { Key = key }).FirstOrDefault();
                if (user?.Object.ValidateProductScope() ?? false)
                {
                    centralAuthenticationServer.DeleteAdminUser(user.Key);
                    result = user;
                }
            }
            catch (Exception ex)
            {
                exception = ex.Handle(key);
            }

            return this.ReturnAsJson(exception, key, result);
        }

        #endregion

        #region RoleInfo

        /// <summary>
        /// Admins the role.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AdminRole()
        {
            return this.RenderAsPanelView<AdminRole>();
        }

        /// <summary>
        /// Creates the or update admin role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateOrUpdateAdminRole(AdminRole role)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                role.CheckNullObject(nameof(role));
                role.EnsureProductScope();

                result = centralAuthenticationServer.CreateOrUpdateAdminRole(role);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(role);
            }

            return this.ReturnAsJson(exception, role, result);
        }

        /// <summary>
        /// Queries the admin role.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryAdminRole(AdminRoleCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var roles = centralAuthenticationServer.QueryAdminRole(criteria).GetUnderlyingObjects();
                return RenderAsListPartialView(roles);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, null);
            }
        }

        /// <summary>
        /// Gets the admin role.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAdminRole(Guid? key)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                key.CheckNullObject(nameof(key));

                var user = centralAuthenticationServer.QueryAdminRole(new AdminRoleCriteria { Key = key }).FirstOrDefault();
                if (user?.Object.ValidateProductScope() ?? false)
                {
                    result = user;
                }
            }
            catch (Exception ex)
            {
                exception = ex.Handle(key);
            }

            return this.ReturnAsJson(exception, key, result);
        }

        #endregion

        #region Permission

        /// <summary>
        /// Admins the permission information.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AdminPermission()
        {
            return this.RenderAsPanelView<AdminPermission>();
        }

        /// <summary>
        /// Creates the or update admin permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateOrUpdateAdminPermission(AdminPermission permission)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                permission.CheckNullObject(nameof(permission));

                result = centralAuthenticationServer.CreateOrUpdateAdminPermission(permission);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(permission);
            }

            return this.ReturnAsJson(exception, permission, result);
        }

        /// <summary>
        /// Queries the admin permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryAdminPermission(AdminPermissionCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var permissions = centralAuthenticationServer.QueryAdminPermission(criteria);
                return RenderAsListPartialView(permissions);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, null);
            }
        }

        [HttpPost]
        public ActionResult DeleteAdminPermission(Guid? key)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                key.CheckNullObject(nameof(key));

                var permission = centralAuthenticationServer.QueryAdminPermission(new AdminPermissionCriteria { Key = key }).FirstOrDefault();
                if (permission.ValidateProductScope())
                {
                    centralAuthenticationServer.DeleteAdminPermission(permission.Key);
                    result = permission;
                }
            }
            catch (Exception ex)
            {
                exception = ex.Handle(key);
            }

            return this.ReturnAsJson(exception, key, result);
        }

        #endregion

        #region User-Role

        [HttpGet]
        public ActionResult AdminUserRole(Guid? userKey)
        {
            try
            {
                userKey.CheckNullObject(nameof(userKey));

                var user = centralAuthenticationServer.QueryAdminUser(new AdminUserCriteria { Key = userKey }).FirstOrDefault()?.Object;
                user.CheckNullObject(nameof(user));
                if (!CentralManagementContext.ValidateProductScope(user))
                {
                    throw new UnauthorizedOperationException("UserOwnership");
                }

                var bindings = centralAuthenticationServer.GetUserRoleBindings(user.Key);
                var roles = centralAuthenticationServer.QueryAdminRole(new AdminRoleCriteria { ProductKey = user.ProductKey }).GetUnderlyingObjects();
                return View(this.GetDefaultPanelViewPath("AdminUserRoleBinding"), new Tuple<AdminUserInfo, List<AdminRole>, List<AdminRoleBinding>>(user, roles, bindings));
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToRedirection(ex, userKey);
            }
        }

        /// <summary>
        /// Binds the role on user.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BindRoleOnUser(AdminRoleBinding binding)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                binding.CheckNullObject(nameof(binding));

                result = centralAuthenticationServer.BindRoleOnUser(binding);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(binding);
            }

            return this.ReturnAsJson(exception, binding, result);
        }

        /// <summary>
        /// Unbinds the role on user.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UnbindRoleOnUser(AdminRoleBinding binding)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                binding.CheckNullObject(nameof(binding));

                centralAuthenticationServer.UnbindRoleOnUser(binding);
                result = "";
            }
            catch (Exception ex)
            {
                exception = ex.Handle(binding);
            }

            return this.ReturnAsJson(exception, binding, result);
        }

        /// <summary>
        /// Deletes the admin role.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAdminRole(Guid? key)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                key.CheckNullObject(nameof(key));

                centralAuthenticationServer.DeleteAdminRole(key);
                result = "";
            }
            catch (Exception ex)
            {
                exception = ex.Handle(key);
            }

            return this.ReturnAsJson(exception, key, result);
        }

        #endregion

        #region Role-Permission

        [HttpGet]
        public ActionResult AdminRolePermission(Guid? roleKey)
        {
            try
            {
                roleKey.CheckNullObject(nameof(roleKey));

                var role = centralAuthenticationServer.QueryAdminRole(new AdminRoleCriteria { Key = roleKey }).FirstOrDefault()?.Object;
                role.CheckNullObject(nameof(role));
                if (!CentralManagementContext.ValidateProductScope(role))
                {
                    throw new UnauthorizedOperationException("RoleOwnership");
                }

                var bindings = centralAuthenticationServer.GetRolePermissionBindings(role.Key);
                var permissions = centralAuthenticationServer.QueryAdminPermission(new AdminPermissionCriteria { ProductKey = role.ProductKey });
                return View(this.GetDefaultPanelViewPath("AdminRolePermissionBinding"), new Tuple<AdminRole, List<AdminPermission>, List<AdminPermissionBinding>>(role, permissions, bindings));
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToRedirection(ex, roleKey);
            }
        }

        [HttpPost]
        public ActionResult BindPermissionOnRole(AdminPermissionBinding binding)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                binding.CheckNullObject(nameof(binding));

                result = centralAuthenticationServer.BindPermissionOnRole(binding);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(binding);
            }

            return this.ReturnAsJson(exception, binding, result);
        }

        /// <summary>
        /// Unbinds the role on user.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UnbindPermissionOnRole(AdminPermissionBinding binding)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                binding.CheckNullObject(nameof(binding));

                centralAuthenticationServer.UnbindPermissionOnRole(binding);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(binding);
            }

            return this.ReturnAsJson(exception, binding, result);
        }

        #endregion

        public void Test()
        {

        }
    }
}
