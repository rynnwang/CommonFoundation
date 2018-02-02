using System;
using System.Web.Mvc;
using Beyova;
using Beyova.ExceptionSystem;
using EF.E1Technology.Developer.Core;
using Beyova.RestApi;
using EF.E1Technology.OdinBridge.Model;
using E1.Content;

namespace EF.E1Technology.Developer.Portal.Controllers
{
    public class OdinBridgeController : BaseRemoteRestApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdinBridgeController"/> class.
        /// </summary>
        public OdinBridgeController() : base(ModuleCodes.OdinBridge)
        {
        }

        public ActionResult Index()
        {
            return View("RestClientPanel");
        }

        public ActionResult RestClient()
        {
            return View("RestClientPanel");
        }

        #region HomeworkUnlock

        [HttpGet]
        public ActionResult HomeworkUnlock()
        {
            var client = GetOnlineSchoolPlatformRestApiClient(ServiceConfigurationUtility.GetEndpoint("OnlineSchoolPlatform", CurrentEnvironment));

            var courseNodes = client.QueryCourseNode(new OnlineSchoolPlatform.Model.CourseNodeCriteria { Level = CourseLevel.Product });
            return View("HomeworkUnlockPanel", courseNodes);
        }

        /// <summary>
        /// Homeworks the unlock.
        /// </summary>
        /// <param name="homeworkEvent">The homework event.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult HomeworkUnlock(OdinBridge.Model.OdinStudentHomeworkEvent homeworkEvent, string environment)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                homeworkEvent.CheckNullObject("homeworkEvent");

                var client = GetOdinBridgeClient(GetEnvironmentEndpoint(environment));
                client.PushOdinStudentHomeworkEvent(homeworkEvent);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { homeworkEvent, environment });
            }

            ApiHandlerBase.PackageResponse(Response, data: result, ex: exception);
            return null;
        }

        #endregion



        #region Raw

        [HttpPost]
        public JsonResult StudentAuthenticate(AuthenticationRequest request, string environment)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                request.CheckNullObject("request");

                var client = GetOdinRestClient(GetEnvironmentEndpoint(environment));
                result = client.StudentAuthenticate(request);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { request, environment });
            }

            ApiHandlerBase.PackageResponse(Response, data: result, ex: exception);
            return null;
        }

        [HttpPost]
        public JsonResult TeacherAuthenticate(AuthenticationRequest request, string environment)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                request.CheckNullObject("request");

                var client = GetOdinRestClient(GetEnvironmentEndpoint(environment));
                result = client.TeacherAuthenticate(request);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { request, environment });
            }

            ApiHandlerBase.PackageResponse(Response, data: result, ex: exception);
            return null;
        }

        [HttpPost]
        public ActionResult OdinStudentProfile(string id, string environment)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                id.CheckEmptyString("id");

                var client = GetOdinRestClient(GetEnvironmentEndpoint(environment));
                result = client.GetStudentUserInfo(id.ToInt32());
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { id, environment });
            }

            ApiHandlerBase.PackageResponse(Response, data: result, ex: exception);
            return null;
        }

        [HttpPost]
        public ActionResult OdinTeacherProfile(string id, string environment)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                id.CheckEmptyString("id");

                var client = GetOdinRestClient(GetEnvironmentEndpoint(environment));
                result = client.GetTeacherUserInfo(id.ToInt32());
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { id, environment });
            }

            ApiHandlerBase.PackageResponse(Response, data: result, ex: exception);
            return null;
        }

        [HttpPost]
        public ActionResult OdinGroupMembers(string id, string environment)
        {
            object result = null;
            BaseException exception = null;

            try
            {
                id.CheckEmptyString("id");

                var client = GetOdinRestClient(GetEnvironmentEndpoint(environment));
                result = client.GetUserInfosByGroupId(id.ToInt32());
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { id, environment });
            }

            ApiHandlerBase.PackageResponse(Response, data: result, ex: exception);
            return null;
        }

        protected override string GetViewFullPath(string viewName)
        {
            return string.Format("~/Views/OdinBridge/{0}.cshtml", viewName);
        }

        #endregion
    }
}