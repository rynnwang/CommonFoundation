using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beyova;
using Beyova.Api;
using Beyova.CommonAdminService;
using Beyova.ExceptionSystem;
using Beyova.RestApi;
using EF.E1Technology.Developer.Core;
using EF.E1Technology.OnlineSchoolPlatform.Model;
using EF.E1Technology.Developer.Portal.Common;
using EF.E1Technology.Developer.Portal.Model;

namespace EF.E1Technology.Developer.Portal.Controllers
{
    [TokenRequired]
    public class MotivationController : BaseRemoteRestApiController
    {
        public MotivationController() : base(ModuleCodes.OnlineSchoolPlatform)
        {
        }

        #region Motivation Type

        [HttpGet]
        public ActionResult MotivationType()
        {
            return View("MotivationTypePanel");
        }

        /// <summary>
        /// Motivations the type.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public PartialViewResult MotivationType(MotivationTypeCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                var client = GetOnlineSchoolPlatformRestApiClient(GetEnvironmentEndpoint(CurrentEnvironment));
                return PartialView("_MotivationTypeList", client.QueryMotivationType(criteria));
            }
            catch (Exception ex)
            {
                return HandleExceptionToPartialView(ex, criteria);
            }
        }

        /// <summary>
        /// Creates the type of the motivation.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult CreateMotivationType(string environment)
        {
            return View("MotivationTypeDetail", null);
        }

        [HttpPost]
        public JsonResult CreateMotivationType(MotivationType entity, string environment)
        {
            object returnObject = null;
            BaseException exception = null;

            try
            {
                entity.CheckNullObject("entity");

                var client = GetOnlineSchoolPlatformRestApiClient(GetEnvironmentEndpoint(CurrentEnvironment));
                returnObject = client.CreateOrUpdateMotivationType(entity);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(entity);
            }

            ApiHandlerBase.PackageResponse(this.Response, returnObject, exception);
            return null;
        }

        /// <summary>
        /// Motivations the type detail.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult MotivationTypeDetail(Guid? key, bool editMode = false)
        {
            try
            {
                MotivationType type = null;
                if (key != null)
                {
                    var client = GetOnlineSchoolPlatformRestApiClient(GetEnvironmentEndpoint(CurrentEnvironment));
                    type = client.QueryMotivationType(new MotivationTypeCriteria { Key = key }).FirstOrDefault();
                }

                if (type == null)
                {
                    return this.RenderAsNotFoundPage();
                }

                ViewBag.IsEdit = editMode;
                return View("MotivationTypeDetail", type);
            }
            catch (Exception ex)
            {
                return HandleExceptionToRedirection(ex, key);
            }
        }

        #endregion

        #region Motivation Shelf

        public ActionResult MotivationRewardShelf(Guid? scopeKey)
        {
            return View();
        }

        #endregion

        #region Motivation Point Audit

        public ActionResult MotivationPointAuditSearch()
        {
            return View();
        }
        public PartialViewResult MotivationPointAuditList()
        {
            return PartialView();
        }
        [HttpPost]
        public PartialViewResult MotivationPointAuditList(MotivationPointAuditCriteria criteria)
        {
            try
            {
                var endpoint = GetEnvironmentEndpoint(CurrentEnvironment);

                if (endpoint != null)
                {
                    var client = GetOnlineSchoolPlatformRestApiClient(endpoint);
                    var model = client.QueryMotivationPointAudit(criteria);
                    return PartialView(model);
                }
                return PartialView();
            }
            catch (Exception)
            {
                return PartialView();
            }
        }
        public ActionResult MotivationPointAuditDetail(Guid key)
        {
            var endpoint = GetEnvironmentEndpoint(CurrentEnvironment);
            if (endpoint != null)
            {
                var client = GetOnlineSchoolPlatformRestApiClient(endpoint);
                var model = client.QueryMotivationPointAudit(new MotivationPointAuditCriteria() { AuditKey = key });
                return View(model.FirstOrDefault());
            }
            return View();
        }
        public ActionResult ModifyPoint()
        {
            ViewBag.FeatureType = MotivationTaskFeatureType.HomeworkGoal.GetList();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ModifyPoint(ModifyMotivationPointModel model)
        {
            try
            {
                ModifyMotivationPointRequest criteria = new ModifyMotivationPointRequest()
                {
                    UserId = model.UserId,
                    FeatureType = model.FeatureType,
                    Description = model.Description,
                    PointAmount = model.PointAmount
                };



                criteria.ScopeKey = model.ScopeKey.ToGuid();
                if (criteria.ScopeKey == null) throw new ArgumentException(@"ScopeKey must be Guid ");

                var userKey = Request.Form["UserKeyGuid"];
                if (!string.IsNullOrEmpty(model.UserKey))
                {
                    criteria.UserKey = model.UserKey.ToGuid();
                    if (criteria.UserKey == null) throw new ArgumentException(@"UserKey must be Guid ");
                }

                criteria.OperatorKey = ContextHelper.CurrentUserInfo.Key;
                var endpoint = GetEnvironmentEndpoint(CurrentEnvironment);
                if (endpoint != null)
                {
                    //endpoint.Host = "localhost"; 
                    var client = GetOnlineSchoolPlatformRestApiClient(endpoint);

                    var result = client.ModifyMotivationPoint(criteria);
                    if (result != null)
                    {
                        return RedirectToAction("ModifyPointResult", model);
                    }
                    else
                    {
                        throw new ArgumentException("Check input");
                    }
                }
            }
            catch (ArgumentException ae)
            {
                ViewBag.ErrMes = ae.Message;
            }
            catch (Exception)
            {
                ViewBag.ErrMes = "Fail. Please check input.";
            }

            ViewBag.FeatureType = MotivationTaskFeatureType.HomeworkGoal.GetList();
            return View(model);
        }
        public ActionResult ModifyPointResult(ModifyMotivationPointModel model)
        {
            return View(model);
        }
        #endregion

        /// <summary>
        /// Gets the view full path.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>System.String.</returns>
        protected override string GetViewFullPath(string viewName)
        {
            return string.Format(Constants.ViewNames.BeyovaComponentDefaultViewPath, "Motivation", viewName);
        }
    }
}