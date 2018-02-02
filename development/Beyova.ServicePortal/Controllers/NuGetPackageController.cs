using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beyova;
using Beyova.Api;
using Beyova.CommonAdminService;

namespace EF.E1Technology.Developer.Portal.Controllers
{
    [TokenRequired]
    public sealed class NuGetPackageController : AdminBaseController
    {
        private static DirectoryInfo feedDirectory = new DirectoryInfo(System.Configuration.ConfigurationManager.AppSettings["packagesPath"]);

        // GET: NuGetPackage
        public ActionResult Index()
        {
            return View(GetViewFullPath("Index"));
        }

        /// <summary>
        /// Deletes the package.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult DeletePackage(string name)
        {
            string result;

            try
            {
                name.CheckEmptyString(nameof(name));

                var fileName = Path.Combine(feedDirectory.FullName, name);
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }

                result = string.Format("{0} is deleted.", name);
            }
            catch (Exception ex)
            {
                result = ex.Handle(data: name).ToExceptionInfo().ToJson();
            }

            return Json(result);
        }

        /// <summary>
        /// Uploads the package.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult UploadPackage()
        {
            string result = null;

            try
            {
                var files = Request.Files;
                if (files != null && files.Count > 0)
                {
                    var package = files[0];
                    var name = package.FileName;

                    if (feedDirectory.GetFiles(name).Any())
                    {
                        result = "File existed.";
                    }
                    else
                    {
                        package.SaveAs(Path.Combine(feedDirectory.FullName, name));
                        result = "Done.";
                    }
                }
                else
                {
                    result = "No file attached.";
                }
            }
            catch (Exception ex)
            {
                result = ex.Handle().ToExceptionInfo().ToJson();
            }

            return Json(result);
        }

        /// <summary>
        /// Gets the view full path.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>System.String.</returns>
        protected override string GetViewFullPath(string viewName)
        {
            return string.Format("~/Views/NuGetPackage/{0}.cshtml", viewName);
        }

        /// <summary>
        /// Gets the feed directory.
        /// </summary>
        /// <returns>DirectoryInfo.</returns>
        public static DirectoryInfo GetFeedDirectory()
        {
            return feedDirectory;
        }
    }
}