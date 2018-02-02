using System;
using Beyova;
using Beyova.CommonAdminService;
using Beyova.CommonServiceInterface;
using E1.Content;
using EF.E1Technology.AuthenticationProfileService.Model;
using EF.E1Technology.Developer.Core;

namespace EF.E1Technology.Developer.Portal.Controllers
{
    /// <summary>
    /// Class AuthenticationController.
    /// </summary>
    public class AuthenticationController : AuthenticationProfileBaseController<UserInfo, UserCriteria, AuthenticationResult, FunctionalRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        public AuthenticationController() : base(ModuleCodes.AuthenticationProfileService)
        {
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>IAuthenticationProfileService&lt;TUserInfo, TUserCriteria, TAuthenticationResult, TFunctionalRole&gt;.</returns>
        protected override IAuthenticationProfileService<UserInfo, UserCriteria, AuthenticationResult, FunctionalRole> GetClient(EnvironmentEndpoint endpoint)
        {
            endpoint.CheckNullObject("endpoint");

            return new AuthenticationProfileService.AuthenticationProfileServiceApiClient(endpoint);
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
        /// Gets the view full path.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>System.String.</returns>
        protected override string GetViewFullPath(string viewName)
        {
            return string.Format(Beyova.CommonAdminService.Constants.ViewNames.BeyovaComponentDefaultViewPath, "Authentication", viewName);
        }
    }
}