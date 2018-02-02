using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beyova;
using Beyova.CommonAdminService;
using Beyova.ExceptionSystem;
using Beyova.RestApi;
using E1;
using EF.E1Technology.Developer.Core;

namespace EF.E1Technology.Developer.Portal.Controllers
{
    public class MobileTrackingController : EnvironmentBaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileTrackingController"/> class.
        /// </summary>
        public MobileTrackingController() : base("ApiTracking")
        {
        }

        // GET: MobileTracking
        public ActionResult Index()
        {
            return View("MobileErrorLogPanel");
        }

        [HttpGet]
        public ActionResult ErrorLog()
        {
            return View("MobileErrorLogPanel");
        }

        /// <summary>
        /// Errors the log detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult ErrorLogDetail(string id)
        {
            try
            {
                id.CheckEmptyString("id");
                var reader = GetMobileTrackReader();
                var rawData = reader.GetMobileErrorLogByElasticId(id);

                if (rawData == null || rawData.Source == null)
                {
                    return this.RenderAsNotFoundPage(string.Format("Mobile Error Log {0} is not found.", id));
                }
                return View("MobileErrorLogDetail", rawData);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToRedirection(ex, id);
            }
        }

        [HttpPost]
        public ActionResult ErrorLog(MobileErrorLogCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");
                var reader = GetMobileTrackReader();

                var logs = reader.QueryMobileErrorLog(criteria);
                return this.PartialView("_MobileErrorLogList", logs);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, criteria);
            }
        }

        public ActionResult QueryErrorPie()
        {
            return null;
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

        protected MobileTrackReader GetMobileTrackReader()
        {
            var endpoint = GetEnvironmentEndpoint(CurrentEnvironment);
            return endpoint == null ? null : new MobileTrackReader(string.Format("{0}://{1}:{2}/", endpoint.Protocol, endpoint.Host, endpoint.Port));
        }

        /// <summary>
        /// Gets the view full path.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>System.String.</returns>
        protected override string GetViewFullPath(string viewName)
        {
            return string.Format("~/Views/MobileTracking/{0}.cshtml", viewName);
        }
    }
}