using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beyova;
using Beyova.Api;

using Beyova.Web;

namespace Beyova.ServicePortal.Controllers
{
    [RestApiContextConsistence]
    [TokenRequired]
    public class DashboardController : AdminBaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        public DashboardController() : base()
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ApiLatency()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ApiLatency(DateTime? fromStamp, DateTime? toStamp, string serviceIdentifier)
        {


            return View();
        }

        protected override string GetViewFullPath(string viewName)
        {
            return string.Format(Constants.ViewNames.BeyovaComponentDefaultViewPath, "Dashboard", viewName);
        }
    }
}