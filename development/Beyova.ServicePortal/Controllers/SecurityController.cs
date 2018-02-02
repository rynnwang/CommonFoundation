using System;
using System.Web.Mvc;
using Beyova;
using Beyova.Api;
using Beyova.Api.RestApi;

namespace Beyova.ServicePortal.Controllers
{
    /// <summary>
    /// Class SecurityController.
    /// </summary>
    public class SecurityController : Controller
    {
        /// <summary>
        /// The service
        /// </summary>
        static CentralAuthentication.Server.CentralAuthenticationServer service = new CentralAuthentication.Server.CentralAuthenticationServer();

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return RedirectToAction("Authenticate");
        }

        /// <summary>
        /// Authenticates the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [TokenRequired(false)]
        public ActionResult Authenticate(string email = null, string returnUrl = null)
        {
            return View("Authenticate");
        }

        [TokenRequired(true)]
        public ActionResult Logout()
        {
            service.DisposeSession(ContextHelper.Token);
            return RedirectToAction("Authenticate");
        }

        /// <summary>
        /// Logins the specified login request.
        /// </summary>
        /// <param name="accessCredential">The login request.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [TokenRequired(false)]
        public void Authenticate(AuthenticationRequest request)
        {
            try
            {
                var session = service.Authenticate(request);
                ApiHandlerBase.PackageResponse(Response, session);
            }
            catch (Exception ex)
            {
                ApiHandlerBase.PackageResponse(Response, null, ex.Handle(request));
            }
        }
    }
}