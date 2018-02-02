using EF.E1Technology.Developer.Core;
using Beyova;
using EF.E1Technology.OdinBridge;
using EF.E1Technology.OnlineSchoolPlatform;
using EF.E1Technology.AuthenticationProfileService;
using EF.E1Technology.OdinIntegration;
using Beyova.CommonAdminService;
using System.Web.Mvc;
using System;
using Beyova.ExceptionSystem;
using Beyova.RestApi;
using EF.E1Technology.OnlineSchoolPlatform.Model;

namespace EF.E1Technology.Developer.Portal.Controllers
{
    /// <summary>
    /// Class BaseRemoteRestApiController.
    /// </summary>
    [EnvironmentBased]
    public abstract class BaseRemoteRestApiController : EnvironmentBaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRemoteRestApiController"/> class.
        /// </summary>
        /// <param name="moduleCode">The module code.</param>
        public BaseRemoteRestApiController(string moduleCode)
            : base(moduleCode)
        {
            this._moduleCode = moduleCode;
        }

        /// <summary>
        /// Gets the environment endpoint.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>EnvironmentEndpoint.</returns>
        protected override EnvironmentEndpoint GetEnvironmentEndpoint(string environment)
        {
            return ServiceConfigurationUtility.GetEndpoint(this._moduleCode, environment);
        }

        /// <summary>
        /// Gets the odin bridge client.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>OdinBridgeApiClient.</returns>
        protected OdinBridgeApiClient GetOdinBridgeClient(EnvironmentEndpoint endpoint)
        {
            return endpoint == null ? null : new OdinBridgeApiClient(endpoint, false);
        }

        /// <summary>
        /// Gets the odin rest client.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>OdinRestClient.</returns>
        protected OdinRestClient GetOdinRestClient(EnvironmentEndpoint endpoint)
        {
            return endpoint == null ? null : new OdinRestClient(endpoint, false);
        }

        /// <summary>
        /// Gets the online school platform rest API client.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>OnlineSchoolPlatformRestApiClient.</returns>
        protected OnlineSchoolPlatformRestApiClient GetOnlineSchoolPlatformRestApiClient(EnvironmentEndpoint endpoint)
        {
            return endpoint == null ? null : new OnlineSchoolPlatformRestApiClient(endpoint, false);
        }

        /// <summary>
        /// Gets the authentication profile service API client.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>AuthenticationProfileServiceApiClient.</returns>
        protected AuthenticationProfileServiceApiClient GetAuthenticationProfileServiceApiClient(EnvironmentEndpoint endpoint)
        {
            return endpoint == null ? null : new AuthenticationProfileServiceApiClient(endpoint, false);
        }

        /// <summary>
        /// Queries the course.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>JsonResult.</returns>
        public JsonResult QueryCourse(CourseNodeCriteria criteria, string environment)
        {
            object returnObject = null;
            BaseException exception = null;

            try
            {
                criteria.CheckNullObject("CourseNodeCriteria");

                var client = GetOnlineSchoolPlatformRestApiClient(ServiceConfigurationUtility.GetEndpoint("OnlineSchoolPlatform", environment));
                returnObject = client.QueryCourseNode(criteria);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(criteria);
            }

            ApiHandlerBase.PackageResponse(this.Response, returnObject, exception);
            return null;
        }
    }
}