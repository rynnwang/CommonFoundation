using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beyova;
using Beyova.Web;

namespace Beyova.ServicePortal.Controllers
{
    [RestApiContextConsistence]
    public class HomeController : AdminBaseController
    {
        public ActionResult Index()
        {
            if (ContextHelper.CurrentCredential == null)
            {
                return RedirectToAction("Authenticate", "Security");
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }

        protected override string GetViewFullPath(string viewName)
        {
            return string.Format("~/Views/Home/{0}.cshtml", viewName);
        }
    }
}