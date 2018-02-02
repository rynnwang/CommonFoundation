using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beyova;
using Beyova.CommonAdminService;
using Beyova.Elastic;
using EF.E1Technology.Developer.Core;

namespace EF.E1Technology.Developer.Portal.Controllers
{
    public class KibanaController : Controller
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return View("Index", ServiceConfigurationUtility.kibanas);
        }
    }
}