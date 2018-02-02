using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beyova;

namespace EF.E1Technology.Developer.Portal.Controllers
{
    public class ProvisioningController : Controller
    {
        // GET: Provisioning
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Nuget()
        {
            try
            {
                List<string> packages = new List<string>();

                var containerPath = ConfigurationManager.AppSettings.Get("packagesPath");
                if (!string.IsNullOrWhiteSpace(containerPath))
                {
                    containerPath = Server.MapPath(containerPath);
                    var directory = new DirectoryInfo(containerPath);

                    if (directory != null && directory.Exists)
                    {
                        foreach (var one in directory.GetFiles("*.nupkg"))
                        {
                            packages.Add(one.Name);
                        }
                    }
                }

                return View("NugetList", packages);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Handle("Nuget"));
            }


        }

        [HttpPost]
        public JsonResult UploadNuget()
        {
            return null;
        }

        [HttpPost]
        public JsonResult RemoveNugetPackage(string name)
        {
            return null;
        }
    }
}